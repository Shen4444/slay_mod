using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

namespace IOUMod;

[ModInitializer("ModLoaded")]
public static class ModEntry
{
	public static void ModLoaded()
	{
		Log.Warn("[IOUMod] Loaded successfully");
	}
}
