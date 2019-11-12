using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsubasa.Csv {

    /// <summary>
    /// 定義エラー
    /// </summary>
    public class CsvFormatException : Exception {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public CsvFormatException(string message) 
            : base(message) { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CsvFormatException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
