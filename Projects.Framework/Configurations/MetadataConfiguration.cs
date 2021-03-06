﻿using Projects.Tool.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Projects.Framework.Configurations
{
    internal class MetadataConfiguration
    {
        IList<Assembly> assemblies;
        Dictionary<Type, ClassDefineMetadata> metadatas = new Dictionary<Type, ClassDefineMetadata>();

        public MetadataConfiguration(IList<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        public void Load()
        {
            foreach (var assembly in assemblies)
            {
                RegisterDefineMetadata(assembly);
            }
        }

        /// <summary>
        /// 注册元数据定义
        /// </summary>
        /// <param name="assembly"></param>
        public void RegisterDefineMetadata(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            Type[] types = assembly.GetExportedTypes();
            foreach (Type type in types)
            {
                var v = type.GetInterface(typeof(ICacheMetadataProvider).FullName);
                if (v != null)
                {
                    var active = FastActivator.Create(type);
                    var metadata = ((ICacheMetadataProvider)active).GetCacheMetadata();

                    if (metadatas.ContainsKey(metadata.EntityType))
                        throw new PlatformException("重复的类型定义。\r\nEntityType:{0}, DefineType:{1}", metadata.EntityType.FullName, type.FullName);

                    //验证方法virtual
                    EntityUtil.CheckVirtualType(metadata.EntityType);

                    metadatas.Add(metadata.EntityType, metadata);
                }
            }
        }

        public ClassDefineMetadata GetDefineMetadata(Type entityType)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");

            var type = entityType;
            do
            {
                var define = metadatas.TryGetValue(type);
                if (define != null)
                    return define;
                type = type.BaseType;
            }
            while (type != null);
            return null;
        }
    }
}
