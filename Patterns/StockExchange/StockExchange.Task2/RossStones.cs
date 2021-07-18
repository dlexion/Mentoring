using System;

namespace StockExchange.Task2
{
    public class RossStones
    {
        public bool SellOffer(string stockName, int numberOfShares)
        {
            return StockMarket.MakeStockRequest(new Stock(false, stockName, numberOfShares, nameof(RossStones)));
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            return StockMarket.MakeStockRequest(new Stock(true, stockName, numberOfShares, nameof(RossStones)));
        }
    }
}