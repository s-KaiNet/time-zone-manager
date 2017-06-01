namespace TimeZoneManager.Services.Common
{
    public class FilterQuery
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string Filter { get; set; } = string.Empty;
    }
}
