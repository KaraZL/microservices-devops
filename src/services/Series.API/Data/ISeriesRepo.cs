using Series.API.Dto;
using Series.API.Models;

namespace Series.API.Data
{
    public interface ISeriesRepo
    {
        public IEnumerable<SeriesModel?>? GetAllSeries();
        public SeriesModel? GetSeriesById(string id);
        public void CreateSeries(CreateSeriesDto series);
        public void UpdateSeries(SeriesModel series);
        public void DeleteSeries(string id);

    }
}
