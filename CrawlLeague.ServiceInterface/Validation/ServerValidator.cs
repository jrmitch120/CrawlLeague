using CrawlLeague.ServiceModel;
using CrawlLeague.ServiceModel.Operations;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace CrawlLeague.ServiceInterface.Validation
{
    public static class ServerRules<T> where T : Server
    {
        public static void GetRules(AbstractValidator<T> validator)
        {
            validator.RuleSet(ApplyTo.Post | ApplyTo.Put, () =>
            {
                validator.RuleFor(r => r.Name).NotEmpty();
                validator.RuleFor(r => r.Abbreviation).NotEmpty();
                validator.RuleFor(r => r.Url).NotEmpty();
                validator.RuleFor(r => r.RcUrl).NotEmpty();
                validator.RuleFor(r => r.MorgueUrl).NotEmpty();
            });
        }
    }

    public class CreateServerValidator : AbstractValidator<CreateServer>
    {
        public CreateServerValidator()
        {
            ServerRules<CreateServer>.GetRules(this);
        }
    }

    public class UpdateServerValidator : AbstractValidator<UpdateServer>
    {
        public UpdateServerValidator()
        {
            ServerRules<UpdateServer>.GetRules(this);
        }
    }
}
