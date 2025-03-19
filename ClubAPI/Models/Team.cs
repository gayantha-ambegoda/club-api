namespace ClubAPI.Models
{
    public class Team
    {
        //name,club,membertype,birthyear
        public int Id { get; set; }
        public string Name { get; set; }
        public string MemberType { get; set; }
        public int BirthYear { get; set; }

        public int clubId { get; set; }
        public Club club { get; set; } = null!;
        public List<User> members { get; } = [];
        public List<TeamMember> teammembers { get; } = [];
    }

    public class TeamMember
    {
        //team,player
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}
