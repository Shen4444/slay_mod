using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace IOUMod.Patches;

/// <summary>
/// The game clamps gold at 0 whenever it is lost. With the IOU:
/// - shop purchases may take gold below 0 (that is the debt);
/// - any other loss while at or below 0 does nothing, so debt is never forgiven or increased.
/// </summary>
[HarmonyPatch(typeof(PlayerCmd), nameof(PlayerCmd.LoseGold))]
public static class LoseGoldPatch
{
	public static bool Prefix(decimal amount, Player player, GoldLossType goldLossType, ref Task __result)
	{
		if (IouRelic.For(player) == null)
		{
			return true;
		}

		int loss = (int)amount;
		bool isShopPurchase = goldLossType == GoldLossType.Spent && player.RunState.CurrentRoom is MerchantRoom;

		if (isShopPurchase && loss > player.Gold)
		{
			SfxCmd.Play("event:/sfx/ui/gold/gold_1");
			var history = player.RunState.CurrentMapPointHistoryEntry?.GetEntry(player.NetId);
			if (history != null)
			{
				history.GoldSpent += loss;
			}
			player.Gold -= loss;
			__result = Task.CompletedTask;
			return false;
		}

		if (!isShopPurchase && player.Gold <= 0)
		{
			__result = Task.CompletedTask;
			return false;
		}

		return true;
	}
}

/// <summary>Marks when gold is being claimed from a combat reward, which is the only gold the IOU reduces.</summary>
[HarmonyPatch(typeof(GoldReward), "OnSelect")]
public static class CombatGoldRewardPatch
{
	public static bool IsClaiming { get; private set; }

	// OnSelect is async, but the game applies gold modifiers before its first await,
	// so the flag is set for exactly that synchronous part.
	public static void Prefix(GoldReward __instance)
	{
		// Gold stolen by an enemy and won back was the player's own money, so it is not reduced.
		IsClaiming = __instance.Player.RunState.CurrentRoom is CombatRoom && !__instance._wasGoldStolenBack;
	}

	public static void Postfix()
	{
		IsClaiming = false;
	}
}

/// <summary>Keeps the debt counter on the relic icon in sync with gold.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.Gold), MethodType.Setter)]
public static class GoldChangedPatch
{
	public static void Postfix(Player __instance)
	{
		IouRelic.For(__instance)?.RefreshCounter();
	}
}
