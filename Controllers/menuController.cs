using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoginRegistrationUsingDapper.Repositeries;
using LoginRegistrationUsingDapper.Models;
using LoginRegistrationUsingDapper.Common;

namespace LoginRegistrationUsingDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class menuController : ControllerBase
    {
        private readonly MenuRepository _repo;
        public menuController(MenuRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("Add menu")]
        public async Task<IActionResult> AddMenu(Menu menu)
        {
            try
            {
                await _repo.AddMenu(menu);
                return StatusCode(500,
                        ResponseHelper.Success(menu, "Menu Added"));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    ResponseHelper.Failure<string>($"Error: {ex.Message}", 500));
            }
        }



        [HttpPost("Assign menu")]
        public async Task<IActionResult> AssignMenu(int roleid, List<int> menuIds)
        {
            try
            {

                await _repo.AssignMenusToRole(roleid, menuIds);
                return Ok(ResponseHelper.Success(menuIds, "Permission assigned"));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    ResponseHelper.Failure<string>($"Error: {ex.Message}", 500));
            }
        }
        
        


    }
}
