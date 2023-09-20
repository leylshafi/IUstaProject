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
    public class WorkerController : ControllerBase
    {
        private readonly ILoginRegisterService _loginRegister;
        private readonly WorkerDbContext workerDbContext;

        public WorkerController(ILoginRegisterService loginRegister, WorkerDbContext workerDbContext)
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

        // Add Categoriesss

        //[HttpPost("addcategories/{workerId}")]
        //public IActionResult AddCategories(Guid workerId, [FromBody] List<string> categoryNames)
        //{
        //    try
        //    {
        //        // Retrieve the worker based on the workerId
        //        var worker = workerDbContext.Workers.FirstOrDefault(w => w.Id == workerId);
        //        if (worker == null)
        //            return NotFound($"Worker with ID {workerId} not found");

        //        // Add categories to the worker
        //        foreach (var categoryName in categoryNames)
        //        {
        //            var category = new Category { CategoryName = categoryName };
        //            worker.Categories.Add(category);
        //        }

        //        workerDbContext.SaveChanges();
        //        return Ok("Categories added successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost("addcategory/{workerId}")]
        public IActionResult AddCategory(Guid workerId, [FromBody] string categoryName)
        {
            try
            {
                var worker = workerDbContext.Workers.FirstOrDefault(w => w.Id == workerId);
                if (worker == null)
                    return NotFound($"Worker with ID {workerId} not found");

                var category = workerDbContext.Categories.FirstOrDefault(c=>c.CategoryName==categoryName);
                if(category == null)
                {
                    category = new Category()
                    {
                        Id = Guid.NewGuid(),
                        CategoryName = categoryName
                    };
                    workerDbContext.Categories.Add(category);
                    workerDbContext.SaveChanges();
                }

                worker.CategoryId = category.Id;

                workerDbContext.SaveChanges();
                return Ok($"Category '{categoryName}' added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("profile")]
        public IActionResult GetProfile(Guid workerId)
        {
            try
            {
                var worker = workerDbContext.Workers.FirstOrDefault(c => c.Id == workerId);

                var workerName = worker.UserName;

                var profile = new
                {
                    WorkerName = workerName,
                };

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("agreement")]
        public IActionResult AgreeWithCustomer([FromBody] AgreementRequest agreementRequest)
        {
            try
            {
                var customer = workerDbContext.Customers.FirstOrDefault(c => c.Id == agreementRequest.CustomerId);
                if (customer == null)
                    return NotFound($"Customer with ID {agreementRequest.CustomerId} not found");

                var worker = workerDbContext.Workers.FirstOrDefault(w => w.Id == agreementRequest.WorkerId);
                if (worker == null)
                    return NotFound($"Worker with ID {agreementRequest.WorkerId} not found");

                var agreement = new Agreement
                {
                    CustomerId = customer.Id,
                    WorkerId = worker.Id,
                    AgreementText = agreementRequest.AgreementText,
                };

                workerDbContext.Agreements.Add(agreement);
                workerDbContext.SaveChanges();

                return Ok("Agreement with customer successfully recorded");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
