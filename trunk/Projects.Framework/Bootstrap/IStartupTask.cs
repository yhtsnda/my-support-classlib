namespace Projects.Framework.Bootstrap
{
    public interface IStartupTask
    {
        void Run();
        void Reset();
    }
}
