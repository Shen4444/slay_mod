using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;

namespace IOUMod.Patches;

/// <summary>
/// Re-adds the IOU's text after the game loads the tables for a new language,
/// just before it tells the UI to refresh.
/// </summary>
[HarmonyPatch(typeof(LocManager), "TriggerLocaleChange")]
public static class LanguageChangePatch
{
	public static void Prefix()
	{
		IouRelic.ApplyLocalization();
	}
}
