using System.Collections.Generic;

namespace Listup.Models
{
    public class CurrencyInfo
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string DecimalSeparator { get; set; }
        public string DecimalFormat { get; set; }
        public string Name { get; set; }

        public static List<CurrencyInfo> All { get; } = new List<CurrencyInfo>
        {
            new CurrencyInfo { Code = "BRL", Symbol = "R$", DecimalSeparator = ",", DecimalFormat = "F2", Name="Real Brasileiro" },
            new CurrencyInfo { Code = "USD", Symbol = "$",  DecimalSeparator = ".", DecimalFormat = "F2", Name="Dólar" },
            new CurrencyInfo { Code = "MXN", Symbol = "$",  DecimalSeparator = ".", DecimalFormat = "F2", Name="Peso Mexicano" },
            new CurrencyInfo { Code = "EUR", Symbol = "€",  DecimalSeparator = ",", DecimalFormat = "F2", Name="Euro" },
            new CurrencyInfo { Code = "GBP", Symbol = "£",  DecimalSeparator = ",", DecimalFormat = "F2", Name="Libra Esterlina" },
            new CurrencyInfo { Code = "PYG", Symbol = "₲",  DecimalSeparator = ",", DecimalFormat = "F2", Name="Guaraní Paraguaio" },
            new CurrencyInfo { Code = "JPY", Symbol = "¥",  DecimalSeparator = ".", DecimalFormat = "F0", Name="Iene" },
            new CurrencyInfo { Code = "INR", Symbol = "₹",  DecimalSeparator = ".", DecimalFormat = "F2", Name="Rúpia" },
            new CurrencyInfo { Code = "PESO", Symbol = "$", DecimalSeparator = ",", DecimalFormat = "F2", Name="Peso" },
            new CurrencyInfo { Code = "RUB", Symbol = "₽", DecimalSeparator = ",", DecimalFormat = "F2", Name="Rublo Russo" },
            new CurrencyInfo { Code = "CNY", Symbol = "¥",  DecimalSeparator = ".", DecimalFormat = "F2", Name="Yuan Chinês" },
            new CurrencyInfo { Code = "KRW", Symbol = "₩",  DecimalSeparator = ",", DecimalFormat = "F0", Name="Won Sul-Coreano" }

        };
    }
}