using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsubasa.Csv {

    /// <summary>
    /// 定義エラー
    /// </summary>
    public class CsvDefineException : Exception {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public CsvDefineException(string message) : base(message) { }
    }
}
