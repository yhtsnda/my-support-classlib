using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool.Util;

namespace Projects.OAuthClient
{
    /// <summary>
    /// 提供一个特定范围执行给定 AccessToken 的方法
    /// </summary>
    public class OAuthScope : IDisposable
    {
        private const string OAUTH_SCOPE_KEY = "_oauthscope_";

        public string AccessToken { get; private set; }

        /// <summary>
        /// 提供一个特定范围执行给定 AccessToken 的方法
        /// </summary>
        public OAuthScope(string accessToken)
        {
            this.AccessToken = accessToken;
            Push(this);
        }

        private static void Push(OAuthScope scope)
        {
            Stack<OAuthScope> scopeStack = (Stack<OAuthScope>)Workbench.Current.Items[OAUTH_SCOPE_KEY];
            if (scopeStack == null)
            {
                scopeStack = new Stack<OAuthScope>();
                Workbench.Current.Items[OAUTH_SCOPE_KEY] = scopeStack;
            }
            scopeStack.Push(scope);
        }

        private static void Pop(OAuthScope scope)
        {
            Stack<OAuthScope> scopeStack = (Stack<OAuthScope>)Workbench.Current.Items[OAUTH_SCOPE_KEY];
            if (scopeStack != null)
                scopeStack.Pop();
        }

        internal static OAuthScope Peek()
        {
            Stack<OAuthScope> scopeStack = (Stack<OAuthScope>)Workbench.Current.Items[OAUTH_SCOPE_KEY];
            if (scopeStack != null && scopeStack.Count > 0)
                scopeStack.Peek();
            return null;
        }

        public void Dispose()
        {
            Pop(this);
        }
    }
}
