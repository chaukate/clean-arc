using App.Application.Common.Interfaces;
using MediatR;

namespace App.Application.AdminArea.Companies.Queries
{
    public class ListCompaniesHandler : IRequestHandler<ListCompaniesQuery, List<ListCompaniesResponse>>
    {
        private readonly IAppDbContext _dbContext;
        public ListCompaniesHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ListCompaniesResponse>> Handle(ListCompaniesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class ListCompaniesQuery : IRequest<List<ListCompaniesResponse>> { }

    public class ListCompaniesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
