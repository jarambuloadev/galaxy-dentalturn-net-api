namespace Galaxy.DentalTurn.Business.DTO.Response
{
    public class ListPractitionersResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
        public List<int> ServiceIds { get; set; } = new();
        public List<PractitionerServiceItemResponse> Services { get; set; } = new();
    }
}
