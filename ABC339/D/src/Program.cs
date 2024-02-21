using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
namespace AtCorder
{
    public class Program : ProgramBase
    {
        public static void Main()
        {
            var input = Line(true);
            var N = input[0].ToInt();
            var D = input[1].ToInt();
            var AList = Line(true);

            var lessCount = AList.Where(a => a.ToInt() < D).Count();
            var moreCount = AList.Count - lessCount;
            var kirisute = moreCount / (lessCount + 1);

            if (kirisute != 0)
            {
                if (moreCount % (lessCount + 1) != 0)
                    Out(kirisute + 1);
                else
                    Out(kirisute);
            }
            else Out(1);
        }
    }

    /// <summary>
    /// ユーティリティ関数を定義します。
    /// </summary>
    public class ProgramBase
    {
        #region データ作成
        /// <summary>
        /// 指定された範囲の数値をキーとした辞書を作成します。
        /// </summary>
        /// <typeparam name="T">辞書の値の型。</typeparam>
        /// <param name="maxCount">数値の最大値。</param>
        /// <param name="defaultValueSetter">デフォルト値を設定するための関数。</param>
        /// <param name="keys">作成されたキーのコレクション。</param>
        /// <param name="startIdx">数値の開始インデックス。</param>
        /// <returns>作成された辞書。</returns>
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
        /// <param name="isNotNull">nullを許容するかどうか。</param>
        /// <returns>取得した文字列</returns>
        public static List<string> Line(bool isNotNull = true) => Line()!;
        /// <summary>
        /// 指定されたインデックスの要素を含む1行の入力を取得します。
        /// </summary>
        /// <param name="idx1">要素のインデックス1。</param>
        /// <param name="idx2">要素のインデックス2。</param>
        /// <param name="idx3">要素のインデックス3。</param>
        /// <returns>取得した要素のタプル。</returns>
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
        /// 出力します。
        /// </summary>
        /// <param name="output">数字または文字列。</param>
        public static void Write(object output) => Out(output);
        /// <summary>
        /// 出力します。
        /// </summary>
        /// <param name="output">数字または文字列。</param>
        public static void ConWrite(object output) => Out(output);
        /// <summary>
        /// 文字の列挙を1行で出力します。
        /// </summary>
        /// <param name="words">単語群。</param>
        public static void Out(IEnumerable<int> words) => Console.WriteLine(string.Join(" ", words.Select(w => w.ToString())));
        /// <summary>
        /// 文字の列挙を1行で出力します。
        /// </summary>
        /// <param name="words">単語群。</param>
        public static void Out(params int[] words) => Out((IEnumerable<int>)words);
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
    /// <summary>
    /// 文字列の拡張メソッドを提供します。
    /// </summary>
    public static class StringExt
    {
        /// <summary>
        /// 文字列を数値に変換します。
        /// </summary>
        /// <param name="str">変換する文字列。</param>
        /// <returns>変換された数値。</returns>
        public static int ToInt(this string str) => int.Parse(str);
        /// <summary>
        /// 文字列を数値に変換します。
        /// </summary>
        /// <param name="str">変換する文字列。</param>
        /// <returns>変換された数値。</returns>
        public static long ToLong(this string str) => long.Parse(str);
    }
    /// <summary>
    /// リングバッファを提供します。
    /// </summary>
    /// <typeparam name="T">要素の型。</typeparam>
    public class CircularBuffer<T>
    {
        private T[] buffer;
        private int head;
        private int tail;
        private int count;
        private int capacity;
        /// <summary>
        /// インスタンスを構築します。
        /// </summary>
        /// <param name="capacity">バッファの容量</param>
        public CircularBuffer(int capacity)
        {
            this.capacity = capacity;
            buffer = new T[capacity];
            head = 0;
            tail = 0;
            count = 0;
        }
        /// <summary>
        /// バッファがいっぱいかどうかを取得します。
        /// </summary>
        public bool IsFull => count == capacity;
        /// <summary>
        /// バッファが空かどうかを取得します。
        /// </summary>

        public bool IsEmpty => count == 0;
        /// <summary>
        /// バッファに要素を追加します。
        /// </summary>
        /// <param name="item">追加する要素</param>
        public void Enqueue(T item)
        {
            if (IsFull)
            {
                throw new InvalidOperationException("Buffer is full");
            }
            buffer[tail] = item;
            tail = (tail + 1) % capacity;
            count++;
        }
        /// <summary>
        /// バッファから要素を取り出します。
        /// </summary>
        /// <returns>取り出した要素</returns>
        public T Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Buffer is empty");
            }
            T item = buffer[head];
            head = (head + 1) % capacity;
            count--;
            return item;
        }
    }
}