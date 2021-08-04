using System;

namespace StockExchange.Task4
{
    public class Blossomers
    {
        public int SoldShares { get; private set; }

        public int BoughtShares { get; private set; }

        public Blossomers()
        {
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            StockMarket.StockRequestCompleted += Update;
            return StockMarket.MakeStockRequest(new Stock(false, stockName, numberOfShares, nameof(Blossomers)));
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            StockMarket.StockRequestCompleted += Update;
            return StockMarket.MakeStockRequest(new Stock(true, stockName, numberOfShares, nameof(Blossomers)));
        }

        void Update(object sender, Stock stock)
        {
            if (stock.Player == nameof(Blossomers))
            {
                if (stock.Type)
                {
                    BoughtShares += stock.Number;
                }
                else
                {
                    SoldShares += stock.Number;
                }
            }
            else
            {
                if (!stock.Type)
                {
                    BoughtShares += stock.Number;
                }
                else
                {
                    SoldShares += stock.Number;
                }
            }
        }
    }
}
