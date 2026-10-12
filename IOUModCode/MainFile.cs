using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

namespace IOUMod;

[ModInitializer(nameof(Initialize))]
public static class MainFile
{
	public const string ModId = "IOUMod";
	public const string ResPath = $"res://{ModId}";

	public static Logger Logger { get; } = new(ModId, LogType.Generic);

	public static void Initialize()
	{
		new Harmony(ModId).PatchAll(Assembly.GetExecutingAssembly());
		Logger.Info("Loaded");
	}
}
