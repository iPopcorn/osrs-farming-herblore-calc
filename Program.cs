using System.Net.Http.Headers;
using osrs_farming_herblore_calc.Models;

HttpClient httpClient = new()
{
    BaseAddress = new Uri("https://prices.runescape.wiki")
};

httpClient.DefaultRequestHeaders.UserAgent.Add(
    new ProductInfoHeaderValue(new ProductHeaderValue("farming_herblore_calc_ipopcorn_on_Discord")));

Console.WriteLine("Getting Prices");

// Find price of items for prayer potions
// list of items by id
var prayerPot4Dose = 2434;
var ranarrSeed = 5295;
var snapeSeed = 22879;

List<(String, int)> items = new()
{
    ("Prayer potion(4)", prayerPot4Dose),
    ("Ranarr seed", ranarrSeed),
    ("Snape grass seed", snapeSeed)
};

// for each item, get current price
foreach(var (name, id) in items)
{
    using HttpResponseMessage response = await httpClient.GetAsync($"api/v2/osrs/latest?id={id}");

    WriteRequestToConsole(response);
    response.EnsureSuccessStatusCode();


    var jsonResponse = await response.Content.ReadAsStringAsync();
    var item = GetItemFromResponse(jsonResponse, name);
    Console.WriteLine($"Name: {item.Name} Price: {item.Price}");
}

// Calculate Profit
// calculate total cost
// calculate break even point

// helper methods
static void WriteRequestToConsole(HttpResponseMessage response)
{
    if (response is null)
    {
        return;
    }

    var request = response.RequestMessage;
    Console.Write($"{request?.Method} ");
    Console.Write($"{request?.RequestUri} ");
    Console.WriteLine($"HTTP/{request?.Version}");
    Console.WriteLine($"{request?.Headers}");
}

static Item GetItemFromResponse(String response, String name)
{
    var stripped = response
        .Replace("{", "")
        .Replace("}", "");

    var tokens = stripped.Split("\"high\":");
    var priceString = tokens[1].Split(",")[0];

    if (!int.TryParse(priceString, out var price))
    {
        throw new Exception("Failed to get price");
    }

    return new Item(name, price);
}