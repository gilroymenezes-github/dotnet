using GraphQL;
using GraphQL.Types;

namespace TravelSample.Models.GraphQL
{
    public class AirportSchema : Schema
    {
        public AirportSchema(IDependencyResolver resolver): base(resolver)
        {
            Query = resolver.Resolve<AirportObjectGraphQuery>();
        }
    }
}
