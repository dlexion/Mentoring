namespace StockExchange.Task3
{
    public class Players
    {
        public RedSocks RedSocks { get; set; }
        public Blossomers Blossomers { get; set; }
        public RossSocks RossSocks { get; set; }

        public Players()
        {
        }

        public Players(RedSocks redSocks, Blossomers blossomers, RossSocks rossSocks)
        {
            RedSocks = redSocks;
            Blossomers = blossomers;
            RossSocks = rossSocks;
        }
    }
}
