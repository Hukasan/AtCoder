using Microsoft.VisualStudio.TestTools.UnitTesting;
using AtCorder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AtCorder.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        #region 定数

        private const string c_TestDataFolderName = "TestData";

        #endregion

        #region メンバ

        public TestContext? TestContext { get; set; }

        private StreamReader? m_input;
        private StringWriter? m_output;

        #endregion

        #region テスト準備・テスト後処理

        [TestInitialize()]
        public void TestInitialize()
        {
            var output = new StringWriter();
            m_output = output;
            Console.SetOut(output);
        }

        [TestCleanup()]
        public void TestCleanUp()
        {
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            m_input?.Dispose();
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            m_output?.Dispose();
        }

        #endregion

        #region テスト

        /// <summary>
        /// オリジナルケースのテストです。
        /// </summary>
        [TestMethod()]
        public void MainTest_Case0()
        {
            SetTestInput("sample-0.in");
            Program.Main();
            VerifyOutput("sample-0.out");
        }

        /// <summary>
        /// ケース1のテストです。
        /// </summary>
        [TestMethod()]
        public void MainTest_Case1()
        {
            SetTestInput("sample-1.in");

            Program.Main();

            VerifyOutput("sample-1.out");
        }

        /// <summary>
        /// ケース2のテストです。
        /// </summary>
        [TestMethod()]
        public void MainTest_Case2()
        {
            SetTestInput("sample-2.in");

            Program.Main();

            VerifyOutput("sample-2.out");
        }

        /// <summary>
        /// ケース3のテストです。
        /// </summary>
        [TestMethod()]
        public void MainTest_Case3()
        {
            SetTestInput("sample-3.in");

            Program.Main();

            VerifyOutput("sample-3.out");
        }

        /// <summary>
        /// ケース4のテストです。
        /// </summary>
        [TestMethod()]
        public void MainTest_Case4()
        {
            SetTestInput("sample-4.in");

            Program.Main();

            VerifyOutput("sample-4.out");
        }

        /// <summary>
        /// ケース5のテストです。
        /// </summary>
        [TestMethod()]
        public void MainTest_Case5()
        {
            SetTestInput("sample-5.in");

            Program.Main();

            VerifyOutput("sample-5.out");
        }

        #endregion

        private void SetTestInput(string inputFileName)
        {
            try
            {
                var text = File.ReadAllText($"{c_TestDataFolderName}/{inputFileName}");
                if (string.IsNullOrEmpty(text))
                {
                    Assert.Inconclusive("このテストはスキップされました。");
                    return;
                }
                var stream = CreateStreamFromString(text);
                m_input = new StreamReader(stream);
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is ArgumentException || ex is ArgumentNullException)
            {
                Assert.Inconclusive("このテストはスキップされました。");
                return;
            }

            Console.SetIn(m_input);
            return;

            Stream CreateStreamFromString(string s)
            {
                var bytes = Encoding.UTF8.GetBytes(s);
                return new MemoryStream(bytes);
            }
        }

        private void VerifyOutput(string expectOutputFileName)
        {
            var expectOutputFilePath = $"{c_TestDataFolderName}/{expectOutputFileName}";
            var expect = File.ReadAllText(expectOutputFilePath);
            if (expect == null) return;
            expect = ConvertEOL(expect);
            File.WriteAllText(expectOutputFilePath, expect);
            var result = m_output?.ToString();
            if (result == null) Assert.Fail("結果がnullです。");
            result = ConvertEOL(result);

            if (expect != result)
            {
                Directory.CreateDirectory("TestResult");
                var testResultPath = $"TestResult/{expectOutputFileName}_result.out";
                File.WriteAllText(testResultPath, result);
                var appDataPath = Environment.GetEnvironmentVariable("APPDATA");
                var processStartInfo = new ProcessStartInfo("python", $"{appDataPath}\\atcoder-cli-nodejs\\Config\\.scripts\\DiffReporter.py {expectOutputFilePath} {testResultPath}")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };
                var process = Process.Start(processStartInfo);
                if (process == null)
                {
                    Assert.Fail("テストに失敗しています、差分ツールが開けませんでした。");
                    return;
                }
                process.WaitForExit();
                var StandardError = process.StandardError.ReadToEnd();
                if (StandardError.Any())
                    Assert.Fail(StandardError);
                var StandardOutput = process.StandardOutput.ReadToEnd();
                Assert.Fail("サンプルケースのテストに失敗しました。\n〇出力結果\n******\n" + result + "******\n〇期待値との比較(行(期待値 結果)  | +:余分, -:ない)\n******\n" + StandardOutput + "******\n");
            }
        }

        // 改行コードを統一
        private static string ConvertEOL(string text, string toEOL = "\n") => text.Replace("\r\n", toEOL).Replace("\r", toEOL).Replace("\n", toEOL);

    }
}