using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace IOUMod;

/// <summary>
/// Lets the owner buy from the merchant on credit. Debt is simply negative gold;
/// while in debt, gold from combat rewards is reduced.
/// </summary>
// The event pool is never used for random drops, so the relic only comes from our shop slot.
[Pool(typeof(EventRelicPool))]
public sealed class IouRelic : CustomRelicModel
{
	public const int Price = 20;
	public const int MaxDebt = 100;
	public const int HighDebtThreshold = 70;
	public const decimal LowDebtPenalty = 0.10m;
	public const decimal HighDebtPenalty = 0.20m;

	public override RelicRarity Rarity => RelicRarity.Event;

	public override bool IsAllowedInShops => false;

	public override string PackedIconPath => $"{ImageDir}/iou_relic.png";
	protected override string PackedIconOutlinePath => $"{ImageDir}/iou_relic_outline.png";
	protected override string BigIconPath => $"{ImageDir}/big/iou_relic.png";

	private const string ImageDir = $"{MainFile.ResPath}/images/relics";

	protected override IEnumerable<DynamicVar> CanonicalVars =>
	[
		new DisplayVar<IouRelic>("Debt", r => r.Debt.ToString()),
		new DisplayVar<IouRelic>("Penalty", r => $"{(int)(r.Penalty * 100)}%"),
	];

	public override List<(string, string)> Localization => LocManager.Instance.Language switch
	{
		"zhs" => new RelicLoc(
			"借据",
			"在[gold]商店[/gold]购物时可以赊账，最多欠款[gold]100[/gold]金币。\n" +
			"负债期间，战斗获得的金币减少[gold]10%[/gold]；欠款超过[gold]70[/gold]时，改为减少[gold]20%[/gold]。\n" +
			"还清欠款后，惩罚消失。\n" +
			"当前欠款：[gold]{Debt}[/gold]（金币减少[gold]{Penalty}[/gold]）",
			"商人总是很乐意借钱给你……利息嘛，另算。"),
		_ => new RelicLoc(
			"IOU",
			"You may buy from the [gold]Merchant[/gold] on credit, owing up to [gold]100[/gold] Gold.\n" +
			"While in debt, Gold from combat is reduced by [gold]10%[/gold], or [gold]20%[/gold] if you owe more than [gold]70[/gold].\n" +
			"Paying off your debt removes the penalty.\n" +
			"Owed: [gold]{Debt}[/gold] (Gold reduced by [gold]{Penalty}[/gold])",
			"The merchant is always happy to lend... interest not included."),
	};

	public override bool IsAllowed(IRunState runState) => false;

	public override bool ShowCounter => Debt > 0;

	public override int DisplayAmount => Debt;

	/// <summary>Null for the canonical (compendium) copy, which has no owner.</summary>
	private Player? OwnerOrNull => IsMutable ? _owner : null;

	public int Debt => OwnerOrNull is { Gold: < 0 } p ? -p.Gold : 0;

	public decimal Penalty => Debt switch
	{
		0 => 0m,
		> HighDebtThreshold => HighDebtPenalty,
		_ => LowDebtPenalty,
	};

	public static IouRelic? For(Player? player) => player?.GetRelic<IouRelic>();

	/// <summary>Whether the player can buy something costing <paramref name="cost"/> in the current shop without exceeding the debt limit.</summary>
	public static bool CanAffordOnCredit(Player player, int cost) =>
		For(player) != null && player.RunState.CurrentRoom is MerchantRoom && player.Gold - cost >= -MaxDebt;

	public override decimal ModifyGoldGained(Player player, decimal amount)
	{
		if (player != OwnerOrNull || !Patches.CombatGoldRewardPatch.IsClaiming || Penalty == 0m)
		{
			return amount;
		}
		// The game truncates to int when adding gold, which rounds the result down.
		return amount * (1m - Penalty);
	}

	public override Task AfterModifyingGoldGained(Player player, decimal amount)
	{
		Flash();
		return Task.CompletedTask;
	}

	public void RefreshCounter() => InvokeDisplayAmountChanged();
}
