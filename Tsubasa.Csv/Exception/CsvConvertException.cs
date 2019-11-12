using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsubasa.Csv {

    /// <summary>
    /// 変換エラー
    /// </summary>
    public class CsvConvertException : Exception {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public CsvConvertException(string message) 
            : base(message) { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CsvConvertException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
