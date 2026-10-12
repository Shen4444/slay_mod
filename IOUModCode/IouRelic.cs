using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace IOUMod;

/// <summary>
/// Lets the owner buy from the merchant on credit. Debt is simply negative gold;
/// while in debt, gold from combat rewards is reduced. Once a debt is fully repaid the relic is used up.
/// </summary>
// The event pool is never used for random drops, so the relic only comes from our shop slot.
[Pool(typeof(EventRelicPool))]
public sealed class IouRelic : CustomRelicModel
{
	// Balance values: change these to rebalance the relic. The description updates automatically.
	public const int Price = 33;
	public const int MaxDebt = 100;
	public const int HighDebtThreshold = 50;
	public const decimal LowDebtPenalty = 0.30m;
	public const decimal HighDebtPenalty = 0.50m;

	public override RelicRarity Rarity => RelicRarity.Event;

	public override bool IsAllowedInShops => false;

	public override string PackedIconPath => $"{ImageDir}/iou_relic.png";
	protected override string PackedIconOutlinePath => $"{ImageDir}/iou_relic_outline.png";
	protected override string BigIconPath => $"{ImageDir}/big/iou_relic.png";

	private const string ImageDir = $"{MainFile.ResPath}/images/relics";

	protected override IEnumerable<DynamicVar> CanonicalVars =>
	[
		new DisplayVar<IouRelic>("Debt", r => r.Debt.ToString()),
		new DisplayVar<IouRelic>("Penalty", r => Percent(r.Penalty)),
	];

	private static string Percent(decimal fraction) => $"{(int)(fraction * 100)}%";

	// The text for each language is in IouRelicText; numbers in it come from the constants above,
	// so balance changes only need editing there.
	public override List<(string, string)> Localization
	{
		get
		{
			var text = IouRelicText.For(LocManager.Instance.Language);
			return new RelicLoc(text.Title, FillNumbers(text.Description), text.Flavor);
		}
	}

	private static string FillNumbers(string description) => description
		.Replace("{max}", MaxDebt.ToString())
		.Replace("{threshold}", HighDebtThreshold.ToString())
		.Replace("{low}", Percent(LowDebtPenalty))
		.Replace("{high}", Percent(HighDebtPenalty));

	/// <summary>
	/// BaseLib adds <see cref="Localization"/> only once at startup, and the game rebuilds its text
	/// tables whenever the language changes, so the text is written again for the new language.
	/// </summary>
	public static void ApplyLocalization()
	{
		IouRelic relic;
		try
		{
			relic = ModelDb.Relic<IouRelic>();
		}
		catch (Exception)
		{
			// The model database is not ready yet; BaseLib adds the text when it is.
			return;
		}
		if (relic == null)
		{
			return;
		}

		var translations = LocManager.Instance.GetTable("relics")._translations;
		foreach (var (key, text) in relic.Localization)
		{
			translations[$"{relic.Id.Entry}.{key}"] = text;
		}
	}

	public override bool IsAllowed(IRunState runState) => false;

	private bool _isRepaid;

	/// <summary>Set once a debt has been fully repaid; the IOU then stops working for the rest of the run.</summary>
	[SavedProperty]
	public bool IsRepaid
	{
		get => _isRepaid;
		set
		{
			AssertMutable();
			_isRepaid = value;
			if (value)
			{
				Status = RelicStatus.Disabled;
			}
		}
	}

	public override bool IsUsedUp => IsRepaid;

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

	/// <summary>The player's IOU if it still works, i.e. has not been used up by repaying a debt.</summary>
	public static IouRelic? ActiveFor(Player? player) => For(player) is { IsRepaid: false } relic ? relic : null;

	/// <summary>Whether the player can buy something costing <paramref name="cost"/> in the current shop without exceeding the debt limit.</summary>
	public static bool CanAffordOnCredit(Player player, int cost) =>
		ActiveFor(player) != null && player.RunState.CurrentRoom is MerchantRoom && player.Gold - cost >= -MaxDebt;

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

	public void OnGoldChanged(int oldGold, int newGold)
	{
		if (oldGold < 0 && newGold >= 0 && !IsRepaid)
		{
			IsRepaid = true;
			Flash();
		}
		InvokeDisplayAmountChanged();
	}
}
