using Listup.Models;
using Listup.Repositories;
using System.Threading.Tasks;

namespace Listup.Helpers
{
    public class CalculateTotal
    {
        private readonly CartRepository _cartRepository = new CartRepository();
        private Cart _cart;

        public CalculateTotal(Cart cart)
        {
            _cart = cart;
        }

        public async Task<double> GetGrandTotalAsync()
        {
            return await _cartRepository.GetTotalAsync(_cart);
        }

        public double GetCartItemTotal(double price, double quantity)
        {
            return price * quantity;
        }
    }
}
