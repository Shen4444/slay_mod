namespace IOUMod;

/// <summary>
/// The IOU's name, description and flavour text in every language the game offers.
/// {max}, {threshold}, {low} and {high} are filled from the balance values in <see cref="IouRelic"/>;
/// {Debt} and {Penalty} are left for the game to fill in when the tooltip is shown.
/// </summary>
public static class IouRelicText
{
	public record Text(string Title, string Description, string Flavor);

	public static Text For(string language) =>
		Texts.TryGetValue(language, out var text) ? text : Texts["eng"];

	private static readonly Dictionary<string, Text> Texts = new()
	{
		["eng"] = new(
			"IOU",
			"You may buy from the [gold]Merchant[/gold] on credit, owing up to [gold]{max}[/gold] Gold.\n" +
			"While in debt, Gold from combat is reduced by [gold]{low}[/gold], or [gold]{high}[/gold] if you owe more than [gold]{threshold}[/gold].\n" +
			"Once your debt is paid off, the IOU is used up.\n" +
			"Owed: [gold]{Debt}[/gold] (Gold reduced by [gold]{Penalty}[/gold])",
			"The merchant is always happy to lend... interest not included."),

		["zhs"] = new(
			"借据",
			"在[gold]商店[/gold]购物时可以赊账，最多欠款[gold]{max}[/gold]金币。\n" +
			"负债期间，战斗获得的金币减少[gold]{low}[/gold]；欠款超过[gold]{threshold}[/gold]时，改为减少[gold]{high}[/gold]。\n" +
			"还清欠款后，借据失效，不能再赊账。\n" +
			"当前欠款：[gold]{Debt}[/gold]（金币减少[gold]{Penalty}[/gold]）",
			"商人总是很乐意借钱给你……利息嘛，另算。"),

		["zht"] = new(
			"借據",
			"在[gold]商店[/gold]購物時可以賒帳，最多欠款[gold]{max}[/gold]金幣。\n" +
			"負債期間，戰鬥獲得的金幣減少[gold]{low}[/gold]；欠款超過[gold]{threshold}[/gold]時，改為減少[gold]{high}[/gold]。\n" +
			"還清欠款後，借據失效，不能再賒帳。\n" +
			"目前欠款：[gold]{Debt}[/gold]（金幣減少[gold]{Penalty}[/gold]）",
			"商人總是很樂意借錢給你……利息嘛，另算。"),

		["deu"] = new(
			"Schuldschein",
			"Du kannst beim [gold]Händler[/gold] auf Pump kaufen und bis zu [gold]{max}[/gold] Gold Schulden machen.\n" +
			"Solange du Schulden hast, erhältst du aus Kämpfen [gold]{low}[/gold] weniger Gold, oder [gold]{high}[/gold] weniger, wenn du mehr als [gold]{threshold}[/gold] schuldest.\n" +
			"Sobald deine Schulden beglichen sind, ist der Schuldschein verbraucht.\n" +
			"Schulden: [gold]{Debt}[/gold] (Gold um [gold]{Penalty}[/gold] verringert)",
			"Der Händler leiht dir immer gern Geld ... Zinsen nicht inbegriffen."),

		["fra"] = new(
			"Reconnaissance de dette",
			"Vous pouvez acheter à crédit chez le [gold]marchand[/gold], jusqu'à [gold]{max}[/gold] d'or de dette.\n" +
			"Tant que vous êtes endetté, l'or gagné en combat est réduit de [gold]{low}[/gold], ou de [gold]{high}[/gold] si vous devez plus de [gold]{threshold}[/gold].\n" +
			"Une fois votre dette remboursée, la reconnaissance de dette est épuisée.\n" +
			"Dette : [gold]{Debt}[/gold] (or réduit de [gold]{Penalty}[/gold])",
			"Le marchand est toujours ravi de vous prêter de l'argent... intérêts non compris."),

		["spa"] = new(
			"Pagaré",
			"Puedes comprarle a crédito al [gold]mercader[/gold], con una deuda de hasta [gold]{max}[/gold] de oro.\n" +
			"Mientras tengas deudas, el oro de los combates se reduce un [gold]{low}[/gold], o un [gold]{high}[/gold] si debes más de [gold]{threshold}[/gold].\n" +
			"Cuando saldes tu deuda, el pagaré se agota.\n" +
			"Deuda: [gold]{Debt}[/gold] (oro reducido un [gold]{Penalty}[/gold])",
			"Al mercader siempre le encanta prestarte dinero... intereses aparte."),

		["esp"] = new(
			"Pagaré",
			"Puedes comprarle a crédito al [gold]mercader[/gold], con una deuda de hasta [gold]{max}[/gold] de oro.\n" +
			"Mientras tengas deudas, el oro de los combates se reduce un [gold]{low}[/gold], o un [gold]{high}[/gold] si debes más de [gold]{threshold}[/gold].\n" +
			"Cuando pagues toda tu deuda, el pagaré se agota.\n" +
			"Deuda: [gold]{Debt}[/gold] (oro reducido un [gold]{Penalty}[/gold])",
			"Al mercader siempre le encanta prestarte dinero... los intereses van aparte."),

		["ita"] = new(
			"Pagherò",
			"Puoi comprare a credito dal [gold]mercante[/gold], con un debito massimo di [gold]{max}[/gold] oro.\n" +
			"Finché sei in debito, l'oro ottenuto in combattimento è ridotto del [gold]{low}[/gold], o del [gold]{high}[/gold] se devi più di [gold]{threshold}[/gold].\n" +
			"Una volta saldato il debito, il pagherò si esaurisce.\n" +
			"Debito: [gold]{Debt}[/gold] (oro ridotto del [gold]{Penalty}[/gold])",
			"Il mercante è sempre felice di prestarti denaro... interessi esclusi."),

		["ptb"] = new(
			"Nota Promissória",
			"Você pode comprar fiado do [gold]Mercador[/gold], devendo até [gold]{max}[/gold] de ouro.\n" +
			"Enquanto estiver endividado, o ouro de combates é reduzido em [gold]{low}[/gold], ou em [gold]{high}[/gold] se você dever mais de [gold]{threshold}[/gold].\n" +
			"Quando a dívida for quitada, a nota promissória se esgota.\n" +
			"Dívida: [gold]{Debt}[/gold] (ouro reduzido em [gold]{Penalty}[/gold])",
			"O mercador sempre fica feliz em te emprestar dinheiro... juros à parte."),

		["pol"] = new(
			"Weksel",
			"Możesz kupować u [gold]Kupca[/gold] na kredyt, zadłużając się do [gold]{max}[/gold] złota.\n" +
			"Dopóki masz dług, złoto z walk jest zmniejszone o [gold]{low}[/gold], lub o [gold]{high}[/gold], jeśli jesteś winien więcej niż [gold]{threshold}[/gold].\n" +
			"Po spłaceniu długu weksel zostaje zużyty.\n" +
			"Dług: [gold]{Debt}[/gold] (złoto zmniejszone o [gold]{Penalty}[/gold])",
			"Kupiec zawsze chętnie pożyczy ci pieniądze... odsetki nie są wliczone."),

		["rus"] = new(
			"Долговая расписка",
			"Можно покупать у [gold]торговца[/gold] в долг, задолжав до [gold]{max}[/gold] золота.\n" +
			"Пока у вас есть долг, золото за бои уменьшается на [gold]{low}[/gold], или на [gold]{high}[/gold], если долг больше [gold]{threshold}[/gold].\n" +
			"Когда долг погашен, расписка теряет силу.\n" +
			"Долг: [gold]{Debt}[/gold] (золото уменьшено на [gold]{Penalty}[/gold])",
			"Торговец всегда рад одолжить вам денег... проценты не включены."),

		["tur"] = new(
			"Senet",
			"[gold]Tüccar'dan[/gold] veresiye alışveriş yapabilirsin; en fazla [gold]{max}[/gold] altın borçlanabilirsin.\n" +
			"Borçlu olduğun sürece savaşlardan kazandığın altın [gold]{low}[/gold] azalır; borcun [gold]{threshold}[/gold] altını aşarsa [gold]{high}[/gold] azalır.\n" +
			"Borcunu tamamen ödediğinde senet tükenir.\n" +
			"Borç: [gold]{Debt}[/gold] (altın [gold]{Penalty}[/gold] azaldı)",
			"Tüccar sana borç vermekten her zaman mutluluk duyar... faiz hariç."),

		["ind"] = new(
			"Surat Utang",
			"Kamu bisa berbelanja dari [gold]Pedagang[/gold] secara kredit, dengan utang hingga [gold]{max}[/gold] Emas.\n" +
			"Selama berutang, Emas dari pertarungan berkurang [gold]{low}[/gold], atau [gold]{high}[/gold] jika utangmu lebih dari [gold]{threshold}[/gold].\n" +
			"Setelah utangmu lunas, Surat Utang habis terpakai.\n" +
			"Utang: [gold]{Debt}[/gold] (Emas berkurang [gold]{Penalty}[/gold])",
			"Pedagang selalu senang meminjamkan uang... bunga belum termasuk."),

		["jpn"] = new(
			"借用書",
			"[gold]商人[/gold]から掛けで買い物ができる。借金は最大[gold]{max}[/gold]ゴールドまで。\n" +
			"借金がある間、戦闘で得るゴールドが[gold]{low}[/gold]減少する。借金が[gold]{threshold}[/gold]を超えると[gold]{high}[/gold]減少する。\n" +
			"借金を完済すると、借用書は使用済みになる。\n" +
			"現在の借金：[gold]{Debt}[/gold]（ゴールド[gold]{Penalty}[/gold]減少）",
			"商人はいつでも喜んでお金を貸してくれる……利息は別だが。"),

		["kor"] = new(
			"차용증",
			"[gold]상인[/gold]에게서 외상으로 구매할 수 있습니다. 최대 [gold]{max}[/gold] 골드까지 빚질 수 있습니다.\n" +
			"빚이 있는 동안 전투에서 얻는 골드가 [gold]{low}[/gold] 감소합니다. 빚이 [gold]{threshold}[/gold]을 넘으면 [gold]{high}[/gold] 감소합니다.\n" +
			"빚을 모두 갚으면 차용증은 소진됩니다.\n" +
			"현재 빚: [gold]{Debt}[/gold] (골드 [gold]{Penalty}[/gold] 감소)",
			"상인은 언제나 기꺼이 돈을 빌려줍니다... 이자는 별도입니다."),

		["tha"] = new(
			"สัญญาเงินกู้",
			"ซื้อของจาก[gold]พ่อค้า[/gold]แบบติดหนี้ได้ สูงสุด [gold]{max}[/gold] ทอง\n" +
			"ระหว่างที่ติดหนี้ ทองจากการต่อสู้จะลดลง [gold]{low}[/gold] หรือลดลง [gold]{high}[/gold] หากติดหนี้เกิน [gold]{threshold}[/gold]\n" +
			"เมื่อชำระหนี้หมดแล้ว สัญญาเงินกู้จะหมดผล\n" +
			"หนี้ปัจจุบัน: [gold]{Debt}[/gold] (ทองลดลง [gold]{Penalty}[/gold])",
			"พ่อค้ายินดีให้คุณยืมเงินเสมอ... ไม่รวมดอกเบี้ยนะ"),
	};
}
