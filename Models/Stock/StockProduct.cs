using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockInventoryWebAPI.Models.Stock
{
    [Table("StockProduct")]
    public class StockProduct
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int productId { get; set; }
        public string productName { get; set; }        
        public string categoryName { get; set; }
        public DateTime createdDate { get; set; }
        public double price { get; set; }
        public string productDetails { get; set; }
    }
    [Table("StockPurchase")]
    public class StockPurchase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int purchaseId { get; set; }
        public DateTime purchaseDate { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public string supplierName { get; set; }
        public double invoiceAmount { get; set; }
        public string invoiceNo { get; set; }
    }
    [Table("StockSale")]

    public class StockSale
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int saleId { get; set; }
        public string invoiceNo { get; set; }
        public string customerName { get; set; }
        public int mobileNo { get; set; }
        public DateTime saleDate { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public int totalAmount { get; set; }
        
    }

    [Table("StockStockMaster")]

    public class StockStock
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int stockId { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime lastModifiedDate { get; set; }
    }

}
