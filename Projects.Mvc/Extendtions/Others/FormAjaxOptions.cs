using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc.Ajax
{
    public class FormAjaxOptions : AjaxOptions
    {
        public FormAjaxOptions()
        {
            OnComplete = "$.loading.hide();";
            OnFailure = "$.ajaxSettings.error";
        }
    }
}
