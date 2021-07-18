namespace StockExchange.Task2
{
    public class StockPlayersFactory
    {
        public Players CreatePlayers()
        {
            return new Players
            {
                RedSocks = new RedSocks(),
                Blossomers = new Blossomers()
            };
        }
    }
}
