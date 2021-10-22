using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Bookkeeper.Tests
{
    [TestFixture]
    [Category("Integration")]
    class IntegrationTests
    {
        private readonly string LocationOfTestFiles = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestFiles");

        [Test]
        public void ShouldProcessOfficialSmallAbnAmroBankExportFile()
        {
            var categoryMapJsonString =
                "{" +
                    "\"Categories\": " +
                    "[ " +
                        "{" +
                            "\"Keyword\": \"zorgverzekeringen\"," +
                            "\"CategoryName\": \"healthcare\" " +
                        "}," +
                        "{" +
                            "\"Keyword\": \"contactalook\"," +
                            "\"CategoryName\": \"healthcare\" " +
                        "}," +
                        "{" +
                            "\"Keyword\": \"cars are us\"," +
                            "\"CategoryName\": \"car\" " +
                        "}" +
                    "]" +
                "}";

            var inputFileData = File.ReadAllText(Path.Combine(LocationOfTestFiles, "SMALL_ABN_AMRO_EXPORT_FILE.TAB"));
            var parser = new TransactionParser(inputFileData);
            var categorizer = new TransactionCategorizer(parser, categoryMapJsonString);
            var categorizedResult = categorizer.CategorizeTransactions();
            var cr = new CategoryReporter(categorizedResult);

            var report = cr.CreateReport();

            StringAssert.Contains("car: -350.12 EUR", report);
            StringAssert.Contains("healthcare: -1259.56 EUR", report);
            StringAssert.Contains("unknown: -1.23 EUR", report);
            StringAssert.Contains("[UNKNOWN] -1.23 EUR @ 1/4/2021 12:00:00 AM - BEA   NR:CT653123 04.01.21/16.33 CCV POSTMASTERS,PAS123   AMSTERDAM", report);
        }

        [Test]
        public void ShouldProcessOfficialLargeAbnAmroBankExportFile()
        {
            var inputFileData = File.ReadAllText(@"C:\dev\export_of_2020.TAB");
            var inputCategoryMapJson = File.ReadAllText(Path.Combine(LocationOfTestFiles, @"C:\dev\categories.json"));
            var parser = new TransactionParser(inputFileData);
            var categorizer = new TransactionCategorizer(parser, inputCategoryMapJson);
            var categorizedResult = categorizer.CategorizeTransactions();
            var cr = new CategoryReporter(categorizedResult);

            var report = cr.CreateReport();
        }

        [Test]
        public void CanCategorizeExpensesForHousing()
        {
            var inputFileData = File.ReadAllText(@"C:\dev\export_of_2020.TAB");
            var parser = new TransactionParser(inputFileData);
            var parsedResult = parser.ParseExpenses();
            var housing = new MainExpenseCategory("Housing", new string[] {
                "Termijnbetaling hy potheek PERIODE",
                "Reden incasso:", // Gemeente bergeijk afval en OZB
                "OXXIO NEDERLAND BV         Machtiging:", // OXXIO ENERGIE  
                "ASR SCHADEVERZEKERING      Machtiging:", // Only ASR can also be credit nota when receiving damage payout
                "BRABANT WATER NV",
                "Waterschap De Dommel"}); // future introduce: taxes, food, insurance, health, entertainment
                                                              //var morgage = new CategoryEntry(housing, "Termijnbetaling hy potheek PERIODE");
            SummizeExpensesFor(parsedResult, housing);

            Assert.IsTrue(housing.TotalAmount < 0.0M);
        }

        private static void SummizeExpensesFor(List<Transaction> parsedResult, MainExpenseCategory expenseCategory)
        {
            foreach (var transaction in parsedResult)
            {
                foreach(var keyword in expenseCategory.DescriptionKeywords)
                {
                    if(transaction.Description.Contains(keyword))
                    {
                        expenseCategory.TotalAmount += transaction.Amount;
                    }
                }
            }
        }
    }
}
