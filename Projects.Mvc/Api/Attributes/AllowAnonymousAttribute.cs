namespace System.Web.Mvc
{
    /// <summary>
    /// 允许匿名访问操作或控制器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AllowAnonymousAttribute: Attribute
    {
        
    }
}
