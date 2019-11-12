using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tsubasa.Csv {

    /// <summary>
    /// 列挙の表示名に変換する
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CsvEnumDisplayAttribute : CsvItemAttribute {

        /// <summary>
        /// 列挙の表示名に変換する
        /// </summary>
        /// <param name="item">列挙</param>
        /// <returns>列挙の項目値文字列</returns>
        public override string ToCsv(object item)
        {
            if (item == null) {
                return "";
            }
            // 型
            var type = Nullable.GetUnderlyingType(item.GetType()) ?? item.GetType();
            // フィールド
            var field = type.GetFields().First(f => f.Name == item.ToString());
            // 表示名定義
            var display = field.GetCustomAttribute<DisplayAttribute>();
            // 取得失敗
            if (display == null) {
                throw new CsvDefineException($"{type.Name}.{field.Name}に{nameof(DisplayAttribute)}の定義はありません。");
            }
            // 表示名
            var name = display.GetName();
            return base.ToCsv(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="csvItem"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public override object FromCsv(string csvItem, Type targetType)
        {
            throw new CsvFormatException("列挙項目表示名で出力した項目は列挙値に逆戻りできません。");
        }
    }
}
