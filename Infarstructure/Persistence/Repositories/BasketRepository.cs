using Domain.Contracts;
using Domain.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();


        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var redisvalue = await _database.StringGetAsync(id);
            if (redisvalue.IsNullOrEmpty) return null;
            var basket = JsonSerializer.Deserialize<CustomerBasket>(redisvalue);
            if (basket is null) return null;
            return basket;
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            var redisvalue = JsonSerializer.Serialize<CustomerBasket>(basket);
            var flag = _database.StringSetAsync(basket.Id, redisvalue, TimeSpan.FromDays(30));
            if (flag is null) return null;
            return await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }


    }
}
