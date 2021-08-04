using System;

namespace Facade.Task1.OrderPlacement
{
    public class OrderFacade
    {
        private readonly InvoiceSystem _invoiceSystem;
        private readonly PaymentSystem _paymentSystem;
        private readonly ProductCatalog _productCatalog;

        public OrderFacade(InvoiceSystem invoiceSystem, PaymentSystem paymentSystem, ProductCatalog productCatalog)
        {
            _invoiceSystem = invoiceSystem;
            _paymentSystem = paymentSystem;
            _productCatalog = productCatalog;
        }

        public void PlaceOrder(string productId, int quantity, string email)
        {
            var product = _productCatalog.GetProductDetails(productId);
            var totalPrice = product.Price;

            _paymentSystem.MakePayment(new Payment()
            {
                ProductId = productId,
                ProductName = product.Name,
                Quantity = quantity,
                TotalPrice = totalPrice
            });

            _invoiceSystem.SendInvoice(new Invoice
            {
                ProductId = productId,
                ProductName = product.Name,
                Quantity = quantity,
                TotalPrice = totalPrice,
                CustomerEmail = email
            });
        }
    }
}
