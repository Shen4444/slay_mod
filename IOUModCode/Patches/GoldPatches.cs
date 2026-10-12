using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Rooms;

namespace IOUMod.Patches;

/// <summary>
/// The game clamps gold at 0 whenever it is lost. With the IOU:
/// - shop purchases may take gold below 0 (that is the debt), and the gold borrowed this way costs extra interest;
/// - any other loss while at or below 0 does nothing, so debt is never forgiven or increased.
/// </summary>
[HarmonyPatch(typeof(PlayerCmd), nameof(PlayerCmd.LoseGold))]
public static class LoseGoldPatch
{
	public static bool Prefix(decimal amount, Player player, GoldLossType goldLossType, ref Task __result)
	{
		var relic = IouRelic.ActiveFor(player);
		if (relic == null)
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
			int interest = IouRelic.InterestFor(player.Gold, loss);
			player.Gold -= loss + interest;
			if (interest > 0)
			{
				relic.OnInterestCharged();
			}
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

/// <summary>Keeps the debt counter in sync with gold and uses up the IOU once a debt is repaid.</summary>
[HarmonyPatch(typeof(Player), nameof(Player.Gold), MethodType.Setter)]
public static class GoldChangedPatch
{
	public static void Prefix(Player __instance, out int __state)
	{
		__state = __instance.Gold;
	}

	public static void Postfix(Player __instance, int __state)
	{
		IouRelic.For(__instance)?.OnGoldChanged(__state, __instance.Gold);
	}
}
