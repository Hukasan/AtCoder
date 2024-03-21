using System.Collections;
using System.Text;

namespace AtCorder;

public class Program : ProgramBase
{
    public static void Main()
    {
        C.BoolOutputOption = BoolOutputOption.YesNo;
        var headLine = C.GetLine<int>();
        var N = headLine[0];

        var lines = C.GetLines<int>();

        foreach (var line in lines)
        {
            foreach (var value in line)
            {
                C.Out(value);
            };
        }

        C.Out(N);
    }
}

#region 基底クラス

public class ProgramBase
{
    public static ConsoleManager C = new();

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
}

#endregion

#region  標準入出力

public class ConsoleManager : IDisposable
{
    #region フィールド

    /// <summary>
    /// 入力ストリーム
    /// </summary>
    private readonly StreamReader m_ConsoleReader;

    #endregion

    #region プロパティ

    /// <summary>
    /// 真偽値の出力オプション
    /// </summary>
    public BoolOutputOption BoolOutputOption { get; set; }

    #endregion

    #region 構築・消滅

    public ConsoleManager(BoolOutputOption boolOutputOption = BoolOutputOption.TrueFalse)
    {
        m_ConsoleReader = new StreamReader(Console.OpenStandardInput());
        BoolOutputOption = boolOutputOption;
    }

    public void Dispose()
    {
        m_ConsoleReader.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion

    /// <summary>
    /// 1行の入力を取得します。
    /// </summary>
    public LineCache<string> GetLine() => new(m_ConsoleReader, w => w);

    /// <summary>
    /// 1行の入力を取得します。
    /// </summary>
    /// <returns>取得した文字列</returns>
    public LineCache<T> GetLine<T>() where T : struct => new(m_ConsoleReader, GetParseLogic<T>());

    /// <summary>
    /// 入力をすべて取得します。
    /// </summary>
    /// <returns>入力された文字列群。行単位で取得します。</returns>
    public CachedLines<T> GetLines<T>() where T : struct => new(m_ConsoleReader, GetParseLogic<T>());

    /// <summary>
    /// 入力文字列の変換処理を定義します。
    /// </summary>
    /// <typeparam name="T">変換先の値型。</typeparam>
    /// <returns>変換処理。</returns>
    /// <exception cref="NotSupportedException"></exception>
    private static Func<string, T> GetParseLogic<T>()
    {
        if (typeof(T) == typeof(int))
            return (Func<string, T>)(object)(Func<string, int>)(s => int.Parse(s));
        else if (typeof(T) == typeof(Int128))
            return (Func<string, T>)(object)(Func<string, Int128>)(s => Int128.Parse(s));
        else if (typeof(T) == typeof(long))
            return (Func<string, T>)(object)(Func<string, long>)(s => long.Parse(s));
        else if (typeof(T) == typeof(double))
            return (Func<string, T>)(object)(Func<string, double>)(s => double.Parse(s));
        else if (typeof(T) == typeof(bool))
            return (Func<string, T>)(object)(Func<string, bool>)(s => bool.Parse(s));
        else
            throw new NotSupportedException($"対応していない型 {typeof(T)} が渡された");
    }

    /// <summary>
    /// 出力します。
    /// </summary>
    /// <param name="output">数字または文字列。</param>
    public void Out(object output)
    {
        switch (output)
        {
            case string:
            case int:
            case Int128:
            case float:
            case long:
                Console.WriteLine(output);
                break;
            case bool boolean:
                switch (BoolOutputOption)
                {
                    case BoolOutputOption.TrueFalse:
                        Console.WriteLine(boolean ? "True" : "False");
                        break;
                    case BoolOutputOption.YesNo:
                        Console.WriteLine(boolean ? "Yes" : "No");
                        break;
                    case BoolOutputOption.OkNg:
                        Console.WriteLine(boolean ? "OK" : "NG");
                        break;
                }
                break;
            case IEnumerable<string> stringCollection:
                Console.WriteLine(string.Join(" ", stringCollection));
                break;
            case IEnumerable objects:
                Console.WriteLine(string.Join(" ", ((IEnumerable<object>)output).Select(w => w.ToString())));
                break;
        };
    }
}

/// <summary>
/// 真偽値の出力オプション
/// </summary>
public enum BoolOutputOption
{
    TrueFalse,
    YesNo,
    OkNg,
}

/// <summary>
/// 入力を最後まで読み込み、キャッシュします。
/// </summary>
public class CachedLines<T> : IEnumerable<LineCache<T>>
{
    #region  フィールド

