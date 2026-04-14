using LoginRegistrationUsingDapper.Common;
using LoginRegistrationUsingDapper.Models;
using LoginRegistrationUsingDapper.Repositeries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoginRegistrationUsingDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleRepository _repo;
        public RoleController(RoleRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("Add Role")]
        public async Task<IActionResult> AddRole(UserRole role)
        {
            await _repo.AddRole(role);
            return Ok(ResponseHelper.Success(role,"role added"));
        }
        
    }
}
