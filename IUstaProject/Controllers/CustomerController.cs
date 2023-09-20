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
        public  List<Worker> GetWorker(string categoryName)
        {
            List<Worker>FoundedWorkers= new List<Worker>();
            var categoriesFound = workerDbContext.Categories.Where(c => c.CategoryName.Contains(categoryName)).ToList();
            foreach (var item in categoriesFound)
            {
                foreach (var worker in workerDbContext.Workers.ToList())
                {
                    if (worker.CategoryId == item.Id)
                        FoundedWorkers.Add(worker);
                }
            }
            return FoundedWorkers;
        }

        [HttpGet("profile")]
        public IActionResult GetProfile(Guid customerId)
        {
            try
            {
                var customer = workerDbContext.Customers.FirstOrDefault(c => c.Id == customerId);
                var customerName = customer.UserName;
                var profile = new
                {
                    CustomerName = customerName,
                };

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
