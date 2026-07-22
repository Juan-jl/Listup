using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Listup.Helpers
{
    public class ContactBuilder
    {
        public string Generator()
        {
            string[] vetor =
            {
                "XHUwMDZh",
                "XHUwMDc1",
                "XHUwMDYx",
                "XHUwMDZl",
                "XHUwMDJl",
                "XHUwMDYz",
                "XHUwMDYx",
                "XHUwMDcy",
                "XHUwMDY0",
                "XHUwMDZm",
                "XHUwMDcz",
                "XHUwMDZm",
                "XHUwMDYz",
                "XHUwMDZm",
                "XHUwMDZl",
                "XHUwMDc0",
                "XHUwMDYx",
                "XHUwMDc0",
                "XHUwMDZm",
                "XHUwMDMx",
                "XHUwMDQw",
                "XHUwMDY3",
                "XHUwMDZk",
                "XHUwMDYx",
                "XHUwMDY5",
                "XHUwMDZj",
                "XHUwMDJl",
                "XHUwMDYz",
                "XHUwMDZm",
                "XHUwMDZk"
            };

            for (int i = 0; i < vetor.Length; i++)
            {
                vetor[i] = Encoding.UTF8.GetString(Convert.FromBase64String(vetor[i]));
            }

            return Regex.Unescape(string.Concat(vetor)); ;
        }
    }
}
