using Dapper;
using LoginRegistrationUsingDapper.Data;
using LoginRegistrationUsingDapper.Models;
namespace LoginRegistrationUsingDapper.Repositeries
{
    public class RoleRepository
    {
        private readonly DapperContext _context;
        public RoleRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task AddRole(UserRole role)
        {
            var query = "INSERT INTO UserRoles (RoleName, IsActive) VALUES (@RoleName, @IsActive)";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, role);
            }
        }
        
    }
}
