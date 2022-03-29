namespace Series.API.Dto
{
    public class CreateSeriesDto
    {
        public string Id { get; set; } = $"series:{Guid.NewGuid()}";
        public string Name { get; set; } = string.Empty;
        public DateTime DatePublished { get; set; }
        public bool IsCompleted { get; set; }
    }
}
