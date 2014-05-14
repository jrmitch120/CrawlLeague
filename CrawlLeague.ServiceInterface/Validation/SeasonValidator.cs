using System;
using CrawlLeague.ServiceModel;
using CrawlLeague.ServiceModel.Operations;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace CrawlLeague.ServiceInterface.Validation
{
    public static class SeasonRules<T> where T : Season
    {
        public static void GetRules(AbstractValidator<T> validator)
        {
            validator.RuleSet(ApplyTo.Post, () =>
            {
                validator.RuleFor(r => r.Name).NotEmpty();
                validator.RuleFor(r => r.Description).NotEmpty();
                validator.RuleFor(r => r.DaysPerRound).GreaterThan(2);
                validator.RuleFor(r => r.CrawlVersion).NotEmpty();
                validator.RuleFor(r => r.Start).GreaterThan(DateTime.UtcNow);
                validator.RuleFor(r => r.End)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .GreaterThan(r => DateTime.UtcNow)
                    .GreaterThan(r => r.Start);
            });
        }
    }

    public class CreateSeasonValidator : AbstractValidator<CreateSeason>
    {
        public CreateSeasonValidator()
        {
            SeasonRules<CreateSeason>.GetRules(this);
        }
    }

    public class UpdateSeasonValidator : AbstractValidator<UpdateSeason>
    {
        public UpdateSeasonValidator()
        {
            SeasonRules<UpdateSeason>.GetRules(this);
        }
    }
}
