namespace Galaxy.DentalTurn.Business.DTO.Request.Practitioner
{
    public class UpdatePractitionerRequest
    {
        public string Name { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
        public List<int> ServiceIds { get; set; } = new();
    }
}
