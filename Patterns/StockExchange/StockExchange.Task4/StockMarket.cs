using System;
using System.Collections.Generic;

namespace StockExchange.Task4
{
    public static class StockMarket
    {
        private static List<Stock> stocks =
            new List<Stock>();

        public static event EventHandler<Stock> StockRequestCompleted = (sender, stock) => { };

        public static bool MakeStockRequest(Stock stock)
        {
            var existingStock = stocks.Find(x => x.Type == !stock.Type
                                                 && x.Player != stock.Player
                                                 && x.Name == stock.Name
                                                 && x.Number == stock.Number);
            if (existingStock != null)
            {
                stocks.Remove(existingStock);
                StockRequestCompleted.Invoke(null, existingStock);
                return true;
            }

            stocks.Add(stock);
            return false;
        }

        public static void Clear()
        {
            stocks.Clear();
            StockRequestCompleted = (sender, stock) => { };
        }
    }
}