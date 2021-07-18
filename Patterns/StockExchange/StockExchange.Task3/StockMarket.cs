using System.Collections.Generic;

namespace StockExchange.Task3
{
    public static class StockMarket
    {
        private static List<Stock> stocks =
            new List<Stock>();

        private static List<IStockListener> listeners = new List<IStockListener>();

        public static bool MakeStockRequest(Stock stock)
        {
            var existingStock = stocks.Find(x => x.Type == !stock.Type
                                                 && x.Player != stock.Player
                                                 && x.Name == stock.Name
                                                 && x.Number == stock.Number);
            if (existingStock != null)
            {
                stocks.Remove(existingStock);
                foreach (var listener in listeners)
                {
                    listener.Update(existingStock);
                }
                return true;
            }

            stocks.Add(stock);
            return false;
        }

        public static void Subscribe(IStockListener listener)
        {
            if(listener == null)
                return;


            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public static void Unsubscribe(IStockListener listener)
        {
            if (listener == null)
                return;

            if (!listeners.Contains(listener))
                listeners.Remove(listener);
        }

        public static void Clear()
        {
            stocks.Clear();
            listeners.Clear();
        }
    }
}