using System;
using System.Collections.Generic;

namespace Bookkeeper
{
    public class TransactionParser
    {
        private readonly string AbnAmroTabbedLineSeparator = Environment.NewLine;
        private readonly string TransactionData; 

        public List<string> Transactions { get; set; }

        public TransactionParser(string transactionData)
        {
            TransactionData = transactionData;
            Transactions = new List<string>();
        }

        public void Parse()
        {
            if (string.IsNullOrWhiteSpace(TransactionData))
            {
                throw new ArgumentException("Cannot parse transactions, no input transactions given.");
            }

            Transactions.AddRange(TransactionData.Split(
                new[] { AbnAmroTabbedLineSeparator },
                StringSplitOptions.None));
        }
    }
}
