using Dapper;
using LoginRegistrationUsingDapper.Data;
using LoginRegistrationUsingDapper.Models;

namespace LoginRegistrationUsingDapper.Repositeries
{
    public class MenuRepository
    {
        private readonly DapperContext _context;
        public MenuRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> AddMenu(Menu menu)
        {
            var query = "INSERT INTO Menus (Name) VALUES (@Name)";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, menu);
            }
        }
        
        public async Task AssignMenusToRole(int roleid, List<int> menuIds)
        {
            using (var connection= _context.CreateConnection())
            {
                var deleteQuery = "DELETE FROM MenuPermissions WHERE UserRoleid = @roleId";
                await connection.ExecuteAsync(deleteQuery, new { roleid });
                foreach (var menuId in menuIds)
                {
                    var insertQuery = @"INSERT INTO MenuPermissions (UserRoleid, MenuId)
                                       VALUES (@UserRoleId, @Menuid)";
                    await connection.ExecuteAsync(insertQuery, new
                    {
                        UserRoleId = roleid,
                        menuId = menuId

                    });
                }
            }
        }
        public async Task<List<string>> GetMenuByRole(int roleId)
        {
            var query = @"SELECT m.Name
                          FROM Menus m
                          INNER JOIN MenuPermissions mp ON m.Id = mp.MenuID
                          WHERE mp.UserRoleId = @roleId";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<string>(query, new { roleId });
                return result.ToList();
            }
        }
    }
}
