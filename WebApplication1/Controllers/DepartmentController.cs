using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.DTO;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]// help in binding
    [ApiController]
    
    // binding in API if the parameters are premetive types (int , string , ....) he git it from the route segment => "/ id /"
    // , if not exist he search in query string => "?id=1" if not both will put default values
    //binding in API if the parameters are Complex types will search in the request body
    public class DepartmentController : ControllerBase // Remove the parentheses
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject the ApplicationDbContext dependency
        public DepartmentController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        // Action method to get all departments
        [HttpGet]
        [Authorize]
        public IActionResult GetAllDepartments()
        {
            // Fetch departments along with employees from the database
            var departments = _context.departments.Include(e => e.Employees).ToList();

            // Create a list of DepartmentWithEmployeesDataDTO objects
            var depts = departments.Select(e => new DepartmentWithEmplyeesDataDTO
            {
                Name = e.Name,
                Id = e.Id,
                Manager = e.Manager,
                EmployeesName = e.Employees.Select(emp => emp.Name).ToList() // Directly populate the employee names here
            }).ToList();

            return Ok(depts);
        }

        // Action method to get a department by its ID
        [HttpGet]
        [Route("id={id}",Name ="GetDeptByID")]
        public IActionResult GetById(int id)
        {
            var dep = _context.departments.Include(e=>e.Employees).SingleOrDefault(x => x.Id == id);
            var deDTO = new DepartmentWithEmplyeesDataDTO()
            {
                Id = dep.Id,
                Manager = dep.Manager,
                Name = dep.Name,
                EmployeesName = dep.Employees.Select(e => e.Name).ToList()
            };
            if (dep == null)
            {
                return NotFound();
            }
            return Ok(deDTO);
        }
        [HttpGet("{Name:alpha}")]
        public IActionResult GetByName(string Name)
        {
            var dep = _context.departments.Include(e=>e.Employees).SingleOrDefault(x => x.Name == Name);
      
            if (dep == null)
            {
                return NotFound();
            }
            var deDTO = new DepartmentWithEmplyeesDataDTO()
            {
                Id = dep.Id,
                Manager = dep.Manager,
                Name = dep.Name,
                EmployeesName = dep.Employees.Select(e => e.Name).ToList()
            };
            return Ok(deDTO);
        }


        // Other CRUD actions can be added here
        [HttpPost]
        
        public IActionResult Post(DeptDTOPost dept)
        {
            var department = new Department()
            {
                Manager = dept.Manager,
                Name = dept.Name,
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(modelState:ModelState);
            }
            
            _context.departments.Add(department);
            _context.SaveChanges();
            /*return Ok("Saved");*/ // 200
            //return Created($"http://localhost:5297/api/Department/{dept.Id}", dept); // 201
            var link = Url.Link("GetDeptByID", new { id = department.Id });
            return Created(link, department);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute]int id , [FromBody]DeptDTOPost dept)
        {
            if (ModelState.IsValid) { 
                var oldDept = _context.departments.FirstOrDefault(x => x.Id == id);
                oldDept.Name = dept.Name;
                oldDept.Manager = dept.Manager;
                _context.departments.Update(oldDept);
                _context.SaveChanges();
                return  StatusCode(204 ,oldDept);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var dept = _context.departments.FirstOrDefault(e => e.Id == id);
            if (dept != null)
            {
                try
                {
                    _context.departments.Remove(dept);
                    _context.SaveChanges();
                    return StatusCode(204, "Removed with Success");
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Invalid ID");
        }

    }
}
