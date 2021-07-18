using System;

namespace StockExchange.Task1
{
    public class Blossomers
    {
        public Blossomers() 
        { 
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            return StockMarket.MakeStockRequest(new Stock(false, stockName, numberOfShares, nameof(Blossomers)));
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            return StockMarket.MakeStockRequest(new Stock(true, stockName, numberOfShares, nameof(Blossomers)));
        }
    }
}
