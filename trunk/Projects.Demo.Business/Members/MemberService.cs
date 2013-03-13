using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool;
using Projects.Framework;

namespace Projects.Demo
{
    public class MemberService : IService
    {
        private IMemberRelationRepository memberRelationRepository;
        private IMemberRepository memberRepository;

        public MemberService(IMemberRepository memberRepository, IMemberRelationRepository memberRelationRepository)
        {
            this.memberRepository = memberRepository;
            this.memberRelationRepository = memberRelationRepository;
        }

        public virtual Member CreateMember(Member member)
        {
            memberRepository.Create(member);
            return member;
        }

        public virtual void UpdateMember(Member member)
        {
            memberRepository.Update(member);
        }

        public virtual void DeleteMember(int id)
        {
            var member = memberRepository.Get(id);
            if (member != null)
                memberRepository.Delete(member);
        }

        public virtual Member GetMemberById(int id)
        {
            return memberRepository.Get(id);
        }

        public virtual PagingResult<Member> GetMemberPaging(MemberSearchFilter filter)
        {
            var result = memberRepository.GetPagingMemberList(filter);
            return result;
        }

        public virtual IList<Member> GetMemberByEmail(string email)
        {
            var spec = memberRepository.CreateSpecification().Where(o => o.EMail.Contains(email));
            return memberRepository.FindAll(spec);
        }
    }
}
