using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Classes
{
    public class SimpleMemoryCache<TItem>
    {
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public TItem GetOrCreate(object key, Func<TItem> createItem)
        {
            TItem cacheEntry;
            if (!_cache.TryGetValue(key, out cacheEntry)) // Ищем ключ в кэше.
            {
                // Ключ отсутствует в кэше, поэтому получаем данные.
                cacheEntry = createItem();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1)//Значение размера
                           // Приоритет на удаление при достижении предельного размера (давления на память)
               .SetPriority(CacheItemPriority.High);
                
                // Сохраняем данные в кэше. 
                _cache.Set(key, cacheEntry);
            }
            return cacheEntry;
        }
    }
}
