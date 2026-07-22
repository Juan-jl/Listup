using Listup.Models;
using Listup.SQLiteData;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Listup.Repositories
{
    public enum CartOrderBy
    {
        TitleAsc,
        TitleDesc,
        EditedDate,
        CreationDate
    }

    public class CartRepository
    {
        private readonly SQLiteAsyncConnection _db;

        public CartRepository()
        {
            _db = DatabaseConnection.GetConnection();
        }

        public async Task<List<Cart>> GetAllAsync(CartOrderBy orderBy = CartOrderBy.CreationDate)
        {
            var list = await _db.Table<Cart>().ToListAsync();

            List<Cart> orderedList = null;

            switch (orderBy)
            {
                case CartOrderBy.TitleAsc:
                    orderedList = list.OrderBy(c => c.Title).ToList();
                    break;
                case CartOrderBy.TitleDesc:
                    orderedList = list.OrderByDescending(c => c.Title).ToList();
                    break;
                case CartOrderBy.EditedDate:
                    orderedList = list.OrderByDescending(c => c.EditedDate).ToList();
                    break;
                case CartOrderBy.CreationDate:
                    orderedList = list.OrderByDescending(c => c.CreationDate).ToList();
                    break;
                default:
                    orderedList = list.OrderByDescending(c => c.EditedDate).ToList();
                    break;
            }

            return orderedList;
        }

        public Task<int> InsertAsync(Cart cart)
        {
            return _db.InsertAsync(cart);
        }

        public Task<int> UpdateAsync(Cart cart)
        {
            if (string.IsNullOrWhiteSpace(cart.Title))
                cart.Title = "Compra sem nome";

            cart.Title = cart.Title.Trim();
            return _db.UpdateAsync(cart);
        }

        public async Task<int> SetAllCurrenciesToSameValue(string value)
        {
            // Raw SQL used for fast bulk update of all CartItem records.
            string query = "UPDATE Cart SET Currency = ?";
            return await _db.ExecuteAsync(query, value);
        }

        public Task<int> DeleteAllAsync()
        {
            return _db.DeleteAllAsync<Cart>();
        }

        public async Task<int> DeleteAsync(Cart cart)
        {
            await _db.Table<CartItem>().Where(ci => ci.IdCartFK == cart.IdCart).DeleteAsync();
            return await _db.DeleteAsync(cart);
        }

        public async Task<double> GetTotalAsync(Cart cart)
        {
            var items = await _db.Table<CartItem>().Where(ci => ci.IdCartFK == cart.IdCart && ci.IsInCart).ToListAsync();
            return (double)items.Sum(ci => ci.Price * ci.Quantity);
        }

        public async Task<int> GetItemsOutsideCartCountAsync(Cart cart)
        {
            return await _db.Table<CartItem>().Where(ci => ci.IdCartFK == cart.IdCart && !ci.IsInCart).CountAsync();
        }
    }
}