    /// <summary>
    /// 入力ストリーム。
    /// </summary>
    private readonly StreamReader m_StreamReader;

    /// <summary>
    /// キャッシュ。
    /// </summary>
    private readonly List<LineCache<T>> cache = new();

    /// <summary>
    /// 文字列変換ロジック。
    /// </summary>
    private readonly Func<string, T> m_ParseLogic;

    #endregion

    #region プロパティ

    /// <summary>
    /// キャッシュを取得します。
    /// </summary>
    public List<LineCache<T>> Cache => cache;

    /// <summary>
    /// 入力がすべてキャッシュされたかどうか。
    /// </summary>
    public bool AllCached { get; private set; } = false;

    #endregion

    #region 構築・消滅

    public CachedLines(StreamReader streamReader, Func<string, T> parseLogic)
    {
        m_StreamReader = streamReader;
        m_ParseLogic = parseLogic;
    }

    #endregion

    /// <summary>
    /// キャッシュされた行を含む、遅延読み込みされる行のシーケンスを取得します。
    /// </summary>
    public IEnumerator<LineCache<T>> GetEnumerator()
    {
        foreach (var lineCache in cache)
        {
            yield return lineCache;
        }
        while (!AllCached)
        {
            var line = new LineCache<T>(m_StreamReader, m_ParseLogic);
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
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// 行を読み込み、空白文字で分割して取得します。
/// </summary>
public class LineCache<T> : IEnumerable<T>
{
    #region フィールド

    /// <summary>
    /// 入力ストリーム。
    /// </summary>
    private readonly StreamReader m_StreamReader;

    /// <summary>
    /// キャッシュ。
    /// </summary>
    private readonly List<T> m_Cache = new();

    /// <summary>
    /// 各要素の変換ロジック。
    /// </summary>
    private readonly Func<string, T> m_ParseLogic;

    #endregion

    #region 構築・消滅

    public LineCache(StreamReader streamReader, Func<string, T> parseLogic)
    {
        m_StreamReader = streamReader;
        m_ParseLogic = parseLogic;
    }

    #endregion

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var value in m_Cache)
        {
            yield return value;
        }

        foreach (var value in GetLine())
        {
            yield return value;
        }
    }

    private IEnumerable<T> GetLine()
    {
        var currentPart = new StringBuilder();
        while (true)
        {
            int charCode = m_StreamReader.Read();
            char character = (char)charCode;

            if (charCode == -1 || character == '\n')
            {
                if (currentPart.Length <= 0) yield break;
                var value = m_ParseLogic(currentPart.ToString());
                m_Cache.Add(value);
                yield return value;
                yield break;
            }
            else if (character == ' ')
            {
                if (currentPart.Length <= 0) yield break;
                var value = m_ParseLogic(currentPart.ToString());
                m_Cache.Add(value);
                currentPart.Clear();
                yield return value;
            }
            else if (character == '\r')
            {
                continue;
            }
            else
            {
                currentPart.Append(character);
            }
        }
    }

    private T GetValueWithIdx(int index)
    {
        if (m_Cache.Count - 1 >= index) return m_Cache[index];
        var currentIdx = m_Cache.Count - 1;
        foreach (var value in GetLine())
        {
            currentIdx++;
            if (currentIdx == index) return value;
        }
        throw new ArgumentOutOfRangeException("不正な添え字指定");
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    // インデクサの定義
    public T this[int index]
    {
        get
        {
            return GetValueWithIdx(index);
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
