using System;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace CrawlLeague.ServiceInterface.Validation
{
    public static class SeasonRules<T> where T : SeasonCore
    {
        public static void GetRules(AbstractValidator<T> validator)
        {
            validator.RuleSet(ApplyTo.Post | ApplyTo.Put, () =>
            {
                validator.RuleFor(r => r.Name).NotEmpty();
                validator.RuleFor(r => r.Description).NotEmpty();
                validator.RuleFor(r => r.DaysPerRound).GreaterThan(0);
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
