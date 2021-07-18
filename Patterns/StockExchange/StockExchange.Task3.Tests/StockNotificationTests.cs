using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StockExchange.Task3.Tests
{
    [TestClass]
    public class StockNotificationTests
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
        public void Should_not_notify_when_bought_requests_are_not_matched()
        {
            redSocks.BuyOffer("MSC", 3).Should().BeFalse();
            redSocks.BoughtShares.Should().Be(0);

            blossomers.BuyOffer("RTC", 2).Should().BeFalse();
            blossomers.BoughtShares.Should().Be(0);
        }

        [TestMethod]
        public void Should_not_notify_sell_shares_when_nobody_sold_them()
        {
            redSocks.SellOffer("MSC", 3).Should().BeFalse();
            redSocks.SoldShares.Should().Be(0);

            blossomers.SellOffer("RTC", 3).Should().BeFalse();
            blossomers.SoldShares.Should().Be(0);
        }

        [TestMethod]
        public void RedSocks_should_not_buy_own_shares()
        {
            blossomers.SellOffer("RTC", 2).Should().BeFalse();
            blossomers.SoldShares.Should().Be(0);

            blossomers.BuyOffer("RTC", 2).Should().BeFalse();
            blossomers.BoughtShares.Should().Be(0);
        }

        [TestMethod]
        public void Blossomers_should_not_sell_own_shares()
        {
            redSocks.BuyOffer("RTC", 2).Should().BeFalse();
            redSocks.BoughtShares.Should().Be(0);

            redSocks.SellOffer("RTC", 2).Should().BeFalse();
            redSocks.SoldShares.Should().Be(0);
        }

        [TestMethod]
        public void RedSocks_should_notify_matched_Blossomers_shares()
        {
            blossomers.SellOffer("RTC", 2).Should().BeFalse();
            blossomers.SoldShares.Should().Be(0);

            redSocks.BuyOffer("RTC", 2).Should().BeTrue();
            blossomers.SoldShares.Should().Be(2);
            redSocks.BoughtShares.Should().Be(2);
        }

        [TestMethod]
        public void RedSocks_should_not_notify_when_matched_Blossomers_shares_are_deleted()
        {
            blossomers.SellOffer("RTC", 2).Should().BeFalse();
            blossomers.SoldShares.Should().Be(0);

            redSocks.BuyOffer("RTC", 2).Should().BeTrue();
            blossomers.SoldShares.Should().Be(2);
            redSocks.BoughtShares.Should().Be(2);

            redSocks.BuyOffer("RTC", 2).Should().BeFalse();
            blossomers.SoldShares.Should().Be(2);
            redSocks.BoughtShares.Should().Be(2);
        }

        [TestMethod]
        public void Blossomers_should_notify_buy_matched_RedSocks_shares()
        {
            redSocks.SellOffer("RTC", 2).Should().BeFalse();
            redSocks.SoldShares.Should().Be(0);

            blossomers.BuyOffer("RTC", 2).Should().BeTrue();
            blossomers.BoughtShares.Should().Be(2);
            redSocks.SoldShares.Should().Be(2);
        }

        [TestMethod]
        public void RedSocks_should_notify_sell_matched_Blossomers_shares()
        {
            blossomers.BuyOffer("RTC", 2).Should().BeFalse();
            blossomers.BoughtShares.Should().Be(0);

            redSocks.SellOffer("RTC", 2).Should().BeTrue();
            redSocks.SoldShares.Should().Be(2);
            blossomers.BoughtShares.Should().Be(2);
        }

        [TestMethod]
        public void Blossomers_should_sell_matched_RedSocks_shares()
        {
            redSocks.BuyOffer("RTC", 2).Should().BeFalse();
            redSocks.BoughtShares.Should().Be(0);

            blossomers.SellOffer("RTC", 2).Should().BeTrue();
            redSocks.BoughtShares.Should().Be(2);
            blossomers.SoldShares.Should().Be(2);
        }

        [TestMethod]
        public void RedSocks_should_not_notify_and_sell_not_matched_Blossomers_shares()
        {
            blossomers.BuyOffer("RTC", 2).Should().BeFalse();
            blossomers.BoughtShares.Should().Be(0);
            blossomers.BuyOffer("MSC", 1).Should().BeFalse();
            blossomers.BoughtShares.Should().Be(0);

            redSocks.SellOffer("RTC", 1).Should().BeFalse();
            redSocks.SoldShares.Should().Be(0);
            redSocks.SellOffer("PTC", 2).Should().BeFalse();
            redSocks.SoldShares.Should().Be(0);
            redSocks.SellOffer("MSC", 2).Should().BeFalse();
            redSocks.SoldShares.Should().Be(0);
        }

        [TestMethod]
        public void Blossomers_should_not_notify_and_sell_not_matched_RedSocks_shares()
        {
            redSocks.BuyOffer("RTC", 2).Should().BeFalse();
            redSocks.BoughtShares.Should().Be(0);
            redSocks.BuyOffer("MSC", 1).Should().BeFalse();
            redSocks.BoughtShares.Should().Be(0);

            blossomers.SellOffer("RTC", 1).Should().BeFalse();
            blossomers.SoldShares.Should().Be(0);
            blossomers.SellOffer("PTC", 2).Should().BeFalse();
            blossomers.SoldShares.Should().Be(0);
            blossomers.SellOffer("MSC", 2).Should().BeFalse();
            blossomers.SoldShares.Should().Be(0);
        }

        [TestMethod]
        public void RedSocks_should_not_notify_and_buy_not_matched_Blossomers_shares()
        {
            blossomers.SellOffer("RTC", 2).Should().BeFalse();
            blossomers.SoldShares.Should().Be(0);
            blossomers.SellOffer("MSC", 1).Should().BeFalse();
            blossomers.SoldShares.Should().Be(0);

            redSocks.BuyOffer("RTC", 1).Should().BeFalse();
            redSocks.BoughtShares.Should().Be(0);
            redSocks.BuyOffer("PTC", 2).Should().BeFalse();
            redSocks.BoughtShares.Should().Be(0);
            redSocks.BuyOffer("MSC", 2).Should().BeFalse();
            redSocks.BoughtShares.Should().Be(0);
        }

        [TestMethod]
        public void Blossomers_should_not_buy_not_matched_RedSocks_shares()
        {
            redSocks.SellOffer("RTC", 2).Should().BeFalse();
            redSocks.SoldShares.Should().Be(0);
            redSocks.SellOffer("MSC", 1).Should().BeFalse();
            redSocks.SoldShares.Should().Be(0);

            blossomers.BuyOffer("RTC", 1).Should().BeFalse();
            blossomers.BoughtShares.Should().Be(0);
            blossomers.BuyOffer("PTC", 2).Should().BeFalse();
            blossomers.BoughtShares.Should().Be(0);
            blossomers.BuyOffer("MSC", 2).Should().BeFalse();
            blossomers.BoughtShares.Should().Be(0);
        }
    }
}
