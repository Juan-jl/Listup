using Listup.Models;
using Listup.SQLiteData;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Listup.Repositories
{
    public class CartItemRepository
    {
        private readonly SQLiteAsyncConnection _db;

        public CartItemRepository()
        {
            _db = DatabaseConnection.GetConnection();
        }

        public async Task<List<CartItem>> GetByCartIdAsync(int cartId)
        {
            return await _db.Table<CartItem>().Where(x => x.IdCartFK == cartId).OrderByDescending(x => x.IdCartItem).ToListAsync();
        }

        public async Task<int> InsertAsync(CartItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
                item.Name = "Item sem nome";

            item.Name = item.Name.Trim();
            return await _db.InsertAsync(item);
        }
        public async Task<int> UpdateAsync(CartItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
                item.Name = "Item sem nome";

            item.Name = item.Name.Trim();
            return await _db.UpdateAsync(item);
        }

        public async Task<int> DeleteAsync(CartItem item)
        {
            return await _db.DeleteAsync(item);
        }
    }
}
