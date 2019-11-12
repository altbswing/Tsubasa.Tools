using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tsubasa.Csv {

    /// <summary>
    /// 列挙の項目値（整数）に変換する
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CsvEnumValueAttribute : CsvItemAttribute {

        /// <summary>
        /// 列挙の項目値（整数）に変換する
        /// </summary>
        /// <param name="item">列挙</param>
        /// <returns>列挙の項目値文字列</returns>
        public override string ToCsv(object item)
        {
            if (item == null) {
                return "";
            }
            var value = base.ToCsv((int)item);
            return value;
        }
    }
}
