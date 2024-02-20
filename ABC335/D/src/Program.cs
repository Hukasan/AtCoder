using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Data;

namespace AtCorder
{
    public class Program : ProgramBase
    {
        public static void Main()
        {
            var N = Line(true)[0].ToInt();

            var grid = CreateGridArray<string>(N, N);

            var pos = (X: 0, Y: 0);

            foreach (var num in Enumerable.Range(1, N * N - 1))
            {
                grid[pos.Y][pos.X] = num.ToString();
                if (pos.X < N - 1 && grid[pos.Y][pos.X + 1] == null && (pos.Y == 0 || grid[pos.Y - 1][pos.X] != null))
                    pos.X++;
                else if (pos.Y < N - 1 && grid[pos.Y + 1][pos.X] == null)
                    pos.Y++;
                else if (pos.X > 0 && grid[pos.Y][pos.X - 1] == null)
                    pos.X--;
                else if (pos.Y > 0 && grid[pos.Y - 1][pos.X] == null)
                    pos.Y--;
            }

            grid[((N + 1) / 2) - 1][((N + 1) / 2) - 1] = "T";

            foreach (var column in grid)
            {
                Outs(column);
            }
        }
    }

    #region ベースクラス

    /// <summary>
    /// ユーティリティ関数を定義します。
    /// </summary>
    public class ProgramBase
    {
        #region データ作成

        /// <summary>
        /// 行-列の順序の2次元配列を作成します。
        /// 行ごとの取得が必要な場合に利用します。
        /// </summary>
        /// <param name="columnCount">行数。</param>
        /// <param name="rowCount">列数。</param>
        public static T?[][] CreateGridArray<T>(int columnCount, int rowCount) => Enumerable.Range(0, columnCount).Select(_ => new T?[rowCount]).ToArray();

        /// <summary>
        /// インデックスの文字列をキーとした辞書を作成します。
        /// </summary>
        /// <typeparam name="T">辞書の値の型</typeparam>
        /// <param name="maxCount">要素数</param>
        /// <param name="defaultValueSetter">辞書の値の初期化処理。</param>
        /// <param name="keys">作成した辞書のキー一覧。</param>
        /// <param name="startIdx">キーの最初のインデックス</param>
        /// <returns>作成した辞書</returns>
        public static Dictionary<string, T> CreateIndexKeyDictionary<T>(int maxCount, Func<int, T> defaultValueSetter, out IEnumerable<string> keys, int startIdx = 0)
        {
            // 辞書を初期化
            var dictionary = new Dictionary<string, T>();

            var keys_in = new List<string>();
            // 0からmaxNumberまでの数を辞書に追加
            for (int i = startIdx; i <= maxCount + startIdx - 1; i++)
            {
                var str = i.ToString();
                keys_in.Add(str);
                // 数値を文字列に変換してキーとして使用
                dictionary.Add(str, defaultValueSetter(i));
            }
            keys = keys_in;
            return dictionary;
        }

        #endregion

        #region 標準入出力

        /// <summary>
        /// 入力をすべて取得します。
        /// </summary>
        /// <returns>入力された文字列群。行単位で取得します。</returns>
        public static IEnumerable<List<string>> GetLines()
        {
            while (true)
            {
                var line = Line();
                if (line == null) break;
                yield return line;
            }
        }

        /// <summary>
        /// 入力をすべて取得します。
        /// </summary>
        /// <returns>入力された文字列群。行単位で取得します。</returns>
        public static IEnumerable<List<string>> Lines() => GetLines();


        /// <summary>
        /// 1行の入力を取得します。
        /// </summary>
        /// <returns>取得した文字列</returns>

        public static List<string>? Line()
        {
            var line = Console.ReadLine()?.Split(" ").ToList();
            return line;
        }

        /// <summary>
        /// 1行の入力を取得します。
        /// </summary>
        /// <returns>取得した文字列</returns>

        public static List<string> Line(bool isNotNull = true) => Line()!;

        public static (string, string, string) Line(int idx1, int idx2, int idx3)
        {
            var l = Line(true);
            return (l[idx1], l[idx2], l[idx3]);
        }

        /// <summary>
        /// 出力します。
        /// </summary>
        /// <param name="output">数字または文字列。</param>
        public static void Out(object output) => Console.WriteLine(output);

        /// <summary>
        /// 1行で出力します。
        /// </summary>
        /// <param name="objects">データ群。</param>
        public static void Outs<T>(IEnumerable<T> objects) => Console.WriteLine(string.Join(" ", objects.Select(w =>
        {
            return w?.ToString() ?? "null";
        })));

        /// <summary>
        /// 文字列群を1行で出力します。
        /// </summary>
        /// <param name="objects">文字列群。</param>
        public static void Out(IEnumerable<string> words) => Outs(words);

        /// <summary>
        /// 数値群を1行で出力します。
        /// </summary>
        /// <param name="numbers">数値群。</param>
        public static void Out(IEnumerable<int> numbers) => Outs(numbers);

        /// <summary>
        /// 数値群を1行で出力します。
        /// </summary>
        /// <param name="numbers">数値群。</param>
        public static void Out(params int[] numbers) => Outs(numbers);

        /// <summary>
        /// 文字列群を1行で出力します。
        /// </summary>
        /// <param name="numbers">文字列群。</param>
        public static void Out(params string[] words) => Outs(words);

        #endregion
    }

    #endregion

    #region  拡張メソッド

    public static class StringExt
    {
        /// <summary>
        /// 文字列を数値に変換します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns>数値。</returns>
        public static int ToInt(this string str) => int.Parse(str);

        /// <summary>
        /// 文字列を数値に変換します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns>数値。</returns>
        public static long ToLong(this string str) => long.Parse(str);
    }

    #endregion

}