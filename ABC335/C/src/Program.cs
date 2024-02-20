using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Specialized;

namespace AtCorder
{
    public class Program : ProgramBase
    {
        public static void Main()
        {
            var N = Line(true)[0].ToInt();
            var queries = GetLines();

            // 移動ログ起点をしっぽに設定。
            // 初期状態で頭の点がしっぽから移動されているものとする。
            var moveLog = new List<(int X, int Y)>();
            foreach (var n in Enumerable.Range(1, N).Reverse())
                moveLog.Add((n - N, 0));

            foreach (var query in queries)
            {
                var queryType = query[0];
                var queryValue = query[1];
                switch (queryType)
                {
                    case "1":
                        var newMoveLog = moveLog.Last();
                        switch (queryValue)
                        {
                            case "R":
                                newMoveLog.X += 1;
                                break;
                            case "L":
                                newMoveLog.X -= 1;
                                break;
                            case "U":
                                newMoveLog.Y += 1;
                                break;
                            case "D":
                                newMoveLog.Y -= 1;
                                break;
                            default:
                                break;
                        }
                        moveLog.Add(newMoveLog);
                        break;
                    case "2":
                        var partsNo = queryValue.ToInt();
                        var (x, y) = moveLog[^partsNo];
                        // 末尾から起点をずらす。
                        Out(x + N, y);
                        break;
                    default:
                        break;
                }
            }

        }
    }

    /// <summary>
    /// ユーティリティ関数を定義します。
    /// </summary>
    public class ProgramBase
    {
        #region データ作成

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
        public static void Outs<T>(IEnumerable<T> objects) where T : notnull => Console.WriteLine(string.Join(" ", objects.Select(w => w.ToString())));

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
        public static void Out(params int[] numbers) => Out((IEnumerable<int>)numbers);

        /// <summary>
        /// 文字列群を1行で出力します。
        /// </summary>
        /// <param name="numbers">文字列群。</param>
        public static void Out(params string[] words) => Out((IEnumerable<string>)words);

        #endregion
    }

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
}