using Projects.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    public class MappingService : IService
    {
        private IMappingRepository mappingRepository;

        public MappingService(IMappingRepository mappingRepository)
        {
            this.mappingRepository = mappingRepository;
        }

        /// <summary>
        /// 创建映射
        /// </summary>
        /// <param name="mapping">映射实体</param>
        /// <returns>如果映射已经存在,则返回已经存在的映射(用户ID和映射类型);如果不存在则创建后返回</returns>
        public Mapping CreateMapping(Mapping mapping)
        {
            var mp = GetMappingByLocalUserId(mapping.LocalUserId, mapping.MappingType);
            if (mp != null)
                return mp;
            mappingRepository.Create(mapping);
            return mapping;
        }

        /// <summary>
        /// 根据映射键和类型获取映射对象
        /// </summary>
        /// <param name="mappingKey">映射的第三方键</param>
        /// <param name="type">映射类型</param>
        /// <returns>映射对象</returns>
        public Mapping GetMappingByMappingKey(string mappingKey, MappingType type)
        {
            var spec = mappingRepository.CreateSpecification()
                .Where(o => o.MappingKey == mappingKey && o.MappingType == type);
            return mappingRepository.FindOne(spec);
        }

        /// <summary>
        /// 获取映射对象
        /// </summary>
        /// <param name="id">映射ID</param>
        /// <returns>映射对象</returns>
        public Mapping GetMapping(int id)
        {
            return mappingRepository.Get(id);
        }

        /// <summary>
        /// 根据用户ID获取该用户所有的映射信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户所有的映射信息</returns>
        public IList<Mapping> GetMappingByLocalUserId(int userId)
        {
            var spec = mappingRepository.CreateSpecification()
                .Where(o => o.LocalUserId == userId);
            return mappingRepository.FindAll(spec);
        }

        /// <summary>
        /// 根据用户ID和映射类型获取用户映射信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="type">映射类型</param>
        /// <returns>映射信息</returns>
        public Mapping GetMappingByLocalUserId(int userId, MappingType type)
        {
            var spec = mappingRepository.CreateSpecification()
                .Where(o => o.LocalUserId == userId && o.MappingType == type);
            return mappingRepository.FindOne(spec);
        }
    }
}
