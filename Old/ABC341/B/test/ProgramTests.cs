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
            m_input?.DiscardBufferedData();
            m_output?.Flush();
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
            m_input = new StreamReader($"{c_TestDataFolderName}/{inputFileName}");

            Console.SetIn(m_input);
        }

        private void VerifyOutput(string expectOutputFileName)
        {
            var expectOutputFilePath = $"{c_TestDataFolderName}/{expectOutputFileName}";
            var expect = File.ReadAllText(expectOutputFilePath);
            if (expect == null) return;
            expect = ConvertEOL(expect);
            File.WriteAllText(expectOutputFilePath, expect);
            var expectLines = expect.Split("\n");
            var result = m_output?.ToString();
            if (result == null) Assert.Fail("結果がnullです。");
            result = ConvertEOL(result);
            var resultLines = result.Split("\n");

            if (expect != result)
            {
                Directory.CreateDirectory("TestResult");
                var testResultPath = $"TestResult/{expectOutputFileName}_result.out";
                File.WriteAllText(testResultPath, result);
                var processStartInfo = new ProcessStartInfo("python", $"C:\\Users\\hukat\\AppData\\Roaming\\atcoder-cli-nodejs\\Config\\.scripts\\DiffReporter.py {expectOutputFilePath} {testResultPath}")
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
                Assert.Fail("\nサンプルケースのテストに失敗しました。(+:余分, -:ない)\n******\n" + StandardOutput + "\n******\n");
            }
        }

        // 改行コードを統一
        private static string ConvertEOL(string text, string toEOL = "\n") => text.Replace("\r\n", toEOL).Replace("\r", toEOL).Replace("\n", toEOL);

    }
}