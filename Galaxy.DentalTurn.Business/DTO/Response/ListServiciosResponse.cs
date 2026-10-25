namespace Galaxy.DentalTurn.Business.DTO.Response
{
    public class ListServiciosResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int DurationMin { get; set; }
        public bool Active { get; set; }
    }
}
