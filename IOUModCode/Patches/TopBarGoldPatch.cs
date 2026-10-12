using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.sts2.Core.Nodes.TopBar;

namespace IOUMod.Patches;

/// <summary>Shows the gold counter in red while it is negative.</summary>
[HarmonyPatch(typeof(NTopBarGold))]
public static class TopBarGoldPatch
{
	private const string BaseColorMeta = "iou_base_color";

	[HarmonyPostfix]
	[HarmonyPatch(nameof(NTopBarGold.Initialize))]
	public static void AfterInitialize(NTopBarGold __instance) => UpdateColor(__instance);

	[HarmonyPostfix]
	[HarmonyPatch(nameof(NTopBarGold.UpdateGold))]
	public static void AfterUpdateGold(NTopBarGold __instance) => UpdateColor(__instance);

	private static void UpdateColor(NTopBarGold gold)
	{
		if (gold._player == null)
		{
			return;
		}
		CanvasItem label = gold._goldLabel;
		if (!label.HasMeta(BaseColorMeta))
		{
			label.SetMeta(BaseColorMeta, label.Modulate);
		}
		label.Modulate = gold._player.Gold < 0 ? StsColors.red : label.GetMeta(BaseColorMeta).AsColor();
	}
}
