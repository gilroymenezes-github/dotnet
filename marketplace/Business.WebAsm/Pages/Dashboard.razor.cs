using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.WebAsm.Pages
{
    public partial class Dashboard : ComponentBase
    {
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        protected IEnumerable<DataItem> Items;
        public class DataItem
        {
            public string Stock { get; set; }
            public double CapitalExpenditures { get; set; }
            public double ChangeToNetIncome { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync("http://localhost:7071/api/cashflow_annually");

                Items =  string.IsNullOrEmpty(response)
                        ? new List<DataItem>()
                        : JsonSerializer.Deserialize<List<DataItem>>(response, jsonSerializerOptions);
                
            }
            
        }

        string FormatAsUSD(object value)
        {
            return ((double)value).ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}
