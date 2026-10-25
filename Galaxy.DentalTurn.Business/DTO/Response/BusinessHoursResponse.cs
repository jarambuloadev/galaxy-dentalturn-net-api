namespace Galaxy.DentalTurn.Business.DTO.Response
{
    public class BusinessHoursResponse
    {
        public int Id { get; set; }
        public int DayOfWeek { get; set; }
        public bool IsOpen { get; set; }
        public string OpenTime { get; set; } = "09:00";
        public string CloseTime { get; set; } = "18:00";
    }
}
