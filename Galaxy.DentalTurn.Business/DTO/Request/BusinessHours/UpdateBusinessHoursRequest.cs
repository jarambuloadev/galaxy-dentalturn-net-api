namespace Galaxy.DentalTurn.Business.DTO.Request.BusinessHours
{
    public class DayHoursItemRequest
    {
        public int DayOfWeek { get; set; }
        public bool IsOpen { get; set; }
        public string OpenTime { get; set; } = "09:00";
        public string CloseTime { get; set; } = "18:00";
    }

    public class UpdateBusinessHoursRequest
    {
        public List<DayHoursItemRequest> Days { get; set; } = new();
    }
}
