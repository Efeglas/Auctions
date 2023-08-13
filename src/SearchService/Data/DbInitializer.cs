using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using System.Text.Json;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static async Task InitDb(WebApplication application) 
        {     
            await DB.InitAsync("SearchDB", MongoClientSettings.FromConnectionString(application.Configuration.GetConnectionString("MongoDbConnection")));
            await DB.Index<Item>().Key(item => item.Make, KeyType.Text).Key(item => item.Model, KeyType.Text).Key(item => item.Color, KeyType.Text).CreateAsync();

            long count = await DB.CountAsync<Item>();

            if (count == 0)
            {
                Console.WriteLine("No data found. Seeding database");

                string itemData = await File.ReadAllTextAsync("Data/auctions.json");
                JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
                List<Item> items = JsonSerializer.Deserialize<List<Item>>(itemData, options);
                await DB.SaveAsync(items);
            }
        }

    }
}
