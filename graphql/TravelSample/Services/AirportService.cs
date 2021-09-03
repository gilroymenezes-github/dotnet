using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Query;
using Microsoft.Extensions.Options;
using TravelSample.Models;
using TravelSample.Helpers;
using Couchbase;

namespace TravelSample.Services
{
    public interface IAirportsService
    {
        Task<(IEnumerable<Airport>, string[])> GetAirports(string search);
        Task<IAsyncEnumerable<Airport>> GetAirportsByRegion(string region);
    }

    public class AirportsService : IAirportsService
    {
        private readonly ICouchbaseService _couchbaseService;
        private readonly AppSettings _appSettings;

        public AirportsService(ICouchbaseService couchbaseService, IOptions<AppSettings> appSettings)
        {
            _couchbaseService = couchbaseService;
            _appSettings = appSettings.Value;
        }

        public async Task<IAsyncEnumerable<Airport>> GetAirportsByRegion(string region)
        {
            var q = "SELECT airport.* FROM `travel-sample`.inventory.airport WHERE country LIKE $1";
            region = char.ToUpper(region[0]) + region.Substring(1) + '%';
            var airportsResult = await _couchbaseService.Cluster.QueryAsync<Airport>(
                q, 
                options => options.Parameter(region)
            );
            if (airportsResult.MetaData.Status != QueryStatus.Success)
            {
                await Task.FromResult(new List<Airport>());
            }
            var airports = await airportsResult.Rows.ToListAsync();
            return airports.ToAsyncEnumerable();
        }

        public async Task<(IEnumerable<Airport>, string[])> GetAirports(string search)
        {
            //var q = "SELECT name FROM `travel-sample`.inventory.airport WHERE ";
            var q = "SELECT airport.* FROM `travel-sample`.inventory.airport WHERE ";

            if (search.Length == 3)        // Is an faa code
            {
                q += "faa=$1";
                search = search.ToUpper();
            } else if (search.Length == 4) // Is an icao code
            {
                q += "icao=$1";
                search = search.ToUpper();
            } else                         // Is not a code
            {
                q += "country LIKE $1";
                search = char.ToUpper(search[0]) + search.Substring(1) + '%';
            }

            var airportsResult = await _couchbaseService.Cluster.QueryAsync<Airport>(
                q,
                options => options.Parameter(search)
            );

            if (airportsResult.MetaData.Status != QueryStatus.Success)
            {
                Console.WriteLine(airportsResult.Errors.OfType<string>());
                return (null, new string[] { "Query Failed." });
            }

            var airports = await airportsResult.Rows.ToListAsync();
            
            var context = new string[] {
                $"N1QL query - scoped to inventory: {q}; -- {search}"
            };

            //return (airports.ToAsyncEnumerable(), context);
            return (airports, context);
        }
    }
}
