namespace API.RequestHelpers;

public class Pagination<T>
{
    public Pagination(int pageIndex, int pagesize, int count, IReadOnlyList<T> data)
    {
        PageIndex = pageIndex;
        PageSize = pagesize;
        Count = count;
        Data = data;
    }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public IReadOnlyList<T> Data {get; set;}
}
