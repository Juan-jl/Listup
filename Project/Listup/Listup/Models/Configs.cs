using SQLite;

namespace Listup.Models
{
    public class Configs
    {
        [PrimaryKey, AutoIncrement]
        public int IdConfigs { get; set; }
        [NotNull]
        public string DefaultCurrencyCode { get; set; }
        [NotNull]
        public string CurrentLanguageId { get; set; }
        [NotNull]
        public bool IsUserOnboarding { get; set; }
    }
}
