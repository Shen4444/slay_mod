namespace IOUMod;

/// <summary>
/// The IOU's name, description and flavour text in every language the game offers.
/// {max} and {interest} are filled from the balance values in <see cref="IouRelic"/>;
/// {Debt} is left for the game to fill in when the tooltip is shown.
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
			"You may buy from the [gold]Merchant[/gold] on credit, borrowing up to [gold]{max}[/gold] Gold.\n" +
			"Borrowed Gold must be repaid with [gold]{interest}[/gold] extra.\n" +
			"Once your debt is paid off, the IOU is used up.\n" +
			"Owed: [gold]{Debt}[/gold]",
			"The merchant is always happy to lend... interest not included."),

		["zhs"] = new(
			"借据",
			"在[gold]商店[/gold]购物时可以赊账，最多借[gold]{max}[/gold]金币。\n" +
			"赊账借的金币需要额外偿还[gold]{interest}[/gold]。\n" +
			"还清欠款后，借据失效，不能再赊账。\n" +
			"当前欠款：[gold]{Debt}[/gold]",
			"商人总是很乐意借钱给你……利息嘛，另算。"),

		["zht"] = new(
			"借據",
			"在[gold]商店[/gold]購物時可以賒帳，最多借[gold]{max}[/gold]金幣。\n" +
			"賒帳借的金幣需要額外償還[gold]{interest}[/gold]。\n" +
			"還清欠款後，借據失效，不能再賒帳。\n" +
			"目前欠款：[gold]{Debt}[/gold]",
			"商人總是很樂意借錢給你……利息嘛，另算。"),

		["deu"] = new(
			"Schuldschein",
			"Du kannst beim [gold]Händler[/gold] auf Pump kaufen und dir bis zu [gold]{max}[/gold] Gold leihen.\n" +
			"Geliehenes Gold muss mit [gold]{interest}[/gold] Aufschlag zurückgezahlt werden.\n" +
			"Sobald deine Schulden beglichen sind, ist der Schuldschein verbraucht.\n" +
			"Schulden: [gold]{Debt}[/gold]",
			"Der Händler leiht dir immer gern Geld ... Zinsen nicht inbegriffen."),

		["fra"] = new(
			"Reconnaissance de dette",
			"Vous pouvez acheter à crédit chez le [gold]marchand[/gold], en empruntant jusqu'à [gold]{max}[/gold] d'or.\n" +
			"L'or emprunté doit être remboursé avec [gold]{interest}[/gold] de plus.\n" +
			"Une fois votre dette remboursée, la reconnaissance de dette est épuisée.\n" +
			"Dette : [gold]{Debt}[/gold]",
			"Le marchand est toujours ravi de vous prêter de l'argent... intérêts non compris."),

		["spa"] = new(
			"Pagaré",
			"Puedes comprarle a crédito al [gold]mercader[/gold], pidiendo prestado hasta [gold]{max}[/gold] de oro.\n" +
			"El oro prestado debe devolverse con un [gold]{interest}[/gold] extra.\n" +
			"Cuando saldes tu deuda, el pagaré se agota.\n" +
			"Deuda: [gold]{Debt}[/gold]",
			"Al mercader siempre le encanta prestarte dinero... intereses aparte."),

		["esp"] = new(
			"Pagaré",
			"Puedes comprarle a crédito al [gold]mercader[/gold], pidiendo prestado hasta [gold]{max}[/gold] de oro.\n" +
			"El oro prestado debe devolverse con un [gold]{interest}[/gold] extra.\n" +
			"Cuando pagues toda tu deuda, el pagaré se agota.\n" +
			"Deuda: [gold]{Debt}[/gold]",
			"Al mercader siempre le encanta prestarte dinero... los intereses van aparte."),

		["ita"] = new(
			"Pagherò",
			"Puoi comprare a credito dal [gold]mercante[/gold], prendendo in prestito fino a [gold]{max}[/gold] oro.\n" +
			"L'oro preso in prestito va restituito con il [gold]{interest}[/gold] in più.\n" +
			"Una volta saldato il debito, il pagherò si esaurisce.\n" +
			"Debito: [gold]{Debt}[/gold]",
			"Il mercante è sempre felice di prestarti denaro... interessi esclusi."),

		["ptb"] = new(
			"Nota Promissória",
			"Você pode comprar fiado do [gold]Mercador[/gold], pegando emprestado até [gold]{max}[/gold] de ouro.\n" +
			"O ouro emprestado deve ser devolvido com [gold]{interest}[/gold] a mais.\n" +
			"Quando a dívida for quitada, a nota promissória se esgota.\n" +
			"Dívida: [gold]{Debt}[/gold]",
			"O mercador sempre fica feliz em te emprestar dinheiro... juros à parte."),

		["pol"] = new(
			"Weksel",
			"Możesz kupować u [gold]Kupca[/gold] na kredyt, pożyczając do [gold]{max}[/gold] złota.\n" +
			"Pożyczone złoto trzeba oddać z dodatkowymi [gold]{interest}[/gold].\n" +
			"Po spłaceniu długu weksel zostaje zużyty.\n" +
			"Dług: [gold]{Debt}[/gold]",
			"Kupiec zawsze chętnie pożyczy ci pieniądze... odsetki nie są wliczone."),

		["rus"] = new(
			"Долговая расписка",
			"Можно покупать у [gold]торговца[/gold] в долг, заняв до [gold]{max}[/gold] золота.\n" +
			"Занятое золото нужно вернуть с наценкой [gold]{interest}[/gold].\n" +
			"Когда долг погашен, расписка теряет силу.\n" +
			"Долг: [gold]{Debt}[/gold]",
			"Торговец всегда рад одолжить вам денег... проценты не включены."),

		["tur"] = new(
			"Senet",
			"[gold]Tüccar'dan[/gold] veresiye alışveriş yapabilirsin; en fazla [gold]{max}[/gold] altın borç alabilirsin.\n" +
			"Borç alınan altın, [gold]{interest}[/gold] fazlasıyla geri ödenmelidir.\n" +
			"Borcunu tamamen ödediğinde senet tükenir.\n" +
			"Borç: [gold]{Debt}[/gold]",
			"Tüccar sana borç vermekten her zaman mutluluk duyar... faiz hariç."),

		["ind"] = new(
			"Surat Utang",
			"Kamu bisa berbelanja dari [gold]Pedagang[/gold] secara kredit, dengan meminjam hingga [gold]{max}[/gold] Emas.\n" +
			"Emas yang dipinjam harus dikembalikan dengan tambahan [gold]{interest}[/gold].\n" +
			"Setelah utangmu lunas, Surat Utang habis terpakai.\n" +
			"Utang: [gold]{Debt}[/gold]",
			"Pedagang selalu senang meminjamkan uang... bunga belum termasuk."),

		["jpn"] = new(
			"借用書",
			"[gold]商人[/gold]から掛けで買い物ができる。借りられるのは最大[gold]{max}[/gold]ゴールドまで。\n" +
			"借りたゴールドは[gold]{interest}[/gold]上乗せして返済する必要がある。\n" +
			"借金を完済すると、借用書は使用済みになる。\n" +
			"現在の借金：[gold]{Debt}[/gold]",
			"商人はいつでも喜んでお金を貸してくれる……利息は別だが。"),

		["kor"] = new(
			"차용증",
			"[gold]상인[/gold]에게서 외상으로 구매할 수 있습니다. 최대 [gold]{max}[/gold] 골드까지 빌릴 수 있습니다.\n" +
			"빌린 골드는 [gold]{interest}[/gold]를 더해서 갚아야 합니다.\n" +
			"빚을 모두 갚으면 차용증은 소진됩니다.\n" +
			"현재 빚: [gold]{Debt}[/gold]",
			"상인은 언제나 기꺼이 돈을 빌려줍니다... 이자는 별도입니다."),

		["tha"] = new(
			"สัญญาเงินกู้",
			"ซื้อของจาก[gold]พ่อค้า[/gold]แบบติดหนี้ได้ ยืมได้สูงสุด [gold]{max}[/gold] ทอง\n" +
			"ทองที่ยืมต้องชำระคืนพร้อมดอกเบี้ยเพิ่ม [gold]{interest}[/gold]\n" +
			"เมื่อชำระหนี้หมดแล้ว สัญญาเงินกู้จะหมดผล\n" +
			"หนี้ปัจจุบัน: [gold]{Debt}[/gold]",
			"พ่อค้ายินดีให้คุณยืมเงินเสมอ... ไม่รวมดอกเบี้ยนะ"),
	};
}
