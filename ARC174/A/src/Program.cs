using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Collections;
namespace AtCorder
{
    public class Program : ProgramBase
    {
        public static void Main()
        {
            var first = GetLine<int>();
            var N = first[0];
            var C = first[1];

            var AList = GetLine<int>();

            var maxSum = 0;
            var maxList = new List<int>();
            foreach (var a in AList)
            {
                if (a)
                {

                }
            }
        }
    }

    #region 基底クラス

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
        public static CachedLines GetLines() => new CachedLines();

        /// <summary>
        /// 1行の入力を取得します。
        /// </summary>
        /// <returns>取得した文字列</returns>
        public static LineCache GetLine<T>()
        {
            new CachedLines().First();
        }

        /// <summary>
        /// 出力します。
        /// </summary>
        /// <param name="output">数字または文字列。</param>
        public static void Out(object output)
        {
            switch (output)
            {
                case string str:
                    Console.WriteLine(str);
                    break;
                case int integer:
                    Console.WriteLine(integer);
                    break;
                case bool boolean:
                    Console.WriteLine(boolean);
                    break;
                case IEnumerable<int> intCollection:
                    Console.WriteLine(string.Join(" ", intCollection.Select(w => w.ToString())));
                    break;
                case IEnumerable<string> stringCollection:
                    Console.WriteLine(string.Join(" ", stringCollection));
                    break;
                case IEnumerable<char> charCollection:
                    Console.WriteLine(string.Join("", charCollection));
                    break;
            };
        }

        #endregion
    }

    #endregion

    #region  標準入出力

    /// <summary>
    /// 入力を最後まで読み込み、キャッシュします。
    /// </summary>
    public class CachedLines : IEnumerable<LineCache>
    {
        #region  フィールド

        private readonly List<LineCache> cache = new();

        #endregion

        #region プロパティ

        /// <summary>
        /// キャッシュを取得します。
        /// </summary>
        public List<LineCache> Cache => cache;

        /// <summary>
        /// 入力がすべてキャッシュされたかどうか。
        /// </summary>
        public bool AllCached { get; private set; } = false;

        #endregion

        /// <summary>
        /// キャッシュされた行を含む、遅延読み込みされる行のシーケンスを取得します。
        /// </summary>
        public IEnumerator<LineCache> GetEnumerator()
        {
            foreach (var lineCache in cache)
            {
                yield return lineCache;
            }

            using (var sr = new StreamReader(Console.OpenStandardInput()))
            {
                while (!AllCached)
                {
                    var line = new LineCache(sr.ReadLine());
                    if (!line.Any())
                    {
                        AllCached = true;
                        yield break;
                    }
                    else
                    {
                        cache.Add(line);
                        yield return line;
                    }
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// 行を読み込み、キャッシュします。
    /// </summary>
    public class LineCache : IEnumerable<string>
    {
        #region フィールド
        /// <summary>
        /// コンソールの入力のキャッシュ
        /// </summary>
        private readonly ReadOnlyMemory<char> inputMemory;

        /// <summary>
        /// キャッシュ。
        /// </summary>
        private readonly List<object> cache = new();

        /// <summary>
        /// 各要素の変換ロジック。
        /// </summary>
        private Action parseLogic;

        #endregion
        #region 構築・消滅

        public LineCache(string? s, Action<string, object> parseLogic) : base()
        {
            inputMemory = s.AsMemory();
            parseLogic = parseLogic;
        }

        public IEnumerator<string> GetEnumerator()
        {
            // 既にキャッシュされている行を返す
            foreach (var section in cache)
            {
                yield return section;
            }

            if (inputMemory.IsEmpty) yield break;

            ReadOnlySpan<char> span = inputMemory.Span;
            while (span.Length > 0)
            {
                int spaceIndex = span.IndexOf(' ');
                if (spaceIndex != -1)
                {
                    var value = parseLogic.Invoke(new string(span));
                    yield return value;
                    cache.Add(value);
                    yield break;
                }

                var value = parseLogic.Invoke(new string(span[..spaceIndex]));
                yield return value;
                cache.Add(value);
                span = span[(spaceIndex + 1)..];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // インデクサの定義
        public string this[int index]
        {
            get
            {
                _ = this.Skip(cache.Count - index + 1);
                return cache[index];
            }
        }
    }

    #endregion

    #region データ型定義

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
        private readonly T[] buffer;
        private int head;
        private int tail;
        private int count;
        private readonly int capacity;
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

    #endregion
}