namespace Galaxy.DentalTurn.Business.DTO.Response
{
    public class PractitionerServiceItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GetPractitionerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
        public List<int> ServiceIds { get; set; } = new();
        public List<PractitionerServiceItemResponse> Services { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = Environment.UserName;
    }
}
