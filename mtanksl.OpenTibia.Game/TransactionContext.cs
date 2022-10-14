using OpenTibia.Threading;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class TransactionContext : IDisposable
    {
        private Scope<TransactionContext> scope;

        public TransactionContext()
        {
            scope = new Scope<TransactionContext>(this);
        }

        private List<Action> actions;

        public void Add(Action action)
        {
            if (actions == null)
            {
                actions = new List<Action>();
            }

            actions.Add(action);
        }

        public void Commit()
        {
            if (actions != null)
            {
                foreach (var action in actions)
                {
                    action();
                }

                actions.Clear();
            }
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}