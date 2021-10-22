using System.Collections.Generic;

namespace Bookkeeper
{
    public interface ITransactionParser
    {
        List<Transaction> ParseExpenses();
    }
}