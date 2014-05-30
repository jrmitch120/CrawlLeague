using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace CrawlLeague.ServiceInterface.Validation
{
    public static class CrawlerRules<T> where T : CrawlerCore
    {
        public static void GetRules(AbstractValidator<T> validator)
        {
            validator.RuleSet(ApplyTo.Post | ApplyTo.Put, () =>
            {
                validator.RuleFor(r => r.UserName).NotEmpty();
            });
        }
    }

    public class CreateCrawlerValidator : AbstractValidator<CreateCrawler>
    {
        public CreateCrawlerValidator()
        {
            CrawlerRules<CreateCrawler>.GetRules(this);
        }
    }

    public class UpdateCrawlerValidator : AbstractValidator<UpdateCrawler>
    {
        public UpdateCrawlerValidator()
        {
            CrawlerRules<UpdateCrawler>.GetRules(this);
        }
    }
}
