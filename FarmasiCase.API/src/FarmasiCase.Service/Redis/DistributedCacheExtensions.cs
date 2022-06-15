using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FarmasiCase.Service.Redis
{
    public static class DistributedCacheExtensions
    {
        // Create
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, 
            string recordId,
            T data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime?? TimeSpan.FromMinutes(60); // Expiration time, even if the data is being used or not => We set the Default to 15 minute
            options.SlidingExpiration = unusedExpireTime; // If the data doesn't get 'used' in this time frame, it will expire => We set the Default to null (no expiration)

            var jsonData = JsonSerializer.Serialize(data); // We take generic data, but whatever it is, we turn it to Json
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        // Get
        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData is null)
            {
                return default(T);
            }
            
            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
