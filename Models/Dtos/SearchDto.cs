namespace WebApplication2.Models.Dtos
{
    public class SearchDto
    {
        //搜尋
        public string? keyword {  get; set; }
        public int? categoryId { get; set; } = 0;//0表示不根據分類編號搜尋

        //排序
        public string? sortBy {  get; set; }
        public string? sortType { get; set; } = "asc";

        //分頁
        public int? page { get; set; } = 1;
        public int? pageSize { get; set; } = 9;
    }
}
