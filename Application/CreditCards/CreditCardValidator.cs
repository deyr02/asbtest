using FluentValidation;

namespace Application.CreditCards
{
    public class CreditCardValidator : AbstractValidator<Domain.CreditCard>
    {
        public CreditCardValidator()
        {
            RuleFor(x => x.Name).Length(1, 50);
            RuleFor(x => x.CardNumber).NotEmpty();
            RuleFor(x => x.CVC).NotEmpty();
            RuleFor(x => x.Expiry).NotEmpty();
        }
    }

}