using System;

namespace StockExchange.Task3
{
    public class RossSocks : IStockListener
    {
        public int SoldShares { get; private set; }

        public int BoughtShares { get; private set; }

        public RossSocks()
        {
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            StockMarket.Subscribe(this);
            return StockMarket.MakeStockRequest(new Stock(false, stockName, numberOfShares, nameof(RossSocks)));
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            StockMarket.Subscribe(this);
            return StockMarket.MakeStockRequest(new Stock(true, stockName, numberOfShares, nameof(RossSocks)));
        }

        public void Update(Stock stock)
        {
            if (stock.Player == nameof(RossSocks))
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
