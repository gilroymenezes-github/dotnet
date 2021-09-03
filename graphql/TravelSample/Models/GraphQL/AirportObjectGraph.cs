using GraphQL.Types;

namespace TravelSample.Models.GraphQL
{
    public class AirportObjectGraph: ObjectGraphType<Airport>
    {
        public AirportObjectGraph()
        {
            Field(f => f.Id);
            Field(f => f.Airportname);
            Field(f => f.City);
            Field(f => f.Country);
            Field(f => f.Faa);
            Field(f => f.Icao);
            Field(f => f.Type);
        }
    }
}
