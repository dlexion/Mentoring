using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StockExchange.Task3.Tests
{
    [TestClass]
    public class StockExchangeTests
    {
        StockPlayersFactory playersFactory;
        RedSocks redSocks;
        Blossomers blossomers;

        [TestInitialize]
        public void Startup()
        {
            playersFactory = new StockPlayersFactory();

            var players = playersFactory.CreatePlayers();

            redSocks = players.RedSocks;
            blossomers = players.Blossomers;
        }

        [TestMethod]
        public void Should_not_buy_shares_when_nobody_sold_them()
        {
            redSocks.BuyOffer("MSC", 3).Should().BeFalse();
            blossomers.BuyOffer("RTC", 2).Should().BeFalse();
        }

        [TestMethod]
        public void Should_not_sell_shares_when_nobody_sold_them()
        {
            redSocks.SellOffer("MSC", 3).Should().BeFalse();
            blossomers.SellOffer("RTC", 3).Should().BeFalse();
        }

        [TestMethod]
        public void RedSocks_should_not_buy_own_shares()
        {
            blossomers.SellOffer("RTC", 2).Should().BeFalse();
            blossomers.BuyOffer("RTC", 2).Should().BeFalse();
        }

        [TestMethod]
        public void Blossomers_should_not_sell_own_shares()
        {
            redSocks.BuyOffer("RTC", 2).Should().BeFalse();
            redSocks.SellOffer("RTC", 2).Should().BeFalse();
        }

        [TestMethod]
        public void RedSocks_should_buy_matched_Blossomers_shares()
        {
            blossomers.SellOffer("RTC", 2).Should().BeFalse();
            redSocks.BuyOffer("RTC", 2).Should().BeTrue();
        }

        [TestMethod]
        public void Blossomers_should_buy_matched_RedSocks_shares()
        {
            redSocks.SellOffer("RTC", 2).Should().BeFalse();
            blossomers.BuyOffer("RTC", 2).Should().BeTrue();
        }

        [TestMethod]
        public void RedSocks_should_sell_matched_Blossomers_shares()
        {
            blossomers.BuyOffer("RTC", 2).Should().BeFalse();
            redSocks.SellOffer("RTC", 2).Should().BeTrue();
        }

        [TestMethod]
        public void Blossomers_should_sell_matched_RedSocks_shares()
        {
            redSocks.BuyOffer("RTC", 2).Should().BeFalse();
            blossomers.SellOffer("RTC", 2).Should().BeTrue();
        }

        [TestMethod]
        public void RedSocks_should_not_sell_not_matched_Blossomers_shares()
        {
            blossomers.BuyOffer("RTC", 2).Should().BeFalse();
            blossomers.BuyOffer("MSC", 1).Should().BeFalse();

            redSocks.SellOffer("RTC", 1).Should().BeFalse();
            redSocks.SellOffer("PTC", 2).Should().BeFalse();
            redSocks.SellOffer("MSC", 2).Should().BeFalse();
        }

        [TestMethod]
        public void Blossomers_should_not_sell_not_matched_RedSocks_shares()
        {
            redSocks.BuyOffer("RTC", 2).Should().BeFalse();
            redSocks.BuyOffer("MSC", 1).Should().BeFalse();

            blossomers.SellOffer("RTC", 1).Should().BeFalse();
            blossomers.SellOffer("PTC", 2).Should().BeFalse();
            blossomers.SellOffer("MSC", 2).Should().BeFalse();
        }

        [TestMethod]
        public void RedSocks_should_not_buy_not_matched_Blossomers_shares()
        {
            blossomers.SellOffer("RTC", 2).Should().BeFalse();
            blossomers.SellOffer("MSC", 1).Should().BeFalse();

            redSocks.BuyOffer("RTC", 1).Should().BeFalse();
            redSocks.BuyOffer("PTC", 2).Should().BeFalse();
            redSocks.BuyOffer("MSC", 2).Should().BeFalse();
        }

        [TestMethod]
        public void Blossomers_should_not_buy_not_matched_RedSocks_shares()
        {
            redSocks.SellOffer("RTC", 2).Should().BeFalse();
            redSocks.SellOffer("MSC", 1).Should().BeFalse();

            blossomers.BuyOffer("RTC", 1).Should().BeFalse();
            blossomers.BuyOffer("PTC", 2).Should().BeFalse();
            blossomers.BuyOffer("MSC", 2).Should().BeFalse();
        }
    }
}
