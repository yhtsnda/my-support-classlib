using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc.Html;
using System.Linq.Expressions;

namespace System.Web.Mvc
{
    public static class KnockoutBindExtend
    {
        public static MvcForm BeginFormBind(this HtmlHelper htmlHelper, string dataBind, object htmlAttributes = null)
        {
            TagBuilder builder = new TagBuilder("form");
            builder.MergeAttribute("data-bind", dataBind);
            if (htmlAttributes != null)
            {
                builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }

            bool flag = htmlHelper.ViewContext.ClientValidationEnabled && 
                !htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled;
            htmlHelper.ViewContext.Writer.Write(builder.ToString(TagRenderMode.StartTag));
            MvcForm form = new MvcForm(htmlHelper.ViewContext);
            if (flag)
            {
                htmlHelper.ViewContext.FormContext.FormId = builder.Attributes["id"];
            }
            return form;
        }

        #region TextBox
        public static MvcHtmlString TextBoxBind(this HtmlHelper htmlHelper, string name)
        {
            return TextBoxBind(htmlHelper, name, null);
        }

        public static MvcHtmlString TextBoxBind(this HtmlHelper htmlHelper, string name, object value)
        {
            return TextBoxBind(htmlHelper, name, null, null);
        }

        public static MvcHtmlString TextBoxBind(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", name);
            return InputExtensions.TextBox(htmlHelper, name, value, bind);
        }

        public static MvcHtmlString TextBoxBind(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", name);
            return InputExtensions.TextBox(htmlHelper, name, value, bind);
        }

        public static MvcHtmlString TextBoxBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return TextBoxBindFor(htmlHelper, expression, null);
        }

        public static MvcHtmlString TextBoxBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", ExpressionHelper.GetExpressionText(expression));
            return InputExtensions.TextBoxFor(htmlHelper, expression, bind);
        }

        public static MvcHtmlString TextBoxBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", ExpressionHelper.GetExpressionText(expression));
            return InputExtensions.TextBoxFor(htmlHelper, expression, bind);
        }

        #endregion

        #region Password
        public static MvcHtmlString PasswordBind(this HtmlHelper htmlHelper, string name)
        {
            return PasswordBind(htmlHelper, name, null);
        }

        public static MvcHtmlString PasswordBind(this HtmlHelper htmlHelper, string name, object value)
        {
            return PasswordBind(htmlHelper, name, null, null);
        }

        public static MvcHtmlString PasswordBind(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", name);
            return InputExtensions.Password(htmlHelper, name, value, bind);
        }

        public static MvcHtmlString PasswordBind(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", name);
            return InputExtensions.Password(htmlHelper, name, value, bind);
        }

        public static MvcHtmlString PasswordBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return PasswordBindFor(htmlHelper, expression, null);
        }

        public static MvcHtmlString PasswordBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", ExpressionHelper.GetExpressionText(expression));
            return InputExtensions.PasswordFor(htmlHelper, expression, bind);
        }

        public static MvcHtmlString PasswordBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", ExpressionHelper.GetExpressionText(expression));
            return InputExtensions.PasswordFor(htmlHelper, expression, bind);
        }

        #endregion

        #region Hidden
        public static MvcHtmlString HiddenBind(this HtmlHelper htmlHelper, string name)
        {
            return HiddenBind(htmlHelper, name, null, (IDictionary<string, object>)null);
        }
        public static MvcHtmlString HiddenBind(this HtmlHelper htmlHelper, string name, object value)
        {
            return HiddenBind(htmlHelper, name, value, (IDictionary<string, object>)null);
        }
        public static MvcHtmlString HiddenBind(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", name);
            return InputExtensions.Hidden(htmlHelper, name, value, bind);
        }
        public static MvcHtmlString HiddenBind(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", name);
            return InputExtensions.Hidden(htmlHelper, name, value, bind);
        }

        public static MvcHtmlString HiddenBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return HiddenBindFor(htmlHelper, expression, (IDictionary<string, object>)null);
        }
        public static MvcHtmlString HiddenBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", ExpressionHelper.GetExpressionText(expression));
            return InputExtensions.HiddenFor(htmlHelper, expression, bind);
        }
        public static MvcHtmlString HiddenBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", ExpressionHelper.GetExpressionText(expression));
            return InputExtensions.HiddenFor(htmlHelper, expression, bind);
        }
        #endregion

        #region DropDownList

        public static MvcHtmlString DropDownListBind(this HtmlHelper htmlHelper, string name)
        {
            return DropDownListBind(htmlHelper, name, null, null, ((IDictionary<string, object>)null));
        }

        public static MvcHtmlString DropDownListBind(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
        {
            return DropDownListBind(htmlHelper, name, selectList, null, ((IDictionary<string, object>)null));
        }

        public static MvcHtmlString DropDownListBind(this HtmlHelper htmlHelper, string name, string optionLabel)
        {
            return DropDownListBind(htmlHelper, name, null, optionLabel, ((IDictionary<string, object>)null));
        }

        public static MvcHtmlString DropDownListBind(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return DropDownListBind(htmlHelper, name, selectList, null, htmlAttributes);
        }

        public static MvcHtmlString DropDownListBind(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            return DropDownListBind(htmlHelper, name, selectList, null, htmlAttributes);
        }

        public static MvcHtmlString DropDownListBind(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel)
        {
            return DropDownListBind(htmlHelper, name, selectList, optionLabel, ((IDictionary<string, object>)null));
        }

        public static MvcHtmlString DropDownListBind(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", name);
            return SelectExtensions.DropDownList(htmlHelper, name, selectList, optionLabel, bind);
        }

        public static MvcHtmlString DropDownListBind(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", name);
            return SelectExtensions.DropDownList(htmlHelper, name, selectList, optionLabel, bind);
        }

        public static MvcHtmlString DropDownListBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            return DropDownListBindFor(htmlHelper, expression, selectList, null, ((IDictionary<string, object>)null));
        }
        public static MvcHtmlString DropDownListBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return DropDownListBindFor(htmlHelper, expression, selectList, null, htmlAttributes);
        }
        public static MvcHtmlString DropDownListBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            return DropDownListBindFor(htmlHelper, expression, selectList, null, htmlAttributes);
        }
        public static MvcHtmlString DropDownListBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel)
        {
            return DropDownListBindFor(htmlHelper, expression, selectList, optionLabel, ((IDictionary<string, object>)null));
        }
        public static MvcHtmlString DropDownListBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", ExpressionHelper.GetExpressionText(expression));
            return SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, optionLabel, bind);
        }
        public static MvcHtmlString DropDownListBindFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            var bind = new KnockoutBind(null, htmlAttributes);
            bind.AddBind("value", ExpressionHelper.GetExpressionText(expression));
            return SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, optionLabel, bind);
        }
        #endregion
    }
}
