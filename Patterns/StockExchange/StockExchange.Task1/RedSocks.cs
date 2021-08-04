using System;

namespace StockExchange.Task1
{
    public class RedSocks
    {
        public RedSocks()
        {
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            return StockMarket.MakeStockRequest(new Stock(false, stockName, numberOfShares, nameof(RedSocks)));
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            return StockMarket.MakeStockRequest(new Stock(true, stockName, numberOfShares, nameof(RedSocks)));
        }
    }
}
