using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsubasa.Csv {

    /// <summary>
    /// CSV出力、ヘッダーの方式
    /// </summary>
    public enum HeaderMode {

        /// <summary>
        /// ヘッダーなしで出力する
        /// </summary>
        None,

        /// <summary>
        /// Entityのプロパティ名を出力する
        /// </summary>
        Property,

        /// <summary>
        /// System.ComponentModel.DataAnnotations.Display
        /// の設定で出力する
        /// </summary>
        Display,
    }

    /// <summary>
    /// エスケープするか
    /// </summary>
    public enum CsvEscape {

        /// <summary>
        /// 「,」と「"」含まれる項目をエスケープ
        /// </summary>
        Auto,

        /// <summary>
        /// エスケープしない
        /// </summary>
        Never,

        /// <summary>
        /// 常にエスケープ
        /// </summary>
        Always,
    }
}
