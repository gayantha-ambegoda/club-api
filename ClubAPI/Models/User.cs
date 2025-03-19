namespace ClubAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

        public List<Club> Clubs { get; set; } = [];

        public List<UserRoles> UserRole { get; } = [];

        public List<Team> Team { get; } = [];
        public List<TeamMember> teamMembers { get; } = [];
    }
}
