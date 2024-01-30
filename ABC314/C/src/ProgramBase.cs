using System;
using System.Linq;

namespace AtCorder
{
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
        public static IEnumerable<List<string>> GetInputs()
        {
            while (true)
            {
                var lineTxt = Console.ReadLine();
                if (string.IsNullOrEmpty(lineTxt)) break;
                yield return lineTxt.Split(" ").ToList();
            }
        }

        /// <summary>
        /// 出力します。
        /// </summary>
        /// <param name="output">数字または文字列。</param>
        public static void OutputWordOrNumber(object? output) => Console.WriteLine(output);

        /// <summary>
        /// 文字の列挙を1行で出力します。
        /// </summary>
        /// <param name="words">単語群。</param>
        public static void OutputWords(IEnumerable<string> words) => Console.WriteLine(string.Join(" ", words));

        #endregion
    }
}