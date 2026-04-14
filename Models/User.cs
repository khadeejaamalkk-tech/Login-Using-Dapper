namespace LoginRegistrationUsingDapper.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserRoleId { get; set; }
        public bool IsActive { get; set; }


    }
}
