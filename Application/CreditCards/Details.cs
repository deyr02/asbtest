using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CreditCards
{
    public class Details
    {
        public class Query : IRequest<Result<Domain.CreditCard>>
        {
            public long CardNumber {get; set;}
        }

        public class Handler : IRequestHandler<Query, Result<Domain.CreditCard>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<CreditCard>> Handle(Query request, CancellationToken cancellationToken)
            {
                var card = await _context.CreditCards.FirstOrDefaultAsync(x => x.CardNumber == request.CardNumber);
                return Result<Domain.CreditCard>.Success(card);
            }
        }


    }
}