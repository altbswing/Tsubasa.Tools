using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tsubasa.Csv {

    /// <summary>
    /// 数字変換
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CsvHexAttribute : CsvItemAttribute {

        ///// <summary>
        ///// 値
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public override string ToCsv(object value)
        //{
        //    // nullの場合（int?とか）
        //    if (value == null) {
        //        return base.ToCsv(value);
        //    }
        //    // 表示桁数自動判断
        //    var digit = value is sbyte || value is byte ? 2
        //              : value is short || value is ushort ? 4
        //              : value is int || value is uint ? 8
        //              : value is long || value is ulong ? 16
        //              : throw new CsvConvertException("HEX表示には整数型が必要です。");
        //    // 0xFF
        //    var item = ((IFormattable)value).ToString($"X{digit}", null);
        //    return base.ToCsv(item);
        //}

        /// <summary>
        /// 数字を作成
        /// </summary>
        /// <param name="csvItem">CSV項目</param>
        /// <param name="targetType">対象の型</param>
        /// <returns>処理用オブジェクト</returns>
        public override object FromCsv(string csvItem, Type targetType)
        {
            // なし
            if (string.IsNullOrEmpty(csvItem)) {
                // classの場合null、structの場合デフォルト値
                return targetType.IsClass ? null : Activator.CreateInstance(targetType);
            }
            // 先頭に0xあれば外す
            csvItem = Regex.Replace(csvItem, "^0x", "");
            // byte,short,int,longなどなどの数字型から「Parse(string, NumberStyles)」メッソドを特定
            var method = targetType.GetMethod(
                nameof(int.Parse), new Type[] { typeof(string), typeof(NumberStyles) });
            // 引数
            var args = new object[] { csvItem, NumberStyles.HexNumber };
            var number = method.Invoke(targetType, args);
            return number;
        }
    }
}
