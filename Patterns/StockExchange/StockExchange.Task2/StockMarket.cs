using System.Collections.Generic;

namespace StockExchange.Task2
{
    public static class StockMarket
    {
        private static List<Stock> stocks =
            new List<Stock>();

        public static bool MakeStockRequest(Stock stock)
        {
            var existingStock = stocks.Find(x => x.Type == !stock.Type
                                                 && x.Player != stock.Player
                                                 && x.Name == stock.Name
                                                 && x.Number == stock.Number);
            if (existingStock != null)
            {
                stocks.Remove(existingStock);
                return true;
            }

            stocks.Add(stock);
            return false;
        }

        public static void Clear()
        {
            stocks.Clear();
        }
    }
}