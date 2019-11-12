using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tsubasa.Csv {

	/// <summary>
	/// Csv出力項目を宣言する
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class CsvItemAttribute : Attribute {

		/// <summary>出力順</summary>
		public int Seq { get; set; }

		/// <summary>CSV生成時、エスケープするか</summary>
		public CsvEscape Escape { get; set; } = CsvEscape.Always;

		/// <summary>C# ルールの書式</summary>
		public virtual string Format { get; set; }

		/// <summary>
		/// 項目をcsv文字列に変換する
		/// フォーマが設定されている場合、フォーマ設定を利用してcsv項目名出力
		/// </summary>
		/// <param name="value">.Net項目</param>
		/// <returns>出力用csv文字列</returns>
		public virtual string ToCsv(object value)
		{
			// フォーマットみ設定の場合、文字列形式そのまま出力
			if (Format == null) {
				return (value ?? "").ToString();
			}
			// string.Formatを呼び出すか
			var callStringFormat = Regex.IsMatch(Format, @"[{]0([:].*)?([,][0-9+-]+)?[}]");
			// string.Format標準じゃない場合
			var formattable = value as IFormattable;
			if (!callStringFormat && formattable != null) {
				return formattable.ToString(Format, null);
			}
			// その他の場合string.Formatを呼び出す
			return string.Format(Format, value);
		}

		/// <summary>
		/// csv出力項目にする
		/// 「"」と「,」のエスケープ
		/// 両端に「"」を設定
		/// </summary>
		/// <param name="item">文字列に変換した項目</param>
		/// <returns></returns>
		public virtual string EscapeCsv(string item)
		{
			if (item == null) {
				return null;
			}
			// エスケープ文字(, ", tab, 改行);
			var regex = new Regex("[,\"\t\r\n]", RegexOptions.Multiline);
			// エスケープ処理
			var escape = this.Escape == CsvEscape.Always
					  || (this.Escape == CsvEscape.Auto && regex.IsMatch(item));
			// エスケープしない
			if (!escape) {
				return item;
			}
			// エスケープ 「"」 => 「""」
			item = (item ?? "").Replace("\"", "\"\"");
			// 両端に「"」
			item = $"\"{item}\"";
			return item;
		}

		/// <summary>
		/// CSV項目を処理用オブジェクトに変換。
		/// ※ 汎用処理
		/// </summary>
		/// <param name="csvItem">CSV項目</param>
		/// <param name="targetType">対象の型</param>
		/// <returns>処理用オブジェクト</returns>
		public virtual object FromCsv(string csvItem, Type targetType)
		{
			// 文字列の場合
			if (targetType == typeof(string)) {
				return csvItem;
			}
			// int?とかのnull可対象か
			var isNullable = targetType.IsGenericType
				&& targetType.GetGenericTypeDefinition() == typeof(Nullable<>);
			// nullの場合
			if (string.IsNullOrEmpty(csvItem) && (targetType.IsClass || isNullable)) {
				return null;
			}
			try {
				// 自動型変換を試行する
				var item = Convert.ChangeType(csvItem, targetType);
				return item;
			} catch (Exception inner) {
				// 変換失敗時のエラーメッセージ
				var message = $"\"{csvItem}\"では「{targetType.Name}」に自動変換できません。" +
					$"別の「{nameof(CsvItemAttribute)}」派生属性を使用してください。";
				throw new CsvFormatException(message, inner);
			}
		}

	}
}
