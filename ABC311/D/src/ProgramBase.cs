using System;
using System.Linq;

namespace AtCorder
{
    /// <summary>
    /// ���[�e�B���e�B�֐����`���܂��B
    /// </summary>
    public class ProgramBase
    {
        #region �W�����o��

        /// <summary>
        /// ���͂����ׂĎ擾���܂��B
        /// </summary>
        /// <returns>���͂��ꂽ������Q�B�s�P�ʂŎ擾���܂��B</returns>
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
        /// �o�͂��܂��B
        /// </summary>
        /// <param name="output">�����܂��͕�����B</param>
        public static void OutputWordOrNumber(object? output) => Console.WriteLine(output);

        /// <summary>
        /// �����̗񋓂�1�s�ŏo�͂��܂��B
        /// </summary>
        /// <param name="words">�P��Q�B</param>
        public static void OutputWords(IEnumerable<string> words) => Console.WriteLine(string.Join(" ", words));

        #endregion
    }
}