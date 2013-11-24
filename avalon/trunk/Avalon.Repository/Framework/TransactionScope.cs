using Avalon.Profiler;
using Avalon.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 表示开启一个范围事务。如果当前已经存于某范围事务，则向上进行合并。
    /// </summary>
    public class TransactionScope : IDisposable
    {
        const string TranscationKey = "__TransactionScopeKey__";
        bool isComplete = false;

        public TransactionScope()
        {
            var data = (ScopeData)Workbench.Current.Items[TranscationKey];
            if (data == null)
            {
                data = new ScopeData();
                Workbench.Current.Items[TranscationKey] = data;
            }
            data.Stack.Push(this);
        }

        /// <summary>
        /// 将事务标志为完成
        /// </summary>
        public void Complate()
        {
            isComplete = true;
        }

        void IDisposable.Dispose()
        {
            ProfilerContext.Current.Trace("platform", "end transaction session");

            var data = GetAndCheckScopeData();
            var scope = data.Stack.Pop();

            if (data.Stack.Count == 0)
            {
                foreach (var tran in data.Transactions.Values)
                {
                    if (scope.isComplete)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        try { tran.Rollback(); }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                data.Transactions.Clear();
            }
        }

        /// <summary>
        /// 添加一个事务实例，以便于在范围事务完成时进行处理。
        /// </summary>
        /// <param name="tran"></param>
        public static void Push(ITransaction tran, object linkObject)
        {
            ProfilerContext.Current.Trace("platform", "begin transaction session");
            var data = GetAndCheckScopeData();
            data.Transactions.Add(linkObject, tran);
        }

        /// <summary>
        /// 判断给定的对象是否与事务参生关联。
        /// </summary>
        /// <param name="linkObject"></param>
        /// <returns></returns>
        public static bool IsLinkWidhTranscation(object linkObject)
        {
            var data = GetAndCheckScopeData();
            return data.Transactions.ContainsKey(linkObject);
        }

        /// <summary>
        /// 判断当前是否为范围事务上下文
        /// </summary>
        public static bool IsScope
        {
            get
            {
                var data = GetScopeData();
                return data != null && data.Stack.Count > 0;
            }
        }

        static ScopeData GetScopeData()
        {
            return (ScopeData)Workbench.Current.Items[TranscationKey];
        }

        static ScopeData GetAndCheckScopeData()
        {
            var data = GetScopeData();
            if (data == null || data.Stack.Count == 0)
                throw new InvalidOperationException("当前非范围事务上下文，无法执行操作");

            return data;
        }

        class ScopeData
        {
            public Stack<TransactionScope> Stack = new Stack<TransactionScope>();

            public Dictionary<object, ITransaction> Transactions = new Dictionary<object, ITransaction>();
        }
    }

    public interface ITransaction
    {
        void Commit();

        void Rollback();
    }
}
