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
                    if(string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var dateString = line.Split('\t')[2].Trim();
                    var date = new DateTime(int.Parse(dateString.Substring(0, 4)), int.Parse(dateString.Substring(4, 2)), int.Parse(dateString.Substring(6, 2)));

                    var t = new Transaction(
                        decimal.Parse(line.Split('\t')[6].Replace(",", ".")),
                        line.Split('\t')[7].Trim(),
                        date);
                    
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
