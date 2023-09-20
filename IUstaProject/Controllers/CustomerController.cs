using IUstaProject.Data;
using IUstaProject.Models;
using IUstaProject.Models.Dtos;
using IUstaProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IUstaProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly ILoginRegisterService _loginRegister;
        private readonly WorkerDbContext workerDbContext;

        public CustomerController(ILoginRegisterService loginRegister, WorkerDbContext workerDbContext)
        {
            this._loginRegister = loginRegister;
            this.workerDbContext = workerDbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CustomerDto userDto)
        {
            try
            {
                var token = await _loginRegister.Login(userDto);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomerDto userDto)
        {
            try
            {
                if (await _loginRegister.Register(userDto))
                    return Ok();
                throw new Exception("Something is wrong!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get worker")]
        public async Task<Worker> GetWorker(string id)
        {
            return await workerDbContext.Workers.FindAsync(id);
        }
    }
}
