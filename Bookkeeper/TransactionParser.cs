using System;
using System.Collections.Generic;

namespace Bookkeeper
{
    public class TransactionParser
    {
        private readonly string AbnAmroTabbedLineSeparator = Environment.NewLine;
        private readonly string TransactionData; 

        public List<Transaction> Transactions { get; set; }

        public TransactionParser(string transactionData)
        {
            TransactionData = transactionData;
            Transactions = new List<Transaction>();
        }

        public void Parse()
        {
            Transactions.Clear();

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
                    var t = new Transaction {
                        Amount = decimal.Parse(line.Split('\t')[6].Replace(",", "."))
                    };
                    
                    Transactions.Add(t);
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Unable to process transactional data.", ex);
            }
        }
    }
}
