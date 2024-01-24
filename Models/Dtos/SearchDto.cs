namespace WebApplication2.Models.Dtos
{
    public class SearchDto
    {
        //搜尋
        public string? Keyword {  get; set; }
        public int? CategoryId { get; set; } = 0;//0表示不根據分類編號搜尋

        //排序
        public string? SortBy {  get; set; }
        public string? SortType { get; set; } = "asc";

        //分頁
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 9;
    }
}
