using IUstaProject.Data;
using IUstaProject.Models.Dtos;
using IUstaProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IUstaProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly ILoginRegisterWorker _loginRegister;
        private readonly WorkerDbContext workerDbContext;

        public WorkerController(ILoginRegisterWorker loginRegister, WorkerDbContext workerDbContext)
        {
            this._loginRegister = loginRegister;
            this.workerDbContext = workerDbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] WorkerDto userDto)
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
        public async Task<IActionResult> Register([FromBody] WorkerDto userDto)
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
    }
}
