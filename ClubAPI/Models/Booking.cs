using Microsoft.VisualBasic.FileIO;

namespace ClubAPI.Models
{
    public class Booking
    {
        public int Id { get; set; } = 0;
        public int FieldPartId { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public string status { get; set; } = "";
        public int UserId { get; set; } = 0;
        public Part part { get; set; } = null!;
        public User user { get; set; }
    }
}
