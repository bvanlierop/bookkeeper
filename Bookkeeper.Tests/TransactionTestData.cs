namespace Bookkeeper.Tests
{
    public static class TransactionTestData
    {
        private const string AbnAmroTabFileColumnSplitter = "\t";
        private const string AbnAmroTabFileLineSplitter = "\r\n";

        public static readonly string ValidTransactionLineWithCreditAmount = $"123551334{AbnAmroTabFileColumnSplitter}EUR{AbnAmroTabFileColumnSplitter}20201218{AbnAmroTabFileColumnSplitter}12958,40{AbnAmroTabFileColumnSplitter}12954,88{AbnAmroTabFileColumnSplitter}20201218{AbnAmroTabFileColumnSplitter}17,75{AbnAmroTabFileColumnSplitter}ACME Company";
        public static readonly string ValidTransactionLineWithDebitAmount = $"123551334{AbnAmroTabFileColumnSplitter}EUR{AbnAmroTabFileColumnSplitter}20201218{AbnAmroTabFileColumnSplitter}12958,40{AbnAmroTabFileColumnSplitter}12954,88{AbnAmroTabFileColumnSplitter}20201218{AbnAmroTabFileColumnSplitter}-3,52{AbnAmroTabFileColumnSplitter}ACME Company";

        public static string TwoValidTransactions => $"{ValidTransactionLineWithCreditAmount}{AbnAmroTabFileLineSplitter}{ValidTransactionLineWithDebitAmount}";

        public static readonly string InvalidTransactionLineWithInvalidAmount = "123551334	EUR	20201218	12958,40	12954,88	20201218	-x,0y	ACME Company";
    }
}
