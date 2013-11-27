using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Framework;
using Avalon.Utility;

namespace Avalon.Purviews
{
    public class ModelServices : IService
    {
        private IModelRepository modelRepository;
        private IAccessRepository accessRepository;
        private CacheDomain<ModelLookup, string> modelCache;

        public ModelServices(IModelRepository modelRepository, IAccessRepository accessRepository)
        {
            this.modelRepository = modelRepository;
            this.accessRepository = accessRepository;

            modelCache = CacheDomain.CreateSingleKey<ModelLookup, string>(
                        o => o.InstanceKey,
                        GetModelLookupInner,
                        null,
                        cacheName: "menu",
                        cacheKeyFormat: "menu:{0}",
                        secondesToLive: 600);
        }

        /// <summary>
        /// 获取指定的模块
        /// </summary>
        public Model GetModel(string instanceKey, int key)
        {
            var lookup = modelCache.GetItem(instanceKey);
            return lookup.GetOrDefault(o => o.ModelDictionary.TryGetValue(key));
        }

        /// <summary>
        /// 根据模块管理Action查询
        /// </summary>
        /// <param name="instanchKey"></param>
        /// <param name="actionKey"></param>
        /// <returns></returns>
        public Model GetModelByAction(string instanceKey, string actionKey)
        {
            var lookup = modelCache.GetItem(instanceKey);
            return lookup.ModelDictionary.Values.Where(o => o.ActionKey == actionKey).FirstOrDefault();
        }

        /// <summary>
        /// 根据父模块ID获取所有子模块信息
        /// </summary>
        /// <param name="instanceKey"></param>
        /// <param name="parentKey"></param>
        /// <returns></returns>
        public IList<Model> GetModelListByParentKey(string instanceKey, int parentKey)
        {
            var spec = modelRepository.CreateSpecification()
                .Where(o=> o.InstanceKey == instanceKey && o.ParentKey == parentKey);
            return modelRepository.FindAll(spec);
        }

        /// <summary>
        /// 批量获取模块
        /// </summary>
        /// <param name="instanceKey"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IList<Model> GetModelList(string instanceKey, IEnumerable<int> ids)
        {
            var lookup = modelCache.GetItem(instanceKey);
            return ids.Select(key => lookup.GetOrDefault(o => o.ModelDictionary.TryGetValue(key)))
                .Where(o => o != null).ToList();
        }

        /// <summary>
        /// 获取根模块
        /// </summary>
        /// <param name="instanceKey"></param>
        /// <returns></returns>
        public Model GetModelRoot(string instanceKey)
        {
            var lookup = modelCache.GetItem(instanceKey);
            return lookup.GetOrDefault(o => o.Root);
        }

        /// <summary>
        /// 获取用户可使用的模块列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="instanceKey"></param>
        /// <returns></returns>
        public Model GetModelListForUser(int userId, string instanceKey)
        {
            //获取用户的信息
            Access access = accessRepository.FindOne(new Access { UserId = userId, InstanceKey = instanceKey });
            if (access == null)
                return null;

            Model root = ModelServices.GetModelRoot(instanceKey);
            if (root == null)
                return root;

            Model userRoot = root.Clone();
            ProcessUserModel(root, userRoot, userId, instanceKey);
            return userRoot;
        }

        /// <summary>
        /// 获取从一级目录到当前目录下的所有模块列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IList<Model> GetModelListFromL1(Model model)
        {
            IList<Model> models = new List<Model>();
            if (model.ParentKey == -1)
                return models;

            models.Insert(0, model);

            Model parentModel = GetModel(model.InstanceKey, model.ParentKey);
            while (parentModel != null && parentModel.ParentKey != -1 &&
                GetModel(model.InstanceKey, parentModel.ParentKey) != null)
            {
                models.Insert(0, parentModel);
                parentModel = GetModel(model.InstanceKey, parentModel.ParentKey);
            }

            return models;
        }

        /// <summary>
        /// 保存模块信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultKey SaveModel(Model model)
        {
            //创建一个新的模块
            if (model.Key == 0)
            {
                if (!String.IsNullOrEmpty(model.ActionKey))
                {
                    bool actionExists = ActionServices.CheckAction(model.InstanceKey, model.ActionKey);
                    if (actionExists)
                    {
                        var result = modelRepository.Create(model).Result;
                        if (result == ResultKey.OK)
                            modelCache.RemoveCache(model.InstanceKey);
                        return result;
                    }
                    else
                    {
                        return ResultKey.Failure;
                    }
                }
                else
                {
                    var result = modelRepository.Create(model).Result;
                    if (result == ResultKey.OK)
                        modelCache.RemoveCache(model.InstanceKey);
                    return result;
                }
            }
            //更新一个模块信息
            else
            {
                var result = modelRepository.Update(model);
                if (result == ResultKey.OK)
                    modelCache.RemoveCache(model.InstanceKey);
                return result;
            }
        }

        /// <summary>
        /// 删除一个模块
        /// </summary>
        /// <param name="model"></param>
        public ResultKey DeleteModel(Model model)
        {
            model = GetModel(model.InstanceKey, model.Key);
            if (model == null)
            {
                return ResultKey.Failure;
            }

            RecursiveDeleteModel(model);
            modelCache.RemoveCache(model.InstanceKey);

            return ResultKey.OK;
        }

