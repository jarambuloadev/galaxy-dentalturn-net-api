namespace Galaxy.DentalTurn.Business.DTO.Response
{
    public class ListPatientsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool Active { get; set; }
    }
}
