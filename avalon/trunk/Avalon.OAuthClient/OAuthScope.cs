using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    /// <summary>
    /// 提供一个特定范围执行给定 AccessToken 的方法
    /// </summary>
    public class OAuthScope : IDisposable
    {
        const string OAuthScopeKey = "_oauthscope_";

        public OAuthScope(string accessToken)
        {
            AccessToken = accessToken;
            PushScope(this);
        }

        static void PushScope(OAuthScope scope)
        {
            Stack<OAuthScope> scopeStack = (Stack<OAuthScope>)Workbench.Current.Items[OAuthScopeKey];
            if (scopeStack == null)
            {
                scopeStack = new Stack<OAuthScope>();
                Workbench.Current.Items[OAuthScopeKey] = scopeStack;
            }
            scopeStack.Push(scope);
        }

        static void PopScope(OAuthScope scope)
        {
            Stack<OAuthScope> scopeStack = (Stack<OAuthScope>)Workbench.Current.Items[OAuthScopeKey];
            if (scopeStack != null)
                scopeStack.Pop();
        }

        internal static OAuthScope PeekOAuthScope()
        {
            Stack<OAuthScope> scopeStack = (Stack<OAuthScope>)Workbench.Current.Items[OAuthScopeKey];
            if (scopeStack != null && scopeStack.Count > 0)
                return scopeStack.Peek();

            return null;
        }

        public string AccessToken
        {
            get;
            private set;
        }

        void IDisposable.Dispose()
        {
            PopScope(this);
        }
    }
}
