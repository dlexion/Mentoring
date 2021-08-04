using Facade.Task1.OrderPlacement;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Facade.Task1.Tests
{
    [TestClass]
    public class OrderPlacementFacadeTests
    {
        private string productId;
        private string productName;
        private string productCategory;
        private decimal productPrice;
        private int quantity;
        private string email;

        private Product product;
        private List<Payment> payments = new List<Payment>();
        private List<Invoice> invoices = new List<Invoice>();

        private OrderFacade orderFacade;

        public OrderPlacementFacadeTests()
        {
            productId = Guid.NewGuid().ToString();
            productName = "My Product";
            productCategory = "My Category";
            productPrice = 120;
            quantity = 12;
            email = "test@gmail.com";

            product = new Product
            {
                Id = productId,
                Name = productName,
                Category = productCategory,
                Price = productPrice
            };
        }

        [TestMethod]
        public void Should_print()
        {
            var invoiceSystemMock = new Mock<InvoiceSystem>();
            var paymentSystemMock = new Mock<PaymentSystem>();
            var productCatalogMock = new Mock<ProductCatalog>();

            productCatalogMock.Setup(c => c.GetProductDetails(productId)).Returns(product);
            paymentSystemMock.Setup(c => c.MakePayment(It.IsAny<Payment>())).Callback<Payment>(payment => payments.Add(payment));
            invoiceSystemMock.Setup(i => i.SendInvoice(It.IsAny<Invoice>())).Callback<Invoice>(invoice => invoices.Add(invoice));

            orderFacade = new OrderFacade(invoiceSystemMock.Object, paymentSystemMock.Object, productCatalogMock.Object);
            orderFacade.PlaceOrder(productId, quantity, email);

            payments.First().Should().BeEquivalentTo(new Payment
            {
                ProductId = productId,
                ProductName = productName,
                Quantity = quantity,
                TotalPrice = productPrice
            });

            invoices.First().Should().BeEquivalentTo(new Invoice
            {
                ProductId = productId,
                ProductName = productName,
                Quantity = quantity,
                TotalPrice = productPrice,
                CustomerEmail = email
            }, opts => opts.Excluding(x => x.InvoiceNumber).Excluding(x => x.SendDate).Excluding(x => x.DueDate));
        }
    }
}