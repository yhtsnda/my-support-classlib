using System.Web.Mvc;

namespace Projects.OpenApi.Areas.V1
{
    public class V1AreaRegistration : AreaRegistration
    {
        private static string[] namespaces = new string[1] { "Projects.OpenApi.Areas.V1.Controllers" };

        public override string AreaName
        {
            get
            {
                return "V1";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "V1_default",
                "V1/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "V1_user",
                "V1/{app}/{terminal}/{source}/{controller}/{action}",
                null,
                new { app = @"[0-9]{4}", terminal = @"[0-9]{4}*", source = @"[0-9]{4}" }
            );
        }
    }
}
