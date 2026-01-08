
using EmployeeManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        //The Context: One object representing the entire database session.
        // The underscore is a C# naming convention used to identify variables that belong to the entire class
        // rather than just one specific method.
        private readonly AppDbContext _context;

        //In this code, the constructor performs Dependency Injection.
        //Its job is to initialize the class with the necessary database context.
        //The logic: The constructor receives the database connection (context)
        //and immediately saves it into the private field (_context)
        //so that every other method in the class (like AddEmployeeAsync) can use it later. 
        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employeeInDb = await _context.Employees.FindAsync(id);

            if (employeeInDb == null)
            {
                throw new KeyNotFoundException($"Employee with {id} was not found.");
            }

            _context.Employees.Remove(employeeInDb);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
