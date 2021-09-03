using GraphQL.Types;
using TravelSample.Services;

namespace TravelSample.Models.GraphQL
{
    public class AirportObjectGraphQuery : ObjectGraphType
    {
     
        public AirportObjectGraphQuery(IAirportsService airportService)
        {
            FieldAsync<ListGraphType<AirportObjectGraph>>(
                name: "Airports",
                resolve: async context => await airportService.GetAirportsByRegion("United States")
            );
        }
    }
}
