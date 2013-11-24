namespace Avalon.Framework
{
    /// <summary>
    /// 引导程序任务接口
    /// </summary>
    public interface IBootstrapperTask
    {
        /// <summary>
        /// 执行
        /// </summary>
        void Run();
        /// <summary>
        /// 重置
        /// </summary>
        void Reset();
    }
}
