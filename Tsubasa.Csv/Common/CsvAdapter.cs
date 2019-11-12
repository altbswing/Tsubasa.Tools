using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Tsubasa.Csv {

    /// <summary>
    /// CSV処理
    /// </summary>
    public partial class CsvAdapter<TEntity> where TEntity : class, new() {

        /// <summary>
        /// ヘッダー出力モード
        /// </summary>
        public HeaderMode HeaderMode { get; set; } = HeaderMode.None;

        /// <summary>
        /// 文字コード
        /// </summary>
        public Encoding Charset { get; set; } = Encoding.UTF8;

        /// <summary>処理対象のプロパティ一覧</summary>
        private readonly static List<PropertyInfo> _PropertyList = _CsvProperties();

        /// <summary>
        /// 処理対象のプロパティ抽出
        /// </summary>
        /// <returns></returns>
        private static List<PropertyInfo> _CsvProperties()
        {
            // 対象プロパティを特定
            var query = from p in typeof(TEntity).GetProperties()
                        let attr = p.GetCustomAttribute<CsvItemAttribute>()
                        where attr != null
                        orderby attr.Seq
                        select p;
            return query.ToList();
        }
    }
}
