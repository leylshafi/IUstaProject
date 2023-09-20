using IUstaProject.Data;
using IUstaProject.Models;
using IUstaProject.Models.Dtos;
using IUstaProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IUstaProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILoginRegisterService _loginRegister;
        private readonly WorkerDbContext workerDbContext;

        public AdminController(ILoginRegisterService loginRegister,WorkerDbContext workerDbContext)
        {
            this._loginRegister = loginRegister;
            this.workerDbContext= workerDbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminDto userDto)
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
        public async Task<IActionResult> Register([FromBody] AdminDto userDto)
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

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CategoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = new()
                    {
                        Id = Guid.NewGuid(),
                        CategoryName = model.CategoryName
                    };

                    await workerDbContext.Categories.AddAsync(category);
                    await workerDbContext.SaveChangesAsync();

                    return StatusCode((int)HttpStatusCode.Created);
                }
                return BadRequest(ModelState);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] Guid productId)
        {
            try
            {
                var category = workerDbContext.Categories.FirstOrDefault(p=>p.Id==productId);
                if (category == null)
                {
                    return NotFound();
                }

                workerDbContext.Categories.Remove(category);
                await workerDbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        {
            try
            {
                var customerCount = workerDbContext.Customers.Count();
                var workerCount = workerDbContext.Workers.Count();
                var categoryCount = workerDbContext.Categories.Count();

                var statistics = new
                {
                    CustomerCount = customerCount,
                    WorkerCount = workerCount,
                    CategoryCount = categoryCount
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


    }
}
