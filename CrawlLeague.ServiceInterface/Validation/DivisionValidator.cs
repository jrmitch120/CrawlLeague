using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace CrawlLeague.ServiceInterface.Validation
{
    public static class DivisionRules<T> where T : DivisionCore
    {
        public static void GetRules(AbstractValidator<T> validator)
        {
            validator.RuleSet(ApplyTo.Post | ApplyTo.Put, () =>
            {
                validator.RuleFor(r => r.Name).NotEmpty();
                validator.RuleFor(r => r.Description).NotEmpty();
            });
        }
    }

    public class CreateDivisionValidator : AbstractValidator<CreateDivision>
    {
        public CreateDivisionValidator()
        {
            DivisionRules<CreateDivision>.GetRules(this);
        }
    }

    public class UpdateDivisionValidator : AbstractValidator<UpdateDivision>
    {
        public UpdateDivisionValidator()
        {
            DivisionRules<UpdateDivision>.GetRules(this);
        }
    }
}
