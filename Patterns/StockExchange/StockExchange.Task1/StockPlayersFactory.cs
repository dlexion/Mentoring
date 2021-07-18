namespace StockExchange.Task1
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
