using System;

namespace StockExchange.Task3
{
    public class Blossomers : IStockListener
    {
        public int SoldShares { get; private set; }

        public int BoughtShares { get; private set; }

        public Blossomers()
        {
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            StockMarket.Subscribe(this);
            return StockMarket.MakeStockRequest(new Stock(false, stockName, numberOfShares, nameof(Blossomers)));
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            StockMarket.Subscribe(this);
            return StockMarket.MakeStockRequest(new Stock(true, stockName, numberOfShares, nameof(Blossomers)));
        }

        public void Update(Stock stock)
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
