
using Series.Domain.Entites;

namespace Series.API.Data
{
    public interface ISeriesRepo
    {
        public Task<IEnumerable<SeriesModel?>?> GetAllSeries();
        public Task<SeriesModel?> GetSeriesById(string id);
        public Task CreateSeries(SeriesModel series);
        public Task UpdateSeries(SeriesModel series);
        public Task DeleteSeries(string id);

    }
}
