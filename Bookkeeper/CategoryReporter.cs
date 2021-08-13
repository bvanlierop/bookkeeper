using System;
using System.Collections;
using System.Collections.Generic;

namespace Bookkeeper
{
    public class CategoryReporter
    {
        private Hashtable categorizedResult;

        public CategoryReporter(Hashtable categorizedResult)
        {
            this.categorizedResult = categorizedResult;
        }

        public string CreateReport()
        {
            string reportString = "IMPORT REPORT" + Environment.NewLine;

            var categoriesWithAmounts = new Dictionary<string, decimal>();
            var unknowns = new List<Transaction>();

            foreach(DictionaryEntry entry in categorizedResult)
            {
                var transaction = (Transaction)entry.Key;
                var category = (Category)entry.Value;

                if(category.Name.Equals("unknown"))
                {
                    unknowns.Add(transaction);
                }

                if(!categoriesWithAmounts.ContainsKey(category.Name))
                {
                    categoriesWithAmounts.Add(category.Name, transaction.Amount);
                }
                else 
                {
                    categoriesWithAmounts[category.Name] += transaction.Amount;
                }

            }

            foreach(var entry in categoriesWithAmounts)
            {
                reportString += $"{entry.Key}: {entry.Value} EUR" + Environment.NewLine; 
            }

            foreach(var unknown in unknowns)
            {
                reportString += $"[UNKNOWN] {unknown.Amount} EUR @ {unknown.Date} - {unknown.Description}" + Environment.NewLine;
            }

            return reportString; 
        }
    }
}
