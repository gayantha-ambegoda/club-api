namespace ClubAPI.Models
{
    public class SignInRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class SignInResponse
    {
        public string Token { get; set; } = "";
        public string Email { get; set; } = "";
        public int ExpiresIn { get; set; } = 0;
    }

}
