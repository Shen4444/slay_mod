using System.Runtime.CompilerServices;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;

namespace IOUMod.Patches;

/// <summary>
/// Every normal merchant gets one extra relic entry selling the IOU until the player owns it.
/// The entry is kept outside the inventory's relic list, because the shop screen only has
/// slots for the standard relics.
/// </summary>
[HarmonyPatch(typeof(MerchantInventory), nameof(MerchantInventory.CreateForNormalMerchant))]
public static class AddIouToMerchantPatch
{
	private static readonly ConditionalWeakTable<MerchantInventory, MerchantRelicEntry> IouEntries = new();

	public static bool TryGetIouEntry(MerchantInventory inventory, out MerchantRelicEntry entry) =>
		IouEntries.TryGetValue(inventory, out entry!);

	public static void Postfix(Player player, MerchantInventory __result)
	{
		if (IouRelic.For(player) != null)
		{
			return;
		}

		var entry = new MerchantRelicEntry(ModelDb.Relic<IouRelic>().ToMutable(), player);
		entry.PurchaseCompleted += __result.UpdateEntries;
		IouEntries.Add(__result, entry);
	}
}

/// <summary>Includes the IOU entry wherever the game walks all entries (refreshing prices, purchase events).</summary>
[HarmonyPatch(typeof(MerchantInventory), nameof(MerchantInventory.AllEntries), MethodType.Getter)]
public static class AllEntriesPatch
{
	public static void Postfix(MerchantInventory __instance, ref IEnumerable<MerchantEntry> __result)
	{
		if (AddIouToMerchantPatch.TryGetIouEntry(__instance, out var entry))
		{
			__result = __result.Append(entry);
		}
	}
}

/// <summary>The IOU always costs <see cref="IouRelic.Price"/> before discounts, instead of a randomised price.</summary>
[HarmonyPatch(typeof(MerchantRelicEntry), nameof(MerchantRelicEntry.CalcCost))]
public static class IouPricePatch
{
	public static bool Prefix(MerchantRelicEntry __instance)
	{
		if (__instance.Model is not IouRelic)
		{
			return true;
		}
		__instance._cost = IouRelic.Price;
		return false;
	}
}

/// <summary>Relics that restock the shop must not refill the IOU slot with a random relic.</summary>
[HarmonyPatch(typeof(MerchantRelicEntry), "RestockAfterPurchase")]
public static class IouNoRestockPatch
{
	public static bool Prefix(MerchantRelicEntry __instance)
	{
		if (__instance.Model is not IouRelic)
		{
			return true;
		}
		__instance.Model = null;
		return false;
	}
}

/// <summary>With the IOU, shop items are affordable as long as the debt stays within the limit.</summary>
[HarmonyPatch(typeof(MerchantEntry), nameof(MerchantEntry.EnoughGold), MethodType.Getter)]
public static class CreditPatch
{
	public static void Postfix(MerchantEntry __instance, ref bool __result)
	{
		if (!__result && IouRelic.CanAffordOnCredit(__instance._player, __instance.Cost))
		{
			__result = true;
		}
	}
}

/// <summary>Shows the IOU entry in its own slot just above the card removal service.</summary>
[HarmonyPatch(typeof(NMerchantInventory), nameof(NMerchantInventory.Initialize))]
public static class IouSlotPatch
{
	private const float Gap = 16f;

	public static void Postfix(NMerchantInventory __instance, MerchantInventory inventory)
	{
		if (!AddIouToMerchantPatch.TryGetIouEntry(inventory, out var entry))
		{
			return;
		}

		var template = __instance._relicContainer?.GetChildOrNull<NMerchantRelic>(0);
		var removal = __instance._cardRemovalNode;
		if (template == null || removal == null)
		{
			MainFile.Logger.Warn("Shop layout not recognised; IOU slot not added");
			return;
		}

		var slot = string.IsNullOrEmpty(template.SceneFilePath)
			? (NMerchantRelic)template.Duplicate()
			: GD.Load<PackedScene>(template.SceneFilePath).Instantiate<NMerchantRelic>();
		slot.Name = "IouRelicSlot";
		removal.GetParent().AddChild(slot);
		slot.Position = removal.Position + new Vector2((removal.Size.X - slot.Size.X) / 2f, -slot.Size.Y - Gap);

		slot.Initialize(__instance);
		slot.FillSlot(entry);
	}
}
