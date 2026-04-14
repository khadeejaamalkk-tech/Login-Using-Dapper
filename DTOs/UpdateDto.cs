namespace LoginRegistrationUsingDapper.DTOs
{
    public class UpdateDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserRoleId { get; set; }
        public bool IsActive { get; set; }
        public List<int> MenuIds { get; set; }
    }
}
