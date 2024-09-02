namespace WebApplication1.DTO
{
    public class DepartmentWithEmplyeesDataDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        public List<string> EmployeesName { get; set; } = new List<string>() ;
    }
}
