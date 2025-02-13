using Microsoft.OpenApi.Any;

namespace StockInventoryWebAPI.Models.Stock
{
    public class ApiResponse
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
