using App.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Application.AdminArea.Users.Queries
{
    public class ListUsersHandler : IRequestHandler<ListUsersQuery, List<ListUsersResponse>>
    {
        private readonly IAppDbContext _dbContext;
        public ListUsersHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ListUsersResponse>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            var agents = await _dbContext.Profiles.Include(i => i.UserRef)
                                                  .Select(s => new ListUsersResponse
                                                  {
                                                      Id = s.Id,
                                                      FullName = s.Suffix + " " + s.FirstName + " " + s.LastName,
                                                      Email = s.UserRef.Email
                                                  })
                                                  .ToListAsync(cancellationToken);
            return agents;
        }
    }

    public class ListUsersQuery : IRequest<List<ListUsersResponse>> { }

    public class ListUsersResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
