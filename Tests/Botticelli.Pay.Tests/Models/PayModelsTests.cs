using Botticelli.Pay.Message;
using Botticelli.Pay.Models;
using Botticelli.Shared.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Pay.Tests.Models;

[TestFixture]
public class PayModelsTests
{
    private static Currency CreateCurrency(string iso = "USD", int? decimals = 2) => new()
    {
        Iso = iso,
        Name = "United States Dollar",
        Demonym = "US",
        MajorSingle = "Dollar",
        MajorPlural = "Dollars",
        Symbol = "$",
        SymbolNative = "$",
        MinorSingle = "Cent",
        MinorPlural = "Cents",
        IsOnum = 840,
        IsOdigits = 2,
        Decimals = decimals,
        NumToBasic = 100
    };

    [Test]
    public void Invoice_TotalAmount_Should_SumPrices()
    {
        var invoice = new Invoice
        {
            Title = "Item",
            Description = "Desc",
            Payload = "payload",
            Currency = CreateCurrency(),
            Prices =
            [
                new Price { Label = "A", Amount = 10.5m },
                new Price { Label = "B", Amount = 1.25m }
            ],
            ProviderToken = "token",
            ProviderData = "{}",
            AdditionalParameters = ["a", "b"]
        };

        invoice.TotalAmount.Should().Be(11.75m);
        invoice.AdditionalParameters.Should().Contain("a");
        invoice.ProviderData.Should().Be("{}");
    }

    [Test]
    public void Invoice_TotalAmount_WhenPricesNull_Should_BeZero()
    {
        var invoice = new Invoice
        {
            Title = "Item",
            Description = "Desc",
            Payload = "payload",
            Currency = CreateCurrency(),
            Prices = null!,
            ProviderToken = null
        };

        invoice.TotalAmount.Should().Be(0m);
    }

    [Test]
    public void SuccessfulPayment_And_RefundedPayment_Should_ExposeProperties()
    {
        var currency = CreateCurrency();
        var order = new OrderInfo
        {
            Name = "John",
            PhoneNumber = "+100",
            Email = "a@b.c",
            ShippingAddress = new ShippingAddress
            {
                CountryCode = "US",
                State = "CA",
                City = "LA",
                StreetLine1 = "Main",
                StreetLine2 = "Apt 1",
                PostCode = "90001"
            }
        };

        var success = new SuccessfulPayment
        {
            Currency = currency,
            TotalAmount = 12.3m,
            InvoicePayload = "p",
            MessengerPaymentChargeId = "m1",
            ProviderPaymentChargeId = "p1",
            SubscriptionExpirationDate = DateTime.UtcNow.AddDays(1),
            IsRecurring = true,
            IsFirstRecurring = false,
            ShippingOptionId = "ship",
            OrderInfo = order
        };

        var refund = new RefundedPayment
        {
            Currency = currency,
            TotalAmount = 12.3m,
            InvoicePayload = "p",
            MessengerPaymentChargeId = "m1",
            ProviderPaymentChargeId = "p1"
        };

        success.IsRecurring.Should().BeTrue();
        success.OrderInfo!.ShippingAddress!.City.Should().Be("LA");
        refund.TotalAmount.Should().Be(12.3m);
    }

    [Test]
    public void PreCheckoutQuery_Should_HoldUserAndCurrency()
    {
        var query = new PreCheckoutQuery
        {
            Id = "q1",
            From = new User { Id = "1", Name = "Ann" },
            Currency = CreateCurrency("EUR"),
            TotalAmount = 5,
            InvoicePayload = "inv",
            ShippingOptionId = "s1",
            OrderInfo = new OrderInfo { Name = "Ann" }
        };

        query.From.Name.Should().Be("Ann");
        query.Currency.Iso.Should().Be("EUR");
        query.ShippingOptionId.Should().Be("s1");
    }

    [Test]
    public void PayMessages_Should_ExposeInvoiceAndPreCheckout()
    {
        var invoiceMessage = new PayInvoiceMessage
        {
            Invoice = new Invoice
            {
                Title = "T",
                Description = "D",
                Payload = "P",
                Currency = CreateCurrency(),
                Prices = [new Price { Label = "L", Amount = 1 }],
                ProviderToken = "tok"
            }
        };

        var preCheckout = new PayPreCheckoutMessage
        {
            PreCheckoutQuery = new PreCheckoutQuery
            {
                Id = "1",
                From = new User { Id = "u" },
                Currency = CreateCurrency(),
                TotalAmount = 1,
                InvoicePayload = "p"
            }
        };

        invoiceMessage.Invoice!.Title.Should().Be("T");
        preCheckout.PreCheckoutQuery.Id.Should().Be("1");
    }

    [Test]
    public void Currency_OptionalFields_Should_BeAssignable()
    {
        var currency = CreateCurrency();
        currency.IsOnum = null;
        currency.IsOdigits = null;
        currency.Decimals = null;
        currency.NumToBasic = null;

        currency.Name.Should().Be("United States Dollar");
        currency.Decimals.Should().BeNull();
    }
}
