using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 支持树结构的领域对象
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ITree<TEntity>
    {
        /// <summary>
        /// 标识
        /// </summary>
        int Id { get; }

        /// <summary>
        /// 父级别标识
        /// </summary>
        int ParentId { get; set; }

        /// <summary>
        /// 同级的排序号
        /// </summary>
        int SortNumber { get; set; }

        /// <summary>
        /// 获取下级集合
        /// </summary>
        IList<TEntity> Children { get; }

        /// <summary>
        /// 获取父级
        /// </summary>
        TEntity Parent { get; set; }
    }

    /// <summary>
    /// 抽象的目录节点
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class AbstractCatalog<TEntity> : ITree<TEntity>
        where TEntity : AbstractCatalog<TEntity>
    {
        public AbstractCatalog()
        {
            Children = new List<TEntity>();
        }

        /// <summary>
        /// 标识（只读）
        /// </summary>
        [JsonProperty]
        public virtual int Id { get; protected set; }

        /// <summary>
        /// 父节点标识（只读）
        /// </summary>
        [JsonProperty]
        public virtual int ParentId { get; protected set; }

        /// <summary>
        /// 同级的排序号（只读）
        /// </summary>
        [JsonProperty]
        public virtual int SortNumber { get; protected set; }

        /// <summary>
        /// 下级集合（只读）
        /// </summary>
        [JsonProperty]
        public virtual IList<TEntity> Children { get; protected set; }

        /// <summary>
        /// 上级对象（只读）
        /// </summary>
        public virtual TEntity Parent { get; protected set; }

        #region ITree

        int ITree<TEntity>.Id
        {
            get { return Id; }
        }

        int ITree<TEntity>.ParentId
        {
            get { return ParentId; }
            set { ParentId = value; }
        }

        int ITree<TEntity>.SortNumber
        {
            get { return SortNumber; }
            set { SortNumber = value; }
        }

        IList<TEntity> ITree<TEntity>.Children
        {
            get { return Children; }
        }

        TEntity ITree<TEntity>.Parent
        {
            get { return Parent; }
            set { Parent = value; }
        }

        #endregion

        /// <summary>
        /// 判断当前的目录是否包含自定的题目
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        public virtual bool Contains(TEntity catalog)
        {
            Arguments.NotNull(catalog, "catalog");

            var c = catalog;
            while (c != null)
            {
                if (c.Id == Id)
                    return true;
                c = c.Parent;
            }
            return false;
        }

        /// <summary>
        /// 获取当前的深度, 顶级为 0
        /// </summary>
        /// <returns></returns>
        public virtual int GetLevel()
        {
            int level = 0;
            var c = this.Parent;
            while (c != null)
            {
                c = c.Parent;
                level++;
            }
            return level;
        }

        /// <summary>
        /// 获取从根目录到当前目录路径上的目录列表（包含当前节点）
        /// </summary>
        /// <returns></returns>
        public virtual IList<TEntity> GetListFromRoot(bool containsRoot = true)
        {
            List<TEntity> items = new List<TEntity>();
            TEntity c = (TEntity)this;
            while (c != null)
            {
                items.Add(c);
                c = c.Parent;
            }

            if (!containsRoot)
                items.RemoveAt(items.Count - 1);

            items.Reverse();
            return items;
        }

        /// <summary>
        /// 获取所有的子集
        /// </summary>
        /// <param name="depth">深度从1开始。</param>
        /// <returns></returns>
        public virtual IList<TEntity> GetAllChildren(int depth = Int32.MaxValue)
        {
            var items = new List<TEntity>();
            Recursive(Children, depth, items);
            return items;
        }

        /// <summary>
        /// 获取将根目录到当前目录的路径信息
        /// </summary>
        /// <param name="pathFunc"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        protected virtual string GetFullPathFromRoot(Func<TEntity, string> pathFunc, string separator = " > ", bool containsRoot = true)
        {
            var items = GetListFromRoot(containsRoot);
            return String.Join(separator, items.Select(o => pathFunc(o)));
        }

        void Recursive(IList<TEntity> children, int depth, IList<TEntity> container)
        {
            if (depth == 0)
                return;

            foreach (var catalog in children)
            {
                container.Add(catalog);
                Recursive(catalog.Children, depth - 1, container);
            }
        }

    }

    /// <summary>
    /// 支持树操作，单根的树结构
    /// </summary>
    public class TreeLookup<TEntity>
        where TEntity : class,ITree<TEntity>
    {
        protected TreeLookup() { }

        public TreeLookup(IEnumerable<TEntity> nodes, Func<TEntity> createRootFunc = null)
        {
            OnInit(nodes, createRootFunc);
        }

        public TreeLookup(TEntity rootNode)
        {
            OnInit(rootNode);
        }

        /// <summary>
        /// 根目录
        /// </summary>
        public TEntity Root { get; set; }

        /// <summary>
        /// 目录字典
        /// </summary>
        public Dictionary<int, TEntity> NodeDictionary { get; private set; }

        /// <summary>
        /// 根据标识获取节点信息
        /// </summary>
        public TEntity GetNode(int id)
        {
            return NodeDictionary.TryGetValue(id);
        }

        /// <summary>
        /// 根据给定的标识批量获取节点信息
        /// </summary>
        public IList<TEntity> GetNodes(IEnumerable<int> ids)
        {
            return ids.Select(o => NodeDictionary.TryGetValue(o)).Where(o => o != null).ToList();
        }

        /// <summary>
        /// 判断给定的两个节点是否包含关系
        /// </summary>
        public bool ContainsNode(TEntity parent, TEntity child)
        {
            if (parent == null)
                throw new ArgumentException("parent不能为空");
            if (child == null)
                throw new ArgumentException("child不能为空");

            var c = child;
            while (c != null)
            {
                if (c.Id == parent.Id)
                    return true;
                c = c.Parent;
            }
            return false;
        }

        /// <summary>
        /// 获取给定标识及深度下的节点集合，不包含本身
        /// </summary>
        /// <param name="id"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public IList<TEntity> GetAllChildren(int id, int depth = 0)
        {
            if (depth == 0)
                depth = Int32.MaxValue;

            return GetAllChildren(GetNode(id), depth);
        }

        /// <summary>
        /// 获取给定节点的深度，根的深度为0
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int GetLevel(TEntity node)
        {
            int level = 0;
            var parent = node.Parent;
            while (parent != null)
            {
                parent = parent.Parent;
                level++;
            }
            return level;
        }

        /// <summary>
        /// 在指定节点的子集加入指定的目录节点，并置于指定的位置
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="index"></param>
        /// <param name="child"></param>
        /// <param name="createAction"></param>
        /// <param name="updateAction"></param>
        public void AppendNode(TEntity parent, int index, TEntity child, Action<TEntity> createAction, Action<TEntity> updateAction)
        {
            if (parent == null)
                throw new ArgumentException("parent不能为空");
            if (child == null)
                throw new ArgumentException("child不能为空");
            if (index < 0)
                throw new ArgumentException("index不能小于0");

            parent = GetNode(parent.Id);
            var isNew = child.Id == 0;

            if (isNew)
            {
                if (child.Children.Count > 0)
                    throw new ArgumentException("创建新目录时，child的子级只能为空集合");
                if (child.Parent != null)
                    throw new ArgumentException("创建新目录时，child的Parent只能为空");
            }
            else
            {
                if (ContainsNode(child, parent))
                    throw new ArgumentException("嵌套引用，给定的目录为自身或包含当前目录");
            }

            var oldSortNumber = child.SortNumber;
            var oldParent = child.Parent;

            //OnAppendNode(parent, index, child);

            //处理排序号
            int sortNumber = 1;
            if (index < parent.Children.Count)
                sortNumber = parent.Children.ElementAtOrDefault(index).GetOrDefault(o => o.SortNumber);
            else
                sortNumber = parent.Children.LastOrDefault().GetOrDefault(o => o.SortNumber) + 1;

            //同级节点向下移动要修正一个数量
            int fixIndex = index;
            if (parent.Children.Contains(child) && parent.Children.IndexOf(child) < index)
            {
                fixIndex++;
                sortNumber++;
            }

            child.SortNumber = sortNumber;
            SetParent(parent, child);

            try
            {
                if (isNew)
                    createAction(child);
                else
                    updateAction(child);
            }
            catch
            {
                if (!isNew)
                {
                    child.SortNumber = oldSortNumber;
                    SetParent(oldParent, child);
                }
                throw;
            }

            //修复同级的排序
            for (int i = fixIndex; i < parent.Children.Count; i++)
            {
                var temp = parent.Children[i];
                temp.SortNumber = sortNumber + (i - fixIndex) + 1;
                updateAction(temp);
            }

            //处理关系变更
            if (index >= parent.Children.Count)
                parent.Children.Add(child);
            else
                parent.Children.Insert(index, child);

        }

        /// <summary>
        /// 移除给定的节点，并批量删除下级
        /// </summary>
        /// <param name="node"></param>
        /// <param name="deleteAction"></param>
        public void RemoveNode(TEntity node, Action<TEntity> deleteAction)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            if (node.ParentId == 0)
                throw new ArgumentException("顶级目录不允许删除");

            node = GetNode(node.Id);
            var deletes = GetAllChildren(node, Int32.MaxValue);
            deletes.Add(node);

            RemoveChild(node);
            node.Parent = null;

            foreach (var delete in deletes)
            {
                deleteAction(delete);
            }
        }

        /// <summary>
        /// 当在子节点被添加到父节点时触发
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="index"></param>
        /// <param name="child"></param>
        protected virtual void OnAppendNode(TEntity parent, int index, TEntity child)
        {

        }

        /// <summary>
        /// 修正节点同级向下移动
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="index"></param>
        int FixIndex(TEntity parent, TEntity child, int index)
        {
            int fixIndex = index;

            if (parent.Children.Contains(child) && parent.Children.IndexOf(child) < index)
            {
                fixIndex++;
            }

            return fixIndex;
        }

        protected virtual void OnInit(TEntity rootNode)
        {
            Arguments.NotNull(rootNode, "rootNode");
            var nodes = GetAllChildren(rootNode, Int32.MaxValue);
            nodes.Add(rootNode);

            Root = rootNode;
            NodeDictionary = nodes.ToDictionary(o => o.Id);

            SetParentInner(rootNode);
        }

        void SetParentInner(TEntity node)
        {
            foreach (var child in node.Children)
            {
                child.Parent = node;
                SetParentInner(child);
            }
        }

        /// <summary>
        /// 将列表转为树
        /// </summary>
        /// <param name="nodes"></param>
        protected virtual void OnInit(IEnumerable<TEntity> nodes, Func<TEntity> createRootFunc = null)
        {
            //重置数据
            nodes.ForEach(o =>
            {
                o.Children.Clear();
                o.Parent = null;
            });

            //创建根级别
            if (createRootFunc != null && nodes.All(o => o.ParentId != 0))
            {
                var root = createRootFunc();
                Arguments.That(root.ParentId == 0, "root", "根节点的 ParentId 必须为0");
                var items = nodes.ToList();
                items.Add(root);
                nodes = items;
            }

            NodeDictionary = nodes.ToDictionary(o => o.Id);

            foreach (var node in nodes)
            {
                if (node.ParentId == 0)
                    Root = node;
                else
                {
                    var parent = NodeDictionary.TryGetValue(node.ParentId);
                    if (parent != null)
                    {
                        node.Parent = parent;
                        parent.Children.Add(node);
                    }
                }
            }
        }

        IList<TEntity> GetAllChildren(TEntity node, int depth)
        {
            IList<TEntity> result = new List<TEntity>();
            if (node != null)
                GetAllChildren(node.Children, depth, result);

            return result;
        }

        /// <summary>
        /// 递归当前目录的所有子目录
        /// </summary>
        void GetAllChildren(IList<TEntity> children, int depth, IList<TEntity> container)
        {
            if (depth == 0)
                return;

            foreach (var catalog in children)
            {
                container.Add(catalog);
                GetAllChildren(catalog.Children, depth - 1, container);
            }
        }

        void SetParent(TEntity parent, TEntity child)
        {
            RemoveChild(child);
            child.Parent = parent;
            child.ParentId = parent.Id;
        }

        void RemoveChild(TEntity child)
        {
            var parent = child.Parent;
            if (parent != null)
                parent.Children.Remove(child);
        }
    }
}
