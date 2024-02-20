using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace AtCorder
{
    public class Program : ProgramBase
    {
        public static void Main()
        {
            var inputs = GetLines();
            Out(inputs.First());
        }
    }

    /// <summary>
    /// ユーティリティ関数を定義します。
    /// </summary>
    public class ProgramBase
    {
        #region 標準入出力

        /// <summary>
        /// 入力をすべて取得します。
        /// </summary>
        /// <returns>入力された文字列群。行単位で取得します。</returns>
        public static IEnumerable<List<string>> GetLines()
        {
            while (true)
            {
                var line = GetLine();
                if (line == null) break;
                yield return line;
            }
        }

        /// <summary>
        /// 1行の入力を取得します。
        /// </summary>
        /// <returns>取得した文字列</returns>
        public static List<string>? GetLine()
        {
            return Console.ReadLine()?.Split(" ").ToList();
        }

        /// <summary>
        /// 出力します。
        /// </summary>
        /// <param name="output">数字または文字列。</param>
        public static void Out(object? output) => Console.WriteLine(output);

        /// <summary>
        /// 出力します。
        /// </summary>
        /// <param name="output">数字または文字列。</param>
        public static void Write(object? output) => Out(output);

        /// <summary>
        /// 出力します。
        /// </summary>
        /// <param name="output">数字または文字列。</param>
        public static void ConWrite(object? output) => Out(output);

        /// <summary>
        /// 文字の列挙を1行で出力します。
        /// </summary>
        /// <param name="words">単語群。</param>
        public static void Out(IEnumerable<string> words) => Console.WriteLine(string.Join(" ", words));

        /// <summary>
        /// 文字の列挙を1行で出力します。
        /// </summary>
        /// <param name="words">単語群。</param>
        public static void Write(IEnumerable<string> words) => Out(words);

        /// <summary>
        /// 文字の列挙を1行で出力します。
        /// </summary>
        /// <param name="words">単語群。</param>
        public static void ConWrite(IEnumerable<string> words) => Out(words);

        #endregion
    }
}