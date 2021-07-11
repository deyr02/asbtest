using System.Collections.Generic;
using MediatR;
using Domain;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Core;

namespace Application.CreditCards
{
    public class List
    {
        public class Query : IRequest<Result<List<Domain.CreditCard>>> { }

        public class Handler : IRequestHandler<Query, Result<List<Domain.CreditCard>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<List<Domain.CreditCard>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<Domain.CreditCard>>.Success(await _context.CreditCards.ToListAsync(cancellationToken));
            }
        }
    }
}