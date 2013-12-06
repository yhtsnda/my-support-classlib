using Avalon.Framework;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class MappingService : IService
    {
        UserInnerService userInnerService;
        IMappingRepository mappingRepository;
        IMappingAppRepository mappingAppRepository;

        public MappingService(UserInnerService userInnerService, 
            IMappingRepository mappingRepository,
            IMappingAppRepository mappingAppRepository)
        {
            this.userInnerService = userInnerService;
            this.mappingRepository = mappingRepository;
            this.mappingAppRepository = mappingAppRepository;
        }
        /// <summary>
        /// 获取用户映射信息
        /// </summary>
        public Mapping GetMapping(string key, int source)
        {
            var spec = mappingRepository.CreateSpecification()
                .Where(o => o.MappingUserKey == key && o.MappingSourceId == source);
            return mappingRepository.FindOne(spec);
        }
        /// <summary>
        /// 创建用户映射信息
        /// </summary>
        public Mapping CreateMapping(string mappingKey, int source, int localUserId, int mappingId = 0)
        {
            var mappingApp = mappingAppRepository.Get(source);
            if (mappingApp == null)
                throw new ArgumentException("映射应用不存在, 请联系管理员添加映射");

            var mapping = GetMapping(mappingKey, source);
            if (mapping != null)
                throw new ArgumentException("绑定已经存在");

            var user = userInnerService.GetUserInner(localUserId);
            if (user == null)
                throw new ArgumentException("用户不存在");

            mapping = new Mapping(mappingId, mappingKey, source, localUserId);
            CreateMapping(mapping);
            return mapping;
        }
        /// <summary>
        /// 创建用户映射信息
        /// </summary>
        public Mapping CreateMapping(string mappingKey, int source, UserInner innerUser, int mappingId = 0)
        {
            var mappingApp = mappingAppRepository.Get(source);
            if (mappingApp == null)
                throw new ArgumentException("映射应用不存在, 请联系管理员添加映射");

            Arguments.NotNull(innerUser, "innerUser");
            var mapping = GetMapping(mappingKey, source);
            if (mapping != null)
                throw new AvalonException("该帐号 {0} 的映射已经存在。", mappingKey);

            mapping = new Mapping(mappingId, mappingKey, source, innerUser.UserId);
            CreateMapping(mapping);

            return mapping;
        }
        /// <summary>
        /// 获取映射键对应的所有映射
        /// </summary>
        public IList<Mapping> GetMappingList(string key)
        {
            var spec = mappingRepository.CreateSpecification().Where(o => o.MappingUserKey == key);
            return mappingRepository.FindAll(spec);
        }
        /// <summary>
        /// 创建新的映射应用
        /// </summary>
        public void CreateMappingApp(MappingApp entry)
        {
            mappingAppRepository.Create(entry);
        }
        /// <summary>
        /// 更新映射应用
        /// </summary>
        public void UpdateMappingApp(MappingApp entry)
        {
            mappingAppRepository.Update(entry);
        }
        /// <summary>
        /// 将映射应用设置为不可用
        /// </summary>
        public void DeleteMappingApp(int id)
        {
            var app = mappingAppRepository.Get(id);
            if (app == null)
                throw new ArgumentException("Mapping App不存在");
            app.SetMappAppStatus(PlatformStatus.NotAvailable);
            mappingAppRepository.Update(app);
        }
        /// <summary>
        /// 获取一个映射应用
        /// </summary>
        public MappingApp GetMappingApp(int id)
        {
            var app = mappingAppRepository.Get(id);
            if (app == null)
                throw new ArgumentException("Mapping App不存在");
            return app;
        }
        /// <summary>
        /// 获取所有的映射应用
        /// </summary>
        public IList<MappingApp> GetMappingAppAll()
        {
            var list = mappingAppRepository.FindAll(mappingAppRepository.CreateSpecification());
            return list;
        }
        void CreateMapping(Mapping mapping)
        {
            mappingRepository.Create(mapping);
        }
        /// <summary>
        /// 确保91passport的映射关系，并返回UserInner
        /// </summary>
        public UserInner EnsureUserMapping(long passport91Id, string userName, string nickName, string password, string email, long platCode, string ip, string browser, bool auto, string fromUrl, string extendField)
        {
            throw new NotImplementedException();
            //Arguments.That(passport91Id > 0, "passport91Id", "passport91Id错误，必须进行91passport的安全授权");

            //var mapping = GetMappingByPassport91Id(passport91Id);
            //UserInner userInner = null;

            //if (mapping == null)
            //{
            //    userInner = userInnerService.GetUserInner(userName);
            //    if (userInner == null)
            //    {
            //        userInner = new UserInner()
            //        {
            //            UserName = userName,
            //            Password = password,
            //            NickName = nickName,
            //            UpgradeStatus = UpgradeStatus.Upgraded,
            //            Email = email
            //        };

            //        userInnerService.CreateUserInner(userInner);
            //        userInnerService.OnRegisterSuccess(userInner, platCode, ip, browser, auto, fromUrl, extendField);
            //    }

            //    mapping = GetMapping(userName, MappingType.X91Passport);
            //    if (mapping == null)
            //    {
            //        mapping = new Mapping()
            //        {
            //            LocalUserId = userInner.UserId,
            //            PassportId = passport91Id,
            //            MappingType = MappingType.X91Passport,
            //            UserKey = userName
            //        };
            //        CreateMapping(mapping);
            //    }
            //    else
            //    {
            //        //本地映射存在，但未关联passportId，在此做更新  by lcj 2013-11-04
            //        mapping.PassportId = passport91Id;
            //        mappingRepository.Update(mapping);
            //    }
            //}
            //else
            //{
            //    userInner = userInnerService.GetUserInner(mapping.LocalUserId);
            //    if (userInner == null)
            //        throw new Exception(String.Format("数据错误。 mapping 存在但 user {0}不存在", mapping.LocalUserId));

            //    if (userInner.UpgradeStatus != UpgradeStatus.Upgraded)
            //    {
            //        userInner.UpgradeStatus = UpgradeStatus.Upgraded;
            //        userInnerService.UpdateUserInner(userInner);

            //        userInnerService.OnUpgradeSuccess(userInner);
            //    }
            //}

            //return userInner;
        }
    }
}
