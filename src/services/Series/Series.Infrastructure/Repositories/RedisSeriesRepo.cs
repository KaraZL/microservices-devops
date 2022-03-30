using Series.Domain.Entites;
using StackExchange.Redis;
using System.Text.Json;

namespace Series.API.Data
{
    public class RedisSeriesRepo : ISeriesRepo
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly string RedisKeyName = "hashseries";

        public RedisSeriesRepo(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public async Task CreateSeries(SeriesModel series)
        {
            var db = _redis.GetDatabase();
            var json = JsonSerializer.Serialize(series);

            await db.HashSetAsync(RedisKeyName, new HashEntry[] {
                new HashEntry(series.Id, json)
            });
        }

        public async Task DeleteSeries(string id)
        {
            var db = _redis.GetDatabase();

            await db.HashDeleteAsync(RedisKeyName, id);
        }

        public async Task<IEnumerable<SeriesModel?>?> GetAllSeries()
        {
            var db = _redis.GetDatabase();
            var json = await db.HashGetAllAsync(RedisKeyName);
            if(json.Length > 0)
            {
                var obj = Array.ConvertAll(json, val => JsonSerializer.Deserialize<SeriesModel>(val.Value)).ToList();
                return obj;
            }

            return null;
            
        }

        public async Task<SeriesModel?> GetSeriesById(string id)
        {
            var db = _redis.GetDatabase();
            var json = await db.HashGetAsync(RedisKeyName, id);

            var series = JsonSerializer.Deserialize<SeriesModel>(json);

            return series;
        }

        //same as Create except we dont use dto
        public async Task UpdateSeries(SeriesModel series)
        {
            var db = _redis.GetDatabase();
            var json = JsonSerializer.Serialize<SeriesModel>(series);

            await db.HashSetAsync(RedisKeyName, new HashEntry[]
            {
                new HashEntry(series.Id, json),
            });
        }
    }
}
