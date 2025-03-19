namespace ClubAPI.Models
{
    public class Booking
    {
        public int FieldPartId { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public string status { get; set; } = "";
        public int UserId { get; set; } = 0;
    }
}
