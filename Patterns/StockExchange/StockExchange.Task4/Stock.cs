namespace StockExchange.Task4
{
    public class Stock
    {
        public Stock(bool type, string name, int number, string player)
        {
            Type = type;
            Name = name;
            Number = number;
            Player = player;
        }

        public bool Type { get; }

        public string Name { get; }

        public int Number { get; }

        public string Player { get; }
    }
}