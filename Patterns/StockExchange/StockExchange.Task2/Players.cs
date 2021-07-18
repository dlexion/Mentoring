namespace StockExchange.Task2
{
    public class Players
    {
        public RedSocks RedSocks { get; set; }
        public Blossomers Blossomers { get; set; }
        public RossStones RossStones { get; set; }

        public Players()
        {
        }

        public Players(RedSocks redSocks, Blossomers blossomers, RossStones rossStones)
        {
            RedSocks = redSocks;
            Blossomers = blossomers;
            RossStones = rossStones;
        }
    }
}
