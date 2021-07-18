using System;

namespace StockExchange.Task3
{
    public class RedSocks : IStockListener
    {
        public int SoldShares { get; private set; }

        public int BoughtShares { get; private set; }

        public RedSocks()
        {
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            StockMarket.Subscribe(this);
            return StockMarket.MakeStockRequest(new Stock(false, stockName, numberOfShares, nameof(RedSocks)));
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            StockMarket.Subscribe(this);
            return StockMarket.MakeStockRequest(new Stock(true, stockName, numberOfShares, nameof(RedSocks)));
        }

        public void Update(Stock stock)
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
