namespace Avalon.Resource
{
    public interface IResourceCollector
    {
        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="path">资源路径</param>
        void Add(string path);
        /// <summary>
        /// 资源类型
        /// </summary>
        ResourceType ResourceType { get; set; }
        /// <summary>
        /// 是否启用调试模式
        /// </summary>
        bool Debug { get; set; }
        /// <summary>
        /// 资源分组名称
        /// </summary>
        string Group { get; set; }
        /// <summary>
        /// 基础目录
        /// </summary>
        string BaseFolder { get; set; }
        /// <summary>
        /// 生成HTML标签
        /// </summary>
        /// <returns></returns>
        string BuildHtmlTag();
    }
}
