
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.Models;
using StockApp.ServiceContracts;
using StockApp.Services;

namespace StockApp.Controllers
{
    public class FinnhubController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly TradingOptions _tradingOptions;
        public FinnhubController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions.Value;
        }
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.DefaultStockSymbol == null)
            {
                _tradingOptions.DefaultStockSymbol = "MSFT";
            }

            Dictionary<string, object>? responseData = await _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);

            Stock stock = new Stock()
            {
                Symbol = _tradingOptions.DefaultStockSymbol,
                CurrentPrice = Convert.ToDouble(responseData["c"].ToString()),
                HighestPrice = Convert.ToDouble(responseData["h"].ToString()),
                LowestPrice = Convert.ToDouble(responseData["l"].ToString()),
                OpenPrice = Convert.ToDouble(responseData["o"].ToString()),
            };
            return View(stock);
        }
    }
}