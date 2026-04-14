namespace LoginRegistrationUsingDapper.DTOs
{
    public class LoginResponseDto
    {
        public string Username { get; set; }
        public object Role { get; set; }
        public List<string> Menus { get; set; }
    }
}
