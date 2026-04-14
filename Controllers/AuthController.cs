using LoginRegistrationUsingDapper.Repositeries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoginRegistrationUsingDapper.Models;
using LoginRegistrationUsingDapper.DTOs;
using LoginRegistrationUsingDapper.Common;

namespace LoginRegistrationUsingDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _repo;
        public AuthController(AuthRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                var result = await _repo.Register(user);
                return StatusCode(201,
                    ResponseHelper.Success(result, "User Registered successfully", 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    ResponseHelper.Failure<String>($"Error: {ex.Message}", 500));
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            try
            {
                var user = await _repo.Login(model.Username, model.Password);
                if (user == null)
                    return StatusCode(401,
                            ResponseHelper.Failure<string>("Invalid credentials", 401));
                return Ok(ResponseHelper.Success(user, "Login Successfully"));
            }
            catch (Exception ex)
            {

                return StatusCode(500,
                    ResponseHelper.Failure<string>($"Error: {ex.Message}", 500));
            }
        }
            [HttpGet("get-user-full/{id}")]
            public async Task<IActionResult> GetUserFull(int id)
            {
                try
                {
                    var result = await _repo.GetUser(id);

                    if (result == null)
                        return StatusCode(404,
                            ResponseHelper.Failure<User>("User not found", 404));

                    return Ok(ResponseHelper.Success(result, "User fetched successfully"));
                }
                catch (Exception ex)
                {
                    return StatusCode(500,
                        ResponseHelper.Failure<string>($"Error: {ex.Message}", 500));
                }
            }
            [HttpGet("get-all-users-full")]
            public async Task<IActionResult> GetAllUsersFull()
            {
                try
                {
                    var users = await _repo.GetAllUsersFull();

                    return Ok(ResponseHelper.Success(users, "Users fetched successfully"));
                }
                catch (Exception ex)
                {
                    return StatusCode(500,
                        ResponseHelper.Failure<string>($"Error: {ex.Message}", 500));
                }
            }
            [HttpPut("UpdateUser/{id}")]
            public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateDto model)
            {
                try
                {
                    await _repo.UpdateUser(id, model);

                    return Ok(ResponseHelper.Success("Updated", "User updated successfully"));
                }
                catch (Exception ex)
                {
                    return StatusCode(500,
                        ResponseHelper.Failure<string>($"Error: {ex.Message}", 500));
                }
            }
            [HttpDelete("delete-user/{id}")]
            public async Task<IActionResult> DeleteUser(int id)
            {
                try
                {
                    var result = await _repo.DeleteUser(id);

                    if (result == 0)
                        return StatusCode(404,
                            ResponseHelper.Failure<string>("User not found", 404));

                    return Ok(ResponseHelper.Success("Deleted", "User deactivated successfully"));
                }
                catch (Exception ex)
                {
                    return StatusCode(500,
                        ResponseHelper.Failure<string>($"Error: {ex.Message}", 500));
                }
            }


        }
    }

