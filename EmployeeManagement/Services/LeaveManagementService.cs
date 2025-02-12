using EmployeeManagement.Data;

namespace EmployeeManagement.Services
{
    public class LeaveManagementService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;
        public LeaveManagementService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateLeaveBalances, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private void UpdateLeaveBalances(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var employees = context.Employees.ToList();
                var now = DateTime.Now;

                foreach (var employee in employees)
                {

                    if (employee.FirstPartLeaveExpiry.HasValue && employee.FirstPartLeaveExpiry.Value < now)
                    {
                        employee.AnnualLeaveDaysRemaining -= Math.Min(employee.AnnualLeaveDaysRemaining, 11);
                        employee.BonusLeaveDaysRemaining = 0;
                        employee.FirstPartLeaveExpiry = null;

                    }

                    if (employee.SecondPartLeaveExpiry.HasValue && employee.SecondPartLeaveExpiry.Value < now)
                    {
                        employee.AnnualLeaveDaysRemaining = 0;
                        employee.SecondPartLeaveExpiry = null;
                    }
                    context.Employees.Update(employee);
                    context.SaveChanges();
                }
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

