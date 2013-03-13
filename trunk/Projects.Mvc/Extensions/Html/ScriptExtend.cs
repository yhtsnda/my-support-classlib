namespace Projects.Framework.Web
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    using Projects.Tool.Util;

    public class ScriptExtend
    {
        private static readonly string scriptManagerKey = "___ScriptManager___";

        private IList<string> scriptStrings = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        protected ScriptExtend()
        {
        }

        /// <summary>
        /// 注册脚本代码，注意，请以分号;结尾
        /// </summary>
        /// <param name="javascript"></param>
        /// <returns></returns>
        public ScriptExtend Script(string javascript)
        {
            this.scriptStrings.Add(javascript);
            return this;
        }

        /// <summary>
        /// 注册脚本代码，设置值和对象，转换成javascript代码为：var {key} = {value};
        /// value内部会自动进行json序列化。
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="variableValue"></param>
        /// <returns></returns>
        public ScriptExtend Script(string variableName, object variableValue)
        {
            this.scriptStrings.Add(string.Format("var {0} = {1};", variableName, JsonConverter.ToJson(variableValue)));
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Render()
        {
            if (this.scriptStrings.Count == 0)
                return string.Empty;

            var scriptBuilder = new StringBuilder();
            scriptBuilder.Append("<script type=\"text/javascript\">")
                .Append(Environment.NewLine);
            foreach (var script in this.scriptStrings)
            {
                scriptBuilder.Append(script).Append(Environment.NewLine);
            }
            scriptBuilder.Append("</script>");
            return scriptBuilder.ToString();
        }

        /// <summary>
        /// 获取ScriptManager实例
        /// </summary>
        public static ScriptExtend Instance
        {
            get
            {
                var httpContext = HttpContext.Current;
                var scriptmanager = httpContext.Items[scriptManagerKey] as ScriptExtend;
                if (scriptmanager == null)
                {
                    scriptmanager = new ScriptExtend();
                    httpContext.Items[scriptManagerKey] = scriptmanager;
                }
                return scriptmanager;
            }
        }

        /// <summary>
        /// 注册脚本代码，注意，请以分号;结尾
        /// </summary>
        /// <param name="javascript"></param>
        /// <returns></returns>
        public static ScriptExtend RegisterScript(string javascript)
        {
            ScriptExtend.Instance.Script(javascript);
            return ScriptExtend.Instance;
        }

        /// <summary>
        /// 注册脚本代码，设置值和对象，转换成javascript代码为：var {variableName} = {variableValue};
        /// value内部会自动进行json序列化。
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="variableValue"></param>
        /// <returns></returns>
        public static ScriptExtend RegisterScript(string variableName, object variableValue)
        {
            ScriptExtend.Instance.Script(variableName, variableValue);
            return ScriptExtend.Instance;
        }
    }
}

namespace System.Web.Mvc
{
    using Projects.Framework.Web;

    public static class ScriptExtension
    {
        public static MvcHtmlString RenderScript(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(ScriptExtend.Instance.Render());
        }
    }
}
