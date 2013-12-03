using Avalon.Framework;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class PlatformService : IService
    {
        IPlatformRepository platformRepository;
        CacheDomain<Platform, string> codeCache;

        public PlatformService(IPlatformRepository platformRepository)
        {
            this.platformRepository = platformRepository;
            codeCache = CacheDomain.CreateSingleKey<Platform, string>(o => o.Code, 
                GetPlatformInner, null, 
                "plat_code",
                "plat_code:{0}");
        }
        /// <summary>
        /// 创建平台
        /// </summary>
        public void CreatePlatform(Platform entry)
        {
            Arguments.NotNull<Platform>(entry, "entry");
            platformRepository.Create(entry);
        }
        /// <summary>
        /// 更新平台
        /// </summary>
        public void ModifyPlatform(Platform entry)
        {
            Arguments.NotNull<Platform>(entry, "entry");
            platformRepository.Update(entry);
            codeCache.RemoveCache(entry.Code);
        }
        /// <summary>
        /// 删除平台
        /// </summary>
        public void DeletePlatform(int id)
        {
            var plat = GetPlatform(id);
            if (plat == null || plat.Status != PlatformStatus.InUse)
                throw new AvalonException("应用不存在或未处于使用状态");
            plat.Status = PlatformStatus.NotAvailable;
            platformRepository.Update(plat);
            codeCache.RemoveCache(plat.Code);
        }
        /// <summary>
        /// 将平台设置为测试状态
        /// </summary>
        public void TestPlatform(int id)
        {
            var plat = GetPlatform(id);
            if (plat == null || plat.Status != PlatformStatus.OnTesting)
                throw new AvalonException("应用不存在或未处于使用状态");
            plat.Status = PlatformStatus.NotAvailable;
            platformRepository.Update(plat);
            codeCache.RemoveCache(plat.Code);
        }
        /// <summary>
        /// 获取平台信息
        /// </summary>
        public Platform GetPlatform(int id)
        {
            return platformRepository.Get(id);
        }
        /// <summary>
        /// 根据平台代码获取平台信息
        /// </summary>
        public Platform GetPlatform(string code)
        {
            return codeCache.GetItem(code);
        }
        /// <summary>
        /// 获取平台列表
        /// </summary>
        public IList<Platform> GetPlatformList(IEnumerable<int> ids)
        {
            return platformRepository.GetList(ids);
        }
        /// <summary>
        /// 获取所有平台信息
        /// </summary>
        public IList<Platform> GetPlatformList()
        {
            return platformRepository.FindAll(platformRepository.CreateSpecification());
        }

        Platform GetPlatformInner(string code)
        {
            var spec = platformRepository.CreateSpecification().Where(o=>o.Code == code);
            return platformRepository.FindOne(spec);
        }
    }
}
