using System;

namespace Bookkeeper
{
    public class CategoryReporter
    {
        private TransactionProcessor processor;

        public CategoryReporter(TransactionProcessor processor)
        {
            this.processor = processor;
        }

        public string CreateReport()
        {
            string reportString = "IMPORT REPORT" + Environment.NewLine;

            CategorizedResult res = processor.SummizeCategories();
            foreach(var category in res.Categories)
            {
                string categoryName = category.Key;
                decimal amount = category.Value;
                reportString += $"{categoryName}: {amount} EUR" + Environment.NewLine; 
            }

            return reportString; 
        }
    }
}
