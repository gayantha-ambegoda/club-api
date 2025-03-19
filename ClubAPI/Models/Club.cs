namespace ClubAPI.Models
{
    public class ClubRequest
    {
        public Club club { get; set; } = null!;
        public User user { get; set; } = null!;
    }
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ShortName { get; set; } = "";
        public string Description { get; set; } = "";
        public string Logo { get; set; } = "";
        public string CountryCode { get; set; } = "";
        public int Status { get; set; } = 0;
        public ICollection<Field> Fields { get; } = new List<Field>();
        public ICollection<Team> Teams { get; } = new List<Team>();

        public List<User> Users { get; } = [];

        public List<UserRoles> UserRoles { get; } = [];
    }


    public class UserRoles
    {
        public int UserId { get; set; }
        public int ClubId { get; set; }
        public string Role { get; set; } = "";
    }

    public class UserRoleRequest
    {
        public string UserEmail { get; set; } 
        public int ClubId { get; set; }
        public string Role { get; set; }
    }
}
