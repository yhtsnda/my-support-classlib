using System;
using System.Collections;
using System.Collections.Generic;

using Projects.Tool;
using Projects.Framework;
using Projects.Purviews;
using Projects.Accesses;

namespace Projects.Repositories
{
    public class AccessRepository : AbstractSimpleRepository<Access, Access>, IAccessRepository
    {
        /// <summary>
        /// 创建用户存取权限
        /// </summary>
        public override ActionResult<ResultKey, Access> Create(Access entity)
        {
            ActionResult<ResultKey, Access> result = new ActionResult<ResultKey, Access>();
            if (entity.AutoCode == 0)
            {
                result = base.Create(entity);
            }
            else
            {
                result.ExtraData = entity;
                result.Result = ResultKey.OK;
            }
            
            if (result.Result == ResultKey.OK)
            {
                Hashtable table = new Hashtable();
                foreach(Role role in entity.Roles)
                {
                    table.Add("AccessKey", result.ExtraData.AutoCode);
                    table.Add("RoleKey", role.Key);
                    SqlMapperManager.Instance[base.MappingKey].Insert("Create_User_Access", table);

                    table.Clear();
                }
            }
            return result;
        }

        /// <summary>
        /// 删除用户已经分配的角色
        /// </summary>
        public ResultKey DeleteAccessRole(int accessKey)
        {
            var result =(int)SqlMapperManager.Instance[base.MappingKey].Delete("Delete_AccessRole", accessKey);
            return result >= 0 ? ResultKey.OK : ResultKey.Failure;
        }
    }
}
