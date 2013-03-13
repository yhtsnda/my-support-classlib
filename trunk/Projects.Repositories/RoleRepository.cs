using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool;
using Projects.Framework;
using Projects.Purviews;
using Projects.Accesses;

namespace Projects.Repositories
{
    public class RoleRepository : AbstractSimpleRepository<Role, Role>, IRoleRepository
    {
        /// <summary>
        /// 查询指定实例下的默认角色
        /// </summary>
        /// <param name="instanceKey"></param>
        /// <returns></returns>
        public Role QueryDefaultRole(string instanceKey)
        {
            return SqlMapperManager.Instance[base.MappingKey].QueryForObject<Role>("Query_Default_Role", 
                new Role { InstanceKey = instanceKey });
        }
    }
}
