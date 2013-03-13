using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Projects.Demo;
using WebTester.Filters;
using WebTester.Controllers;

namespace WebTester.Areas.Api.Controllers
{
    public class MemberController : OpenApiBaseController
    {
        private MemberService memberService;

        public MemberController(MemberService memberService)
        {
            this.memberService = memberService;
        }

        //[WebUrl]/member/get?accessToken=ba120e045b3243d78a100860f593ee8c&memberId=2
        [OAuthAuthorize, ActionName("get")]
        public object GetMemberInfo(int memberId)
        {
            var member = memberService.GetMemberById(memberId);
            return new
            {
                Id =  member.MemberId,
                Name = member.RealName
            };
        }
    }
}
