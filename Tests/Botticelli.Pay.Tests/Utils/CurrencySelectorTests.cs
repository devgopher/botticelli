using Botticelli.Pay.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Pay.Tests.Utils;

[TestFixture]
public class CurrencySelectorTests
{
    [Test]
    public void SelectCurrency_KnownIso_Should_ReturnCurrencyWithIso()
    {
        var currency = CurrencySelector.SelectCurrency("USD");

        currency.Should().NotBeNull();
        currency.Iso.Should().Be("USD");
        currency.Name.Should().NotBeNullOrWhiteSpace();
        currency.Symbol.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    public void SelectCurrency_UnknownIso_Should_Throw()
    {
        var act = () => CurrencySelector.SelectCurrency("ZZZ_NOT_A_CURRENCY");

        act.Should().Throw<KeyNotFoundException>()
           .WithMessage("*iso could not be found*");
    }

    [Test]
    public void SelectCurrency_Eur_Should_ReturnEuro()
    {
        var currency = CurrencySelector.SelectCurrency("EUR");

        currency.Iso.Should().Be("EUR");
        currency.Decimals.Should().Be(2);
    }
}
