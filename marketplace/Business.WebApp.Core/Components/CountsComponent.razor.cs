using Business.Shared.Connections;
using Business.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.WebApp.Core.Components
{
    public partial class CountsComponent
    {
        [Inject] public CountsHttpClientWithAuth<CountModel> CountsClient { get; set; }

        Dictionary<string, List<DataItem>> ChartData = new Dictionary<string, List<DataItem>>();

        bool smooth = true;
        class DataItem
        {
            public string Date { get; set; }
            public int Count { get; set; }
        }
        protected JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        protected override async Task OnInitializedAsync()
        {
            await LoadCounts();
        }

        public async Task LoadCounts()
        {
            var counts = await CountsClient.GetAsync();
            if (counts is null) return;
            foreach(var count in counts)
            {
                var dictionary = JsonSerializer.Deserialize<Dictionary<string, int>>(count.JsonData, JsonSerializerOptions);
                foreach(var kvp in dictionary)
                {
                    if (!ChartData.Keys.Contains(kvp.Key)) ChartData.Add(kvp.Key, new List<DataItem>());
                    ChartData[kvp.Key].Add(new DataItem() { Date = count.Name, Count = kvp.Value });
                }
            }
        }
    }
}
