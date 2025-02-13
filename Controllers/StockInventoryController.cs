using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockInventoryWebAPI.Models.Stock;

namespace StockInventoryWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class StockInventoryController : ControllerBase
    {
        private readonly StockDbContext _context;
        public StockInventoryController(StockDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAllPurchase")]
        public ApiResponse GetAllPurchase()
        {
            ApiResponse _res = new ApiResponse();
            try
            {
                
                 var all = (from purchase in _context.StockPurchases
                           join product in _context.StockProducts on purchase.productId equals product.productId
                           select new
                           {
                               purchaseId = purchase.purchaseId,
                               purchaseDate = purchase.purchaseDate,
                               productId = purchase.productId,
                               quantity = purchase.quantity,
                               supplierName = purchase.supplierName,
                               invoiceAmount = purchase.invoiceAmount,
                               invoiceNo = purchase.invoiceNo,       
                               productName = product.productName

                           }).OrderByDescending(x => x.purchaseId).ToList();                
                _res.Result = true;
                _res.Data = all;
                return _res;
            }
            catch (Exception ex)
            {
                _res.Result = false;
                _res.Message = ex.Message; ;
                return _res;
            }
        }

        [HttpPost("CreateNewPurchase")]
        public ApiResponse CreateNewPurchase([FromBody] StockPurchase obj)
        {
            ApiResponse _res = new ApiResponse();

            if (!ModelState.IsValid)
            {
                _res.Result = false;
                _res.Message = "Validation Error";
                return _res;
            }

            try
            {
                var isExist = _context.StockPurchases.SingleOrDefault(m => m.invoiceNo.ToLower() == obj.invoiceNo.ToLower());
                if (isExist == null)
                {
                    _context.StockPurchases.Add(obj);
                    _context.SaveChanges();
                    var isStockExist = _context.StockStocks.SingleOrDefault(m => m.productId == obj.productId);
                    if (isStockExist == null)
                    {
                        StockStock _stock = new StockStock()
                        {
                            createdDate = DateTime.Now,
                            lastModifiedDate = DateTime.Now,
                            productId = obj.productId,
                            quantity = obj.quantity
                        };
                        _context.StockStocks.Add(_stock);
                        _context.SaveChanges();
                    }
                    else
                    {
                        isStockExist.quantity = isStockExist.quantity + obj.quantity;
                        isStockExist.lastModifiedDate = DateTime.Now;
                        _context.SaveChanges();
                    }
                    _res.Result = true;
                    _res.Message = "Purchase Entry Created Successfully";
                    return _res;
                }
                else
                {
                    _res.Result = false;
                    _res.Message = "Invoice No Already Present";
                    return _res;
                }
            }

            catch (Exception ex)
            {
                _res.Message = ex.Message;
                _res.Result = false;
                return _res;
            }
        }

        [HttpGet("GetAllSale")]
        public ApiResponse GetAllSale()
        {
            ApiResponse _res = new ApiResponse();
            try
            {
                var all = (from sale in _context.StockSales
                           join product in _context.StockProducts on sale.productId equals product.productId
                           select new
                           {
                               saleId = sale.saleId,
                               saleDate = sale.saleDate,
                               productId = sale.productId,
                               quantity = sale.quantity,
                               supplierName = sale.customerName,
                               totalAmount = sale.totalAmount,
                               invoiceNo = sale.invoiceNo,
                               productName = product.productName

                           }).OrderByDescending(x => x.saleId).ToList();
                _res.Result= true;
                _res.Data = all;
                return _res;

            }
            catch (Exception ex)
            {
                _res.Message = ex.Message;
                _res.Result = false;
                return _res;
            }
          
        }
        [HttpPost("CreateNewSale")]
        public ApiResponse CreateNewSale([FromBody] StockSale obj)
        {
            ApiResponse _res = new ApiResponse();

            if(!ModelState.IsValid)
            {
                _res.Result = false;
                _res.Message = "Validation Error";
                return _res;
            }
            try
            {
                var isExist = _context.StockSales.SingleOrDefault(m => m.invoiceNo.ToLower() == obj.invoiceNo.ToLower());
                var isStockExist = _context.StockStocks.SingleOrDefault(m => m.productId == obj.productId);
                if (isExist == null && isStockExist != null)
                {
                    _context.StockSales.Add(obj);
                    _context.SaveChanges();
                    isStockExist.quantity = isStockExist.quantity - obj.quantity;
                    isStockExist.lastModifiedDate = DateTime.Now;
                    _context.SaveChanges();
                    _res.Result = true;
                    _res.Message = "Sale Entry Created Successfully";
                    return _res;
                }
                else
                {
                    _res.Result = false;
                    _res.Message = "Invoice No Already Present";
                    return _res;
                }
            }
            catch (Exception ex)
            {
                _res.Message = ex.Message; _res.Result = false;
                return _res;
            }
           
        }

        [HttpGet("GetAllProduct")]
        public ApiResponse GetAllProducts()
        {
            ApiResponse _res = new ApiResponse();
            try
            {
                var all = _context.StockProducts.ToList();
                _res.Result = true;
                _res.Data = all;
                return _res;

            }
            catch(Exception ex)
            {
                _res.Result = false;
                _res.Message= ex.Message;
                return _res;
            }
        }
        [HttpPost("CreateNewProduct")]
        public ApiResponse CreateNewProduct([FromBody] StockProduct obj)
        {
            ApiResponse _res = new ApiResponse();
            if(!ModelState.IsValid)
            {
                _res.Result = false;
                _res.Message = "Validation Error";
                return _res;
            }
            try
            {
                var isExist = _context.StockProducts.SingleOrDefault(m => m.productName.ToLower() == obj.productName.ToLower());
                if (isExist == null)
                {
                    _context.StockProducts.Add(obj);
                    _context.SaveChanges();
                    _res.Result = true;
                    _res.Message = "Product Entry Created Successfully";
                    return _res;

                }
                else
                {
                    _res.Result = false;
                    _res.Message = "Product Name Already Present";
                    return _res;
                }

            }
            catch
            {
                _res.Result = false;
                _res.Message = "Product Name Already Present";
                return _res;
            }
        }
        [HttpGet("GetAllStock")]
        public ApiResponse GetAllStock()
        {
            ApiResponse _res = new ApiResponse();
            try
            {
                var all = (from stock in _context.StockStocks
                           join product in _context.StockProducts on stock.productId equals product.productId
                           select new
                           {
                               createdDate = stock.createdDate,
                               lastModifiedDate = stock.lastModifiedDate,
                               productId = stock.productId,
                               quantity = stock.quantity,
                               productName = product.productName,
                               stockId = stock.stockId

                           }).OrderByDescending(x => x.stockId).ToList();
                _res.Result = true;
                _res.Data = all;
                return _res;

            }
            catch (Exception ex)
            {
                _res.Message = ex.Message;
                _res.Result = false;
                return _res;
            }

        }

        [HttpGet("checkStockByProductId")]
        public ApiResponse CheckStockByProductId(int productId)
        {
            ApiResponse _res = new ApiResponse();
            try
            {
                var stock = _context.StockStocks.FirstOrDefault(m => m.productId == productId);
                if(stock != null)
                {
                    if(stock.quantity != 0)
                    {
                        _res.Result = true;
                        _res.Data = stock;
                        _res.Message = "Stock Available";
                    }
                    else
                    {
                        _res.Result = false;
                        _res.Message = "No Stock Available";
                    }
                }
            }
            catch (Exception ex) 
            {
                _res.Message = ex.Message;
                _res.Result = false;
                return _res;

            }
            return _res;
        }
        

    }
}
