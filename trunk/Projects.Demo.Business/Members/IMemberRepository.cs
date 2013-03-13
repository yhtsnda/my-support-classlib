using Projects.Tool;
using Projects.Framework;

namespace Projects.Demo
{
    public interface IMemberRepository : IShardRepository<Member>
    {
        PagingResult<Member> GetPagingMemberList(MemberSearchFilter filter);
    }
}
