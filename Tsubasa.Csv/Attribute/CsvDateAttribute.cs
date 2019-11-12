using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tsubasa.Csv {

	/// <summary>
	/// 日付変換
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class CsvDateAttribute : CsvItemAttribute {

		/// <summary>フォーマ固定、setできないように</summary>
		public override string Format { get; set; } = "yyyy/MM/dd";

		/// <summary>
		/// DateTimeを作成
		/// </summary>
		/// <param name="csvItem">CSV項目</param>
		/// <param name="targetType">対象の型</param>
		/// <returns>処理用オブジェクト</returns>
		public override object FromCsv(string csvItem, Type targetType)
		{
			// 自動解析を試す
			DateTime value;
			if (DateTime.TryParse(csvItem, out value)) {
				return value;
			}
			// null
			if (string.IsNullOrEmpty(csvItem)) {
				return targetType == typeof(DateTime) ? value : null as DateTime?;
			}
			// フォーマット指定
			value = DateTime.ParseExact(csvItem, this.Format, null);
			return value;
		}
	}
}
