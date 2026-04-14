using Dapper;
using LoginRegistrationUsingDapper.Data;
using LoginRegistrationUsingDapper.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LoginRegistrationUsingDapper.DTOs;


namespace LoginRegistrationUsingDapper.Repositeries
{
    public class AuthRepository
    {
        private readonly DapperContext _context;
        public AuthRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> Register(User user)
        {
            var query = @"INSERT INTO Users (Username, Password,UserRoleId,IsActive)
                       VALUES (@Username, @Password,@UserRoleId,@IsActive)";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, user);

            }
        }
        public async Task<LoginResponseDto> Login(string Username, string Password)
        {
            using (var connection = _context.CreateConnection())
            {
              
                var userQuery = @"SELECT * FROM Users
                          WHERE Username = @Username 
                          AND Password = @Password 
                          AND IsActive = 1";

                var user = await connection.QueryFirstOrDefaultAsync<User>(
                    userQuery, new { Username,  Password });

                if (user == null)
                    return null;

                
                var roleQuery = @"SELECT * FROM UserRoles WHERE Id = @RoleId";

                var role = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    roleQuery, new { RoleId = user.UserRoleId });

                
                var menuQuery = @"SELECT m.Name 
                          FROM Menus m
                          INNER JOIN MenuPermissions mp ON m.Id = mp.MenuId
                          WHERE mp.UserRoleId = @RoleId";

                var menus = await connection.QueryAsync<string>(
                    menuQuery, new { RoleId = user.UserRoleId });

                
                return new LoginResponseDto
                {
                    Username = user.Username,

                    Role = new
                    {
                        user.Id,
                        user.Username,
                        user.IsActive,
                        user.UserRoleId
                    },
                    Menus = menus.ToList()
                };
            }
        }
        public async Task<object> GetUser(int id)
        {

            using (var connection = _context.CreateConnection())
            {
                var userQuery = @"
                     SELECT 
                           u.Id,
                           u.Username,
                           u.Password,
                           u.UserRoleId,
                           u.IsActive,
                           r.RoleName
                     FROM Users u
                     INNER JOIN UserRoles r ON u.UserRoleId = r.UserRoleId
                     WHERE u.Id = @Id";
                var user = await connection.QueryFirstOrDefaultAsync(userQuery, new { id });
                if (user == null)
                {
                    return null;
                }
                var menuQuery = @"SELECT m.name FROM Menus m INNER JOIN MenuPermissions mp ON m.id = mp.MenuId WHERE mp.UserRoleId = @RoleId";
                var menus = await connection.QueryAsync(menuQuery, new { RoleId = user.UserRoleId });
                return new
                {
                    id = user.Id,
                    username = user.Username,
                    password = user.Password,
                    userRoleId = user.UserRoleId,
                    isActive = user.IsActive,
                    role = user.RoleName,
                    menus = menus
                };
            }
        }
        public async Task<IEnumerable<object>> GetAllUsersFull()
        {
            using (var connection = _context.CreateConnection())
            {
                
                var userQuery = @"
            SELECT 
                u.Id,
                u.Username,
                u.UserRoleId,
                u.IsActive,
                r.RoleName
            FROM Users u
            INNER JOIN UserRoles r ON u.UserRoleId = r.UserRoleId";

                var users = await connection.QueryAsync(userQuery);

                var result = new List<object>();

                foreach (var user in users)
                {
                    
                    var menuQuery = @"
                SELECT m.Name
                FROM Menus m
                INNER JOIN MenuPermissions mp ON m.Id = mp.MenuId
                WHERE mp.UserRoleId = @RoleId";

                    var menus = await connection.QueryAsync<string>(
                        menuQuery,
                        new { RoleId = user.UserRoleId }
                    );

                    result.Add(new
                    {
                        id = user.Id,
                        username = user.Username,
                        userRoleId = user.UserRoleId,
                        isActive = user.IsActive,
                        role = user.RoleName,
                        menus = menus
                    });
                }

                return result;
            }
        }
        public async Task UpdateUser(int id, UpdateDto model)
        {
            using (var connection = _context.CreateConnection())
            {
                
                var userQuery = @"
            UPDATE Users
            SET Username = @Username,
                Password = @Password,
                UserRoleId = @UserRoleId,
                IsActive = @IsActive
            WHERE Id = @Id";

                await connection.ExecuteAsync(userQuery, new
                {
                    Id = id,
                    model.Username,
                    model.Password,
                    model.UserRoleId,
                    model.IsActive
                });

                
                var deleteQuery = "DELETE FROM MenuPermissions WHERE UserRoleId = @RoleId";
                await connection.ExecuteAsync(deleteQuery, new { RoleId = model.UserRoleId });

               
                foreach (var menuId in model.MenuIds)
                {
                    var insertQuery = @"
                INSERT INTO MenuPermissions (UserRoleId, MenuId)
                VALUES (@UserRoleId, @MenuId)";

                    await connection.ExecuteAsync(insertQuery, new
                    {
                        UserRoleId = model.UserRoleId,
                        MenuId = menuId
                    });
                }
            }
        }
        public async Task<int> DeleteUser(int id)
        {
            var query = @"
        UPDATE Users
        SET IsActive = 0
        WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }


    }
}
