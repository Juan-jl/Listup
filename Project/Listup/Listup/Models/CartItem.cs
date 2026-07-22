using SQLite;
using System.ComponentModel;
using Xamarin.Forms;

namespace Listup.Models
{
    public class CartItem : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int IdCartItem { get; set; }

        [NotNull]
        public int IdCartFK { get; set; }

        [NotNull]
        public string Name { get; set; }

        public double Price { get; set; }

        private string _displayPrice;
        [Ignore]
        public string DisplayPrice
        {
            get => _displayPrice;
            set
            {
                if (_displayPrice != value)
                {
                    _displayPrice = value;
                    OnPropertyChanged(nameof(DisplayPrice));
                }
            }
        }

        private int? _quantity;

        public int? Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        [NotNull]
        public bool IsInCart { get; set; }

        private string _displayTotalPrice;

        [Ignore]
        public string DisplayTotalPrice
        {
            get => _displayTotalPrice;
            set
            {
                if (_displayTotalPrice != value)
                {
                    _displayTotalPrice = value;
                    OnPropertyChanged(nameof(DisplayTotalPrice));
                }
            }
        }

        private Color _bgColorCartItemConteiner;

        [Ignore]
        public Color BgColorCartItemConteiner
        {
            get => _bgColorCartItemConteiner;
            set
            {
                if (_bgColorCartItemConteiner != value)
                {
                    _bgColorCartItemConteiner = value;
                    OnPropertyChanged(nameof(BgColorCartItemConteiner));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}