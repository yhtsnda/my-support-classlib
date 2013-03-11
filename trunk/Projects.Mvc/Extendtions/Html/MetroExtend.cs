using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace System.Web.Mvc
{
    public static class MetroExtend
    {
        public static MetroMessage Message(this HtmlHelper helper, string title)
        {
            return new MetroMessage().Title(title);
        }

        public static MvcHtmlString FormItemLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return FormItemLabel(html, ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData), ExpressionHelper.GetExpressionText(expression), null);
        }

        public static MvcHtmlString FormItemLabel(this HtmlHelper html, string expression, string labelText)
        {
            return FormItemLabel(html, ModelMetadata.FromStringExpression(expression, html.ViewData), expression, labelText);
        }

        static MvcHtmlString FormItemLabel(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string labelText)
        {
            string str = labelText ?? (metadata.DisplayName ?? (metadata.PropertyName ?? htmlFieldName.Split(new char[] { '.' }).Last<string>()));
            if (string.IsNullOrEmpty(str))
            {
                return MvcHtmlString.Empty;
            }
            TagBuilder tagBuilder = new TagBuilder("label");
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tagBuilder.Attributes.Add("class", "ui-form-item-label");
            tagBuilder.SetInnerText(str);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }


    public class MetroMessage
    {
        string title;
        string detail;
        MessageColor color = MessageColor.Default;
        MetroIcon icon = MetroIcon.Info;

        public MetroMessage Title(string title)
        {
            this.title = title;
            return this;
        }

        public MetroMessage Detail(string detail)
        {
            this.detail = detail;
            return this;
        }

        public MetroMessage Color(MessageColor color)
        {
            this.color = color;
            return this;
        }

        public MetroMessage Icon(MetroIcon icon)
        {
            this.icon = icon;
            return this;
        }

        public MvcHtmlString Render()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<div class=\"ui-message{0}\">", color != MessageColor.Default ? " ui-message-" + color.ToString().ToLower() : "");
            builder.AppendFormat("<span class=\"ui-micon ui-micon-{0}\"></span>", icon.ToString().ToLower());
            builder.Append("<div class=\"ui-message-text\">");
            if (!String.IsNullOrEmpty(title))
                builder.AppendFormat("<em>{0}</em>", title);
            if (!String.IsNullOrEmpty(detail))
                builder.AppendFormat("<p>{0}</p>", detail);
            builder.Append("</div></div>");
            return MvcHtmlString.Create(builder.ToString());
        }
    }

    public enum MetroIcon
    {
        Info,
        Question,
        Error,
        Check,
        Stop
    }

    public enum MessageColor
    {
        Default,
        Red,
        Orange
    }
}