        /// <summary>
        /// 循环删除所有模块
        /// </summary>
        /// <param name="model"></param>
        private void RecursiveDeleteModel(Model model)
        {
            foreach (var child in model.Childs)
            {
                if (child.Childs.Count > 0)
                    RecursiveDeleteModel(child);
                modelRepository.Delete(child);
            }
            modelRepository.Delete(model);
        }

        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <param name="instanceKey"></param>
        /// <returns></returns>
        private IList<Model> GetModelList(string instanceKey)
        {
            return modelRepository.FindAll(new Model{InstanceKey = instanceKey});
        }

        /// <summary>
        /// 构建模块树
        /// </summary>
        /// <param name="instanceKey"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        private ModelLookup CreateModelTree(string instanceKey, IList<Model> models)
        {
            if(models.Count == 0)
                return null;

            var lookup = new ModelLookup{InstanceKey = instanceKey};
            foreach (var model in models)
            {
                model.Childs.Clear();
                lookup.ModelDictionary.Add(model.Key, model);
            }

            foreach (var model in models)
            {
                //如果是根模块
                if (model.ParentKey == -1)
                {
                    lookup.Root = model;
                }
                else
                {
                    var parent = lookup.ModelDictionary.TryGetValue(model.ParentKey);
                    if (parent != null)
                    {
                        model.ParentKey = parent.ParentKey;
                        int childCount = parent.Childs.Count;
                        if (childCount == 0)
                            parent.Childs.Add(model);
                        if (childCount == 1)
                        {
                            if (model.SortNumber < parent.Childs[0].SortNumber)
                                parent.Childs.Insert(0, model);
                            else
                                parent.Childs.Add(model);
                        }

                        for (int i = 1; i < childCount; i++)
                        {
                            if (model.SortNumber < parent.Childs[0].SortNumber)
                            {
                                parent.Childs.Insert(0, model);
                                break;
                            }
                            if (model.SortNumber > parent.Childs[childCount - 1].SortNumber)
                            {
                                parent.Childs.Add(model);
                                break;
                            }
                            if (parent.Childs[i - 1].SortNumber <= model.SortNumber &&
                                parent.Childs[i].SortNumber > model.SortNumber)
                            {
                                parent.Childs.Insert(i, model);
                                break;
                            }
                        }
                    }
                }
            }
            return lookup;
        }

        /// <summary>
        /// 获取指定域下的菜单树
        /// </summary>
        /// <param name="instanceKey"></param>
        private ModelLookup GetModelLookupInner(string instanceKey)
        {
            var model = GetModelList(instanceKey);
            return CreateModelTree(instanceKey, model);
        }

        /// <summary>
        /// 获取没有使用的模块
        /// </summary>
        /// <param name="root"></param>
        /// <param name="uselessModels"></param>
        /// <returns></returns>
        private int GetUselessModel(Model root, IList<Model> uselessModels)
        {
            int flag = 0;
            foreach (var model in root.Childs)
            {
                if (model.Childs.Count > 0)
                    flag += GetUselessModel(model, uselessModels);
                if (model.Childs.Count == 0 && model.ActionKey == null)
                    uselessModels.Add(model);
                if (model.ActionKey != null)
                    flag++;
            }

            if (flag == 0 && root.ActionKey == null)
            {
                uselessModels.Add(root);
            }
            return flag;
        }

        /// <summary>
        /// 获取有使用的模块
        /// </summary>
        /// <param name="root"></param>
        /// <param name="usedModels"></param>
        /// <returns></returns>
        private int GetUsedModel(Model root, IList<Model> usedModels)
        {
            int flag = 0;
            foreach (var model in root.Childs)
            {
                if (model.Childs.Count > 0)
                    flag += GetUsedModel(model, usedModels);
                if (model.Childs.Count != 0 && model.ActionKey == null)
                    usedModels.Add(model);
                if (model.ActionKey != null)
                    flag++;
            }

            if (flag == 0 && root.ActionKey == null)
            {
                usedModels.Add(root);
            }
            return flag;
        }

        /// <summary>
        /// 处理用户模块
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="userModel"></param>
        /// <param name="userId"></param>
        /// <param name="instanceKey"></param>
        private void ProcessUserModel(Model systemModel, Model userModel, int userId, string instanceKey)
        {
            foreach (var child in systemModel.Childs)
            {
                if (String.IsNullOrWhiteSpace(child.ActionKey) || 
                    AccessServices.CheckAction(userId, instanceKey, child.ActionKey))
                {
                    var childUserModel = child.Clone();
                    ProcessUserModel(child, childUserModel, userId, instanceKey);

                    //有下级或当前级非目录
                    if (!String.IsNullOrEmpty(childUserModel.Url) || childUserModel.Childs.Count > 0)
                        userModel.Childs.Add(childUserModel);
                }
            }
        }

        private class ModelLookup
        {
            /// <summary>
            /// 域实例键
            /// </summary>
            public string InstanceKey { get; set; }

            /// <summary>
            /// 根模块
            /// </summary>
            public Model Root { get; set; }

            /// <summary>
            /// 模块字典
            /// </summary>
            public Dictionary<int, Model> ModelDictionary { get; private set; }

            /// <summary>
            /// 实例对应的模块对象
            /// </summary>
            public ModelLookup()
            {
                ModelDictionary = new Dictionary<int, Model>();
            }
        }
    }
}
