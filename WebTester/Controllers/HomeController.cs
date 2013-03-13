using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Projects.Framework;
using Projects.Demo;

namespace WebTester.Controllers
{
    public class HomeController : Controller
    {
        private MemberService memberService;

        public HomeController(MemberService memberService)
        {
            this.memberService = memberService;
        }

        public ActionResult Create()
        {
            Member member = new Member(Guid.NewGuid().ToString(),"Test@91up.com",Guid.NewGuid().ToString());
            var result = memberService.CreateMember(member);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get(int id)
        {
            var result = memberService.GetMemberById(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
