using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Hosting;

namespace ClubAPI.Models
{
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Facilities { get; set; }
        public string DeckType { get; set; }
        public bool HasLighting { get; set; }
        public bool HasHeating { get; set; }

        public int clubId { get; set; }
        public Club club { get; set; } = null!;
        public ICollection<Part> Parts { get; } = new List<Part>();
    }

    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool AutoApprove { get; set; }
        public int FieldId { get; set; }
        public Field field { get; set; } = null!;
        public ICollection<Booking> booking { get; set; } = new List<Booking>();
    }
}
