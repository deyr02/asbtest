using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CreditCards
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Domain.CreditCard CreditCard { get; set; }
        }

        public class CommandVaildator : AbstractValidator<Command>
        {
            public CommandVaildator()
            {
                RuleFor(x => x.CreditCard).SetValidator(new CreditCardValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var newCard = await _context.CreditCards.FirstOrDefaultAsync(x =>
                x.CardNumber == request.CreditCard.CardNumber);
                if(newCard != null){
                   return Result<Unit>.Failure("The card number is alredy registered");
                }

                _context.CreditCards.Add(request.CreditCard);
                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to create CreditCard");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}