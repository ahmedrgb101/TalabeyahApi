using TalabeyahTaskApi.Application.Identity.Roles;
using TalabeyahTaskApi.Application.Identity.Users;

namespace TalabeyahTaskApi.Application.Dashboard;
public class GetStatsRequest : IRequest<StatsDto>
{
}

public class GetStatsRequestHandler : IRequestHandler<GetStatsRequest, StatsDto>
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IStringLocalizer<GetStatsRequestHandler> _localizer;
    private readonly IJobService _jobService;
    private readonly IRepository<Ticket> _repository;

    public GetStatsRequestHandler(IUserService userService, IRoleService roleService, IJobService jobService, IRepository<Ticket> repository, IStringLocalizer<GetStatsRequestHandler> localizer)
    {
        _userService = userService;
        _roleService = roleService;
        _jobService = jobService;
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<StatsDto> Handle(GetStatsRequest request, CancellationToken cancellationToken)
    {
        var stats = new StatsDto
        {
            UserCount = await _userService.GetCountAsync(cancellationToken),
            RoleCount = await _roleService.GetCountAsync(cancellationToken)
        };

        int selectedYear = DateTime.Now.Year;
        double[] productsFigure = new double[13];
        double[] brandsFigure = new double[13];
        for (int i = 1; i <= 12; i++)
        {
            int month = i;
            var filterStartDate = new DateTime(selectedYear, month, 01);
            var filterEndDate = new DateTime(selectedYear, month, DateTime.DaysInMonth(selectedYear, month), 23, 59, 59); // Monthly Based
        }

        stats.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Products"], Data = productsFigure });
        stats.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Brands"], Data = brandsFigure });

        // Schedule methode to handle tickets
        _jobService.Schedule(() => HandleTickets(), TimeSpan.FromMinutes(60));

        return stats;
    }

    /// <summary>
    /// Handle tickets that is created 60 min ago.
    /// </summary>
    public async Task HandleTickets()
    {
        var tickets = _repository.Entities.AsEnumerable().Where(x => !x.IsHandled && (DateTime.Now - x.CreatedOn).TotalMinutes > 1).ToList();
        foreach (var ticket in tickets)
        {
            ticket.Update(true);
            await _repository.UpdateAsync(ticket);
        }

        // Schedule methode to handle tickets
        _jobService.Schedule(() => HandleTickets(), TimeSpan.FromMinutes(60));
    }

}