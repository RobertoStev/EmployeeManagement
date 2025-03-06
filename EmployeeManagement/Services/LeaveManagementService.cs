using EmployeeManagement.Data;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeManagement.Services
{
    public class LeaveManagementService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;
        private readonly ILogger<LeaveManagementService> _logger;
        private readonly IMemoryCache _cache;

        public LeaveManagementService(IServiceScopeFactory scopeFactory, ILogger<LeaveManagementService> logger, IMemoryCache cache)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _cache = cache;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateLeaveBalances, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private void UpdateLeaveBalances(object state)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var employees = context.Employees.ToList();
                //var now = DateTime.Now;
                var now = DateTime.Today;

                foreach (var employee in employees)
                {
                    bool employeeUpdated = false;

                    if (employee.FirstPartLeaveExpiry.HasValue && employee.FirstPartLeaveExpiry.Value < now)
                    {
                        employee.AnnualLeaveDaysRemaining = Math.Min(employee.AnnualLeaveDaysRemaining, 11);
                        employee.BonusLeaveDaysRemaining = 0;
                        employee.FirstPartLeaveExpiry = null;
                        employeeUpdated = true;                       

                        _logger.LogInformation("First part expired for employee with id = {Id}", employee.EmployeeId);
                    
                    }

                    if (employee.SecondPartLeaveExpiry.HasValue && employee.SecondPartLeaveExpiry.Value < now)
                    {
                        employee.AnnualLeaveDaysRemaining = 0;
                        employee.SecondPartLeaveExpiry = null;
                        employeeUpdated = true;

                        _logger.LogInformation("Second part expired for employee with id = {Id}", employee.EmployeeId);
                
                    }

                    if (employeeUpdated)
                    {
                        _cache.Remove("AllLeaveRequests");

                        context.Employees.Update(employee);
                        context.SaveChanges();
                    }
  
                }   
            }
            catch (Exception ex)
            {
                // Add logging here
                _logger.LogError(ex, "Error updating leave balances");
            }
            
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
