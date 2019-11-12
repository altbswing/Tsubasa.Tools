using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Data.OleDb;

namespace Tsubasa.Csv {

    /// <summary>
    /// CSV入力処理
    /// </summary>
    public partial class CsvAdapter<TEntity> {

        /// <summary>
        /// Csv解析
        /// </summary>
        /// <returns></returns>
        public async Task<List<TEntity>> ReadCsvListAsync(FileInfo file) {
            // テーブルを読み込む
            var table = await ReadCsvTableAsync(file);
            // リスト
            var list = new List<TEntity>(table.Rows.Count);
            // 行毎に
            foreach (var row in table.Rows.Cast<DataRow>()) {
                var entity = new TEntity();
                var index = 0;
                // 確認
                if (_PropertyList.Count != row.ItemArray.Length) {
                    var message = $"{index + 1}行目データの構造はEntity定義と一致しません。";
                    throw new CsvFormatException(message);
                }
                // プロパティ毎に
                foreach (var p in _PropertyList) {
                    // 項目取得
                    var cellItem = row.ItemArray[index++];
                    // 文字列
                    var csvItem = (cellItem == DBNull.Value ? null : cellItem)?.ToString();
                    // csv設定
                    var attr = p.GetCustomAttribute<CsvItemAttribute>();
                    // 項目
                    var item = attr.FromCsv(csvItem, p.PropertyType);
                    // 項目設定
                    p.SetValue(entity, item);
                }
                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        /// 解析処理
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<DataTable> ReadCsvTableAsync(FileInfo file)
        {
            // 接続文字列
            var build = new OleDbConnectionStringBuilder();
            // バージョン
            build.Provider = "Microsoft.Jet.OLEDB.4.0";
            //build.Provider = "Microsoft.ACE.OLEDB.12";
            // フォルダー
            build.DataSource = file.DirectoryName;
            // プロパティ
            build["Extended Properties"] = ""
                + $"Text;"
                + $"FMT=Delimited(,);"
                + $"CharacterSet={this.Charset.CodePage};"
                //+ $"IMEX=1;"
                + $"HDR={(HeaderMode == HeaderMode.None ? "No" : "Yes")};"
            ;
            // 接続
            using (var conn = new OleDbConnection(build.ToString())) {
                conn.Open();
                var commond = conn.CreateCommand();
                commond.CommandText = $"SELECT * FROM {file.Name}";
                var reader = await commond.ExecuteReaderAsync();
                var table = new DataTable();
                table.Load(reader);
                return table;
            }
        }
    }
}
