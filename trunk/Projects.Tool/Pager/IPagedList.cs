namespace Projects.Tool.Pager
{
    public interface IPagedList
    {
        int CurrentPageIndex { get; set; }
        int PageSize { get; set; }
        int TotalItemCount { get; set; }
    }
}
