using SQLite;
using System;

namespace Listup.Models
{
    public class Cart
    {
        [PrimaryKey, AutoIncrement]
        public int IdCart { get; set; }
        [NotNull]
        public string Title { get; set; }
        public string Description { get; set; }
        [NotNull]
        public DateTime EditedDate { get; set; }
        [NotNull]
        public DateTime CreationDate { get; set; }
        [NotNull]
        public string AddMode { get; set; } //its value can be "cart" or "list".
        [NotNull]
        public string Currency { get; set; }
    }
}
