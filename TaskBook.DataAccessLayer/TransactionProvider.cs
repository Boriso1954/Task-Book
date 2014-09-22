using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace TaskBook.DataAccessLayer
{
    /// <summary>
    /// Customizes the TransactionScope object
    /// </summary>
    public sealed class TransactionProvider
    {
        /// <summary>
        /// Returns an instance of the TransactionScope object with custom options
        /// </summary>
        /// <returns>Reference to the TransactionScope object</returns>
        public static TransactionScope GetTransactionScope()
         { 
            var transactionOptions = new TransactionOptions 
            {
                IsolationLevel = IsolationLevel.ReadCommitted, // The default value is Serializable, this could often result in a transaction deadlock 
                Timeout = TimeSpan.FromMinutes(5) //Assume 5 min is the timeout time
            }; 
            return new TransactionScope(TransactionScopeOption.Required, transactionOptions); 
        } 
    }
}
