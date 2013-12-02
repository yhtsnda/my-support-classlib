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

        public MappingService(UserInnerService userInnerService, IMappingRepository mappingRepository)
        {
            this.userInnerService = userInnerService;
            this.mappingRepository = mappingRepository;
        }

        public Mapping GetMappingByPassport91Id(long passwordId)
        {
            var spec = mappingRepository.CreateSpecification().Where(o => o.PassportId == passwordId && o.MappingType == MappingType.X91Passport);
            return mappingRepository.FindOne(spec);
        }

        public Mapping GetMapping(string key, MappingType source)
        {
            var spec = mappingRepository.CreateSpecification().Where(o => o.UserKey == key && o.MappingType == source);
            return mappingRepository.FindOne(spec);
        }

        public Mapping GetMappingForPassport91(long userId)
        {
            var spec = mappingRepository.CreateSpecification().Where(o => o.LocalUserId == userId && o.MappingType == MappingType.X91Passport);
            return mappingRepository.FindOne(spec);
        }

        public Mapping CreateMapping(string key, MappingType source, long userId)
        {
            var mapping = GetMapping(key, source);
            if (mapping != null)
                throw new ArgumentException("绑定已经存在");

            var user = userInnerService.GetUserInner(userId);
            if (user == null)
                throw new ArgumentException("用户不存在");

            mapping = new Mapping()
            {
                LocalUserId = userId,
                MappingType = source,
                UserKey = key
            };
            CreateMapping(mapping);
            return mapping;
        }

        public IList<Mapping> GetMappingList(string key)
        {
            var spec = mappingRepository.CreateSpecification().Where(o => o.UserKey == key);
            return mappingRepository.FindAll(spec);
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
            Arguments.That(passport91Id > 0, "passport91Id", "passport91Id错误，必须进行91passport的安全授权");

            var mapping = GetMappingByPassport91Id(passport91Id);
            UserInner userInner = null;

            if (mapping == null)
            {
                userInner = userInnerService.GetUserInner(userName);
                if (userInner == null)
                {
                    userInner = new UserInner()
                    {
                        UserName = userName,
                        Password = password,
                        NickName = nickName,
                        UpgradeStatus = UpgradeStatus.Upgraded,
                        Email = email
                    };

                    userInnerService.CreateUserInner(userInner);
                    userInnerService.OnRegisterSuccess(userInner, platCode, ip, browser, auto, fromUrl, extendField);
                }

                mapping = GetMapping(userName, MappingType.X91Passport);
                if (mapping == null)
                {
                    mapping = new Mapping()
                    {
                        LocalUserId = userInner.UserId,
                        PassportId = passport91Id,
                        MappingType = MappingType.X91Passport,
                        UserKey = userName
                    };
                    CreateMapping(mapping);
                }
                else 
                {
                    //本地映射存在，但未关联passportId，在此做更新  by lcj 2013-11-04
                    mapping.PassportId = passport91Id;
                    mappingRepository.Update(mapping);
                }
            }
            else
            {
                userInner = userInnerService.GetUserInner(mapping.LocalUserId);
                if (userInner == null)
                    throw new Exception(String.Format("数据错误。 mapping 存在但 user {0}不存在", mapping.LocalUserId));

                if (userInner.UpgradeStatus != UpgradeStatus.Upgraded)
                {
                    userInner.UpgradeStatus = UpgradeStatus.Upgraded;
                    userInnerService.UpdateUserInner(userInner);

                    userInnerService.OnUpgradeSuccess(userInner);
                }
            }

            return userInner;
        }

        public Mapping CreateMapping(string userKey, MappingType mappingSource, UserInner innerUser)
        {
            Arguments.NotNull(innerUser, "innerUser");
            var mapping = GetMapping(userKey, mappingSource);
            if (mapping != null)
                throw new AvalonException("该帐号 {0} 的映射已经存在。", userKey);

            mapping = new Mapping()
            {
                LocalUserId = innerUser.UserId,
                MappingType = mappingSource,
                UserKey = userKey
            };
            CreateMapping(mapping);

            return mapping;
        }
    }
}
