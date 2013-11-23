using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Utility
{
    public static class ToolSettingUtil
    {
        public static IEnumerable<SettingNode> GetInstanceSettingNodes(IEnumerable<SettingNode> rootNodes, object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            var name = instance.GetType().Name;
            name = name.Substring(0, 1).ToLower() + name.Substring(1);

            List<SettingNode> settingNodes = new List<SettingNode>();
            foreach (SettingNode rootNode in rootNodes)
            {
                SettingNode targetNode = rootNode.TryGetNode(name);
                if (targetNode != null)
                    settingNodes.Add(targetNode);
                settingNodes.Add(rootNode);
            }
            return settingNodes;
        }

        /// <summary>
        /// 尝试创建缓存实例
        /// </summary>
        /// <param name="node">允许为空</param>
        /// <param name="typePath"></param>
        /// <returns></returns>
        public static T TryCreateInstance<T>(SettingNode node, string typePath) where T : class
        {
            if (node != null)
            {
                string typeName = node.TryGetValue(typePath);
                if (!String.IsNullOrEmpty(typeName))
                {
                    Type type = Type.GetType(typeName);
                    if (type == null)
                        throw new ArgumentNullException("typeName", String.Format("无法创建类型为 {0} 的对象", typeName));

                    return (T)FastActivator.Create(type);
                }
            }
            return null;
        }

        public static T TryCreateInstance<T>(IEnumerable<SettingNode> nodes, string nodeName, string typePath) where T : class
        {
            string typeName = TryGetValue(nodes, nodeName, typePath);
            if (!String.IsNullOrEmpty(typeName))
            {
                Type type = Type.GetType(typeName);
                if (type == null)
                    throw new ArgumentNullException("typeName", "无法创建类型 " + typeName + " 。");
                return (T)FastActivator.Create(type);
            }
            return null;
        }

        public static string TryGetValue(IEnumerable<SettingNode> settingNodes, string nodeName, string path)
        {
            bool flag = true;
            foreach (SettingNode node in settingNodes)
            {
                if (flag || node.Name == nodeName)
                {
                    flag = false;
                    string v = node.TryGetValue(path);
                    if (!String.IsNullOrEmpty(v))
                        return v;
                }
            }
            return null;
        }

        public static SettingNode TryGetNode(IEnumerable<SettingNode> settingNodes, string nodeName, string path)
        {
            bool flag = true;
            foreach (SettingNode node in settingNodes)
            {
                if (flag || node.Name == nodeName)
                {
                    flag = false;
                    SettingNode targetNode = node.TryGetNode(path);
                    if (targetNode != null)
                        return targetNode;
                }
            }
            return null;
        }

        public static IEnumerable<SettingNode> TryGetNodes(IEnumerable<SettingNode> settingNodes, string nodeName, string path)
        {
            bool flag = true;
            foreach (SettingNode node in settingNodes)
            {
                if (flag || node.Name == nodeName)
                {
                    flag = false;
                    IEnumerable<SettingNode> nodes = node.TryGetNodes(path);
                    if (nodes.Count() > 0)
                        return nodes;
                }
            }
            return new List<SettingNode>();
        }

        public static void TrySetSetting<TEntity, TProperty>(this TEntity instance, IEnumerable<SettingNode> settingNodes, string nodeName, string path, Expression<Func<TEntity, TProperty>> property)
        {
            TrySetSetting(instance, settingNodes, nodeName, path, property, (v) => (TProperty)Convert.ChangeType(v, typeof(TProperty)));
        }

        public static void TrySetSetting<TEntity, TProperty>(this TEntity instance, IEnumerable<SettingNode> settingNodes, string nodeName, string path, Expression<Func<TEntity, TProperty>> property, Func<string, TProperty> converter)
        {
            var value = ToolSettingUtil.TryGetValue(settingNodes, nodeName, path);
            if (!String.IsNullOrEmpty(value))
            {
                var ta = TypeAccessor.GetAccessor(instance.GetType());
                ta.SetProperty(GetProperty(property.Body).Name, instance, converter(value));
            }
        }

        public static void TrySetSetting<TEntity, TProperty>(this TEntity instance, SettingNode settingNode, string path, Expression<Func<TEntity, TProperty>> property)
        {
            TrySetSetting(instance, settingNode, path, property, (v) => (TProperty)Convert.ChangeType(v, typeof(TProperty)));
        }

        public static void TrySetSetting<TEntity, TProperty>(this TEntity instance, SettingNode settingNode, string path, Expression<Func<TEntity, TProperty>> property, Func<string, TProperty> converter)
        {
            var value = settingNode.TryGetValue(path);
            if (!String.IsNullOrEmpty(value))
            {
                var ta = TypeAccessor.GetAccessor(instance.GetType());
                ta.SetProperty(GetProperty(property.Body).Name, instance, converter(value));
            }
        }

        static PropertyInfo GetProperty(Expression expression)
        {
            MemberExpression memberExpression = null;
            if (expression.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression as MemberExpression;
            }

            if (memberExpression == null)
            {
                return null;
            }

            return (PropertyInfo)memberExpression.Member;
        }

    }
}
