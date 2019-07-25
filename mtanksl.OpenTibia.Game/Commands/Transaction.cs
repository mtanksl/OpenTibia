using OpenTibia.Threading;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class Transaction : IDisposable
    {
        public static Transaction Current
        {
            get
            {
                var current = Scope<Transaction>.Current;

                if (current != null)
                {
                    return current.Value;
                }

                return null;
            }
        }

        private Scope<Transaction> scope;

        public Transaction()
        {
            scope = new Scope<Transaction>(this);
        }

        private List<ITransaction> transactions = new List<ITransaction>();

        public void Add(ITransaction transaction)
        {
            transactions.Add(transaction);
        }

        public bool Complete()
        {
            for (int i = 0; i < transactions.Count; i++)
            {
                if ( !transactions[i].Execute() )
                {
                    for (int j = 0; j < i + 1; j++)
                    {
                        transactions[i - j].Rollback();
                    }

                    return false;
                }
            }

            return true;
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }

    public interface ITransaction
    {
        bool Execute();

        void Rollback();
    }
}