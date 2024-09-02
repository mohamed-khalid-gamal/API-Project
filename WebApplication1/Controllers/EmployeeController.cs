using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // help in binding
    // binding in API if the parameters are premetive types (int , string , ....) he git it from the route segment => "/ id /"
    // , if not exist he search in query string => "?id=1" if not both will put default values
    //binding in API if the parameters are Complex types will search in the request body
    public class EmployeeController : ControllerBase
    {
        ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetEmployees()
        {
            var emps = _context.employees
                .Include(e=>e.Department)
                .Select(e=>new EmployeeWithDepartmentDataDTO
                {
                    DepartmentName=e.Department.Name,
                    DepartmentManager=e.Department.Manager,
                    EmployeeName=e.Name,
                    Id=e.Id,
                    grade=e.grade
                })
                .ToList();
            return Ok(emps);
        }
        [HttpGet("{id:int}",Name = "GetEmployeeById")]
        public IActionResult GetEmployeeById(int id)
        {
            var emp = _context.employees
                .Include(e => e.Department)
                .Select(e => new EmployeeWithDepartmentDataDTO
                {
                    DepartmentName = e.Department.Name,
                    DepartmentManager = e.Department.Manager,
                    EmployeeName = e.Name,
                    Id = e.Id,
                    grade = e.grade
                })
                .FirstOrDefault(e=>e.Id == id);
            return Ok(emp);
        }
        [HttpPost]

        public IActionResult Post(EmpDTOPost Emp)
        {
            var employee = new Employee()
            {
                Name = Emp.Name,
                grade = Emp.grade,
                DepId = Emp.DepId,
                Department = _context.departments.FirstOrDefault(e => e.Id == Emp.DepId)
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(modelState: ModelState);
            }

            _context.employees.Add(employee);
            _context.SaveChanges();
            /*return Ok("Saved");*/ // 200
            //return Created($"http://localhost:5297/api/Department/{dept.Id}", dept); // 201
            var link = Url.Link("GetEmployeeById", new { id = employee.Id });
            var GetDto = new EmployeeWithDepartmentDataDTO()
            {
                Id = employee.Id,
                DepartmentName = employee.Department.Name,
                DepartmentManager = employee.Department.Manager,
                EmployeeName = employee.Name,
                grade = employee.grade
            };
            return Created(link, GetDto);
        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] EmpDTOPost Emp)
        {
            if (ModelState.IsValid)
            {
                var oldEmp = _context.employees.FirstOrDefault(x => x.Id == id);
                oldEmp.Name = Emp.Name;
                oldEmp.grade = Emp.grade;
                oldEmp.DepId = Emp.DepId;
                oldEmp.Department = _context.departments.FirstOrDefault(x => x.Id == Emp.DepId);
                _context.employees.Update(oldEmp);
                _context.SaveChanges();
                var GetDto = new EmployeeWithDepartmentDataDTO()
                {
                    Id = oldEmp.Id,
                    DepartmentName = oldEmp.Department.Name,
                    DepartmentManager = oldEmp.Department.Manager,
                    EmployeeName = oldEmp.Name,
                    grade = oldEmp.grade
                };
                return StatusCode(204, GetDto);
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var emp = _context.employees.FirstOrDefault(e => e.Id == id);
            if (emp != null)
            {
                try
                {
                    _context.employees.Remove(emp);
                    _context.SaveChanges();
                    return StatusCode(204, "Removed with Success");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Invalid ID");
        }

    }
}
