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

        List<DataItem> A = new List<DataItem>();
        List<DataItem> B = new List<DataItem>();
        List<DataItem> C = new List<DataItem>();
        List<DataItem> D = new List<DataItem>();
        List<DataItem> E = new List<DataItem>();
        List<DataItem> F = new List<DataItem>();
        List<DataItem> G = new List<DataItem>();
        List<DataItem> H = new List<DataItem>();

        bool smooth = false;
        class DataItem
        {
            public string Date { get; set; }
            public int Count { get; set; }
        }
        protected JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) await LoadCounts();
        }

        public async Task LoadCounts()
        {
            var counts = await CountsClient.GetAsync();
            foreach(var count in counts)
            {
                var dictionary = JsonSerializer.Deserialize<Dictionary<string, int>>(count.JsonData, JsonSerializerOptions);
                A.Add(new DataItem() { Date = count.Name, Count = dictionary["A"] });
                B.Add(new DataItem() { Date = count.Name, Count = dictionary["B"] });
                C.Add(new DataItem() { Date = count.Name, Count = dictionary["C"] });
                D.Add(new DataItem() { Date = count.Name, Count = dictionary["D"] });
                E.Add(new DataItem() { Date = count.Name, Count = dictionary["E"] });
                F.Add(new DataItem() { Date = count.Name, Count = dictionary["F"] });
                G.Add(new DataItem() { Date = count.Name, Count = dictionary["G"] });
                H.Add(new DataItem() { Date = count.Name, Count = dictionary["H"] });
            }
        }
    }
}
