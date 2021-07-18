using System;

namespace StockExchange.Task4
{
    public class RedSocks
    {
        public int SoldShares { get; private set; }

        public int BoughtShares { get; private set; }

        public RedSocks()
        {
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            StockMarket.StockRequestCompleted += Update;
            return StockMarket.MakeStockRequest(new Stock(false, stockName, numberOfShares, nameof(RedSocks)));
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            StockMarket.StockRequestCompleted += Update;
            return StockMarket.MakeStockRequest(new Stock(true, stockName, numberOfShares, nameof(RedSocks)));
        }

        void Update(object sender, Stock stock)
        {
            if (stock.Player == nameof(RedSocks))
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
