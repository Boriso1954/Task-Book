using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace TaskBook.DataAccessLayer
{
    public class TransactionProvider
    {
        public static TransactionScope GetTransactionScope()
         { 
            var transactionOptions = new TransactionOptions 
            {
                IsolationLevel = IsolationLevel.ReadCommitted, // The default value is Serializable, this could often result in a transaction deadlock 
                Timeout = TimeSpan.FromMinutes(5) //assume 5 min is the timeout time
            }; 
            return new TransactionScope(TransactionScopeOption.Required, transactionOptions); 
        } 
    }
}
