using CrawlLeague.ServiceModel;
using CrawlLeague.ServiceModel.Operations;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace CrawlLeague.ServiceInterface.Validation
{
    public static class CrawlerRules<T> where T : Crawler
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
