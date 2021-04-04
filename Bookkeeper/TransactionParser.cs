using System;
using System.Collections.Generic;

namespace Bookkeeper
{
    public class TransactionParser : ITransactionParser
    {
        private readonly string AbnAmroTabbedLineSeparator = Environment.NewLine;
        private readonly string TransactionData; 

        public TransactionParser(string transactionData)
        {
            TransactionData = transactionData;
        }

        public List<Transaction> Parse()
        {
            var transactions = new List<Transaction>();

            if (string.IsNullOrWhiteSpace(TransactionData))
            {
                throw new ArgumentException("Cannot parse transactions, no input transactions given.");
            }

            var lines = TransactionData.Split(
                new[] { AbnAmroTabbedLineSeparator },
                StringSplitOptions.None);

            try
            {
                foreach(var line in lines)
                {
                    var t = new Transaction(
                        decimal.Parse(line.Split('\t')[6].Replace(",", ".")),
                        line.Split('\t')[7].Trim());
                    
                    transactions.Add(t);
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Unable to process transactional data.", ex);
            }

            return transactions;
        }
    }
}
