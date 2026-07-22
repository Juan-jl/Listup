using Listup.Models;
using System.Globalization;

namespace Listup.Helpers
{
    class FormatCurrency
    {
        private readonly Cart _cart;

        public FormatCurrency(Cart cart)
        {
            _cart = cart;
        }

        public string GetCurrencySymbol()
        {
            var currency = GetCurrency();
            if (currency != null)
            {
                return currency.Symbol;
            }
            else
            {
                return "R$";
            }
        }

        public string GetPriceFormatted(double price)
        {
            return price.ToString(GetDecimalFormat(), CultureInfo.InvariantCulture)
                        .Replace(".", GetDecimalSeparator());
        }

        private string GetDecimalSeparator()
        {
            var currency = GetCurrency();
            if (currency != null)
            {
                return currency.DecimalSeparator;
            }
            else
            {
                return ".";
            }
        }

        private string GetDecimalFormat()
        {
            var currency = GetCurrency();
            if (currency != null)
            {
                return currency.DecimalFormat;
            }
            else
            {
                return "F2";
            }
        }

        public double ParsePrice(string displayPrice) //converter para o formato certo na hora de salvar
        {
            string text = displayPrice.Replace(",", ".");
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) && value >= 0)
            {
                return value;
            }
            else
            {
                return 0;
            }
        }

        private CurrencyInfo GetCurrency()
        {
            return CurrencyInfo.All.Find(c => c.Code == _cart.Currency);
        }
    }
}