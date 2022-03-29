using AutoMapper;
using Series.API.Dto;
using Series.API.Models;
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
        public void CreateSeries(CreateSeriesDto series)
        {
            var db = _redis.GetDatabase();
            var json = JsonSerializer.Serialize(series);

            db.HashSet(RedisKeyName, new HashEntry[] {
                new HashEntry(series.Id, json)
            });
        }

        public void DeleteSeries(string id)
        {
            var db = _redis.GetDatabase();

            db.HashDelete(RedisKeyName, id);
        }

        public IEnumerable<SeriesModel?>? GetAllSeries()
        {
            var db = _redis.GetDatabase();
            var json = db.HashGetAll(RedisKeyName);
            if(json.Length > 0)
            {
                var obj = Array.ConvertAll(json, val => JsonSerializer.Deserialize<SeriesModel>(val.Value)).ToList();
                return obj;
            }

            return null;
            
        }

        public SeriesModel? GetSeriesById(string id)
        {
            var db = _redis.GetDatabase();
            var json = db.HashGet(RedisKeyName, id);

            var series = JsonSerializer.Deserialize<SeriesModel>(json);

            return series;
        }

        //same as Create except we dont use dto
        public void UpdateSeries(SeriesModel series)
        {
            var db = _redis.GetDatabase();
            var json = JsonSerializer.Serialize<SeriesModel>(series);

            db.HashSet(RedisKeyName, new HashEntry[]
            {
                new HashEntry(series.Id, json),
            });
        }
    }
}
