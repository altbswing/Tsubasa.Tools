using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Tsubasa.Csv {

    /// <summary>
    /// CSV出力処理
    /// </summary>
    public partial class CsvAdapter<TEntity> {

        /// <summary>
        /// メモリ管理用、各型の行ごとに必要な文字数をざっくり計算する
        /// </summary>
        private static int _LineCapacity = 128;

        /// <summary>
        /// Entityの集合型をCSVに変換する
        /// </summary>
        /// <typeparam name="TEntity">Entityの型</typeparam>
        /// <param name="list">Entityの集合</param>
        /// <param name="headerMode">ヘッダー出力方式</param>
        /// <returns>CSV文字列</returns>
        public string ToCsv(List<TEntity> list)
        {
            // nullチェック
            if (list == null) {
                throw new NullReferenceException("nullの集合に対し、csvを作成できません。");
            }
            // StringBuilderの初期サイズ
            var type = typeof(TEntity);
            // csv行を格納するためのリスト（予めメモリを確保）
            var builder = new StringBuilder(_LineCapacity * (list.Count + 1));
            // ヘッダー作成
            var header = CreateHeader();
            builder.AppendLine(header);
            // 行ごとに
            foreach (var row in list) {
                // csv行作成
                var line = ToCsvLine(row);
                builder.AppendLine(line);
                // 行の最大長さを記憶 (改行"\r\n"の2文字含めて計上)
                _LineCapacity = Math.Max(_LineCapacity, line.Length + 2);
            }
            // 文字列
            var csv = builder.ToString().Trim();
            return csv;
        }

        /// <summary>
        /// Entityの型からCsvのヘッダーを作成する
        /// </summary>
        /// <typeparam name="TEntity">Entityの型</typeparam>
        /// <param name="headerMode">ヘッダー方式</param>
        /// <returns></returns>
        public string CreateHeader()
        {
            // ヘッダーなし
            if (this.HeaderMode == HeaderMode.None) {
                return null;
            }
            // ヘッダー項目（表示名）
            var itemList = new List<string>();
            // プロパティ毎に
            foreach (var p in _PropertyList) {
                // csv設定
                var attr = p.GetCustomAttribute<CsvItemAttribute>();
                // デフォルトはプロパティ名
                var item = p.Name;
                // 表示名の場合
                if (this.HeaderMode == HeaderMode.Display) {
                    var display = p.GetCustomAttribute<DisplayAttribute>();
                    if (display == null) {
                        throw new CsvDefineException(
                            $"「{typeof(TEntity).Name}.{p.Name}」に「{nameof(DisplayAttribute)}」が設定されていません。");
                    }
                    item = display.GetName();
                }
                // エスケープ処理
                item = attr.EscapeCsv(item);
                itemList.Add(item);
            }
            return string.Join(",", itemList);
        }

        /// <summary>
        /// Entityからcsvの行を作成する
        /// </summary>
        /// <typeparam name="TEntity">Entityの型</typeparam>
        /// <param name="row">Entity情報</param>
        /// <returns>一行のcsv</returns>
        public string ToCsvLine(TEntity row)
        {
            // nullチェック
            if (row == null) {
                throw new NullReferenceException("nullの対象に対し、csv行を作成できません。");
            }
            // CSV出力対象のプロパティを取得する
            var pList = _CsvProperties();
            // 行内csv項目
            var itemList = new List<string>();
            // プロパティごとに
            foreach (var p in pList) {
                // csv設定
                var attr = p.GetCustomAttribute<CsvItemAttribute>();
                // 項目値
                var value = p.GetValue(row);
                // csv文字列にする
                var item = attr.ToCsv(value);
                // エスケープ処理
                item = attr.EscapeCsv(item);
                itemList.Add(item);
            }
            // csv行作成
            var csvLine = string.Join(",", itemList);
            return csvLine;
        }

        /// <summary>
        /// CSVを一括ファイルに保存する
        /// </summary>
        /// <param name="list">対象リスト</param>
        /// <param name="file">保存先</param>
        public async Task SaveCsvAsync(List<TEntity> list, FileInfo file)
        {
            // フォルダー作成
            if (!file.Directory.Exists) {
                file.Directory.Create();
            }
            // csv作成
            var csv = ToCsv(list);
            // ファイルに非同期で保存
            Action action = () => File.WriteAllText(file.FullName, csv, this.Charset);
            await Task.Run(action);
        }
    }
}
