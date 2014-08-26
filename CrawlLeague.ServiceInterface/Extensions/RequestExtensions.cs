using System.Collections.Generic;
using CrawlLeague.ServiceModel.Operations;
using CrawlLeague.ServiceModel.Types;
using ServiceStack;

namespace CrawlLeague.ServiceInterface.Extensions
{
    public static class RequestExtensions
    {
        public static UpdateSeason ToUpdateRequest(this Season season)
        {
            return (new UpdateSeason().PopulateWith(season));
        }

        public static IEnumerable<Season> GetAll(this SeasonService service, FetchSeasons request)
        {
            var seasons = new List<Season>();
            SeasonsResponse response;
            request.Page = 1;

            do
            {
                response = service.Get(request);
                seasons.AddRange(response.Seasons);

                request.Page++;
            } while (request.Page <= response.Paging.TotalCount); 

            return seasons;
        }
    }
}
