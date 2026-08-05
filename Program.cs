using System.Net.Http.Headers;

Console.WriteLine("Hello World");

// Find price of items for prayer potions
// list of items by id
var prayerPot4Dose = 2434;

// for each item, get current price
HttpClient httpClient = new()
{
    BaseAddress = new Uri("https://prices.runescape.wiki")
};

httpClient.DefaultRequestHeaders.UserAgent.Add(
    new ProductInfoHeaderValue(new ProductHeaderValue("farming_herblore_calc_ipopcorn_on_Discord")));

using HttpResponseMessage response = await httpClient.GetAsync($"api/v2/osrs/latest?id={prayerPot4Dose}");

WriteRequestToConsole(response);
response.EnsureSuccessStatusCode();


var jsonResponse = await response.Content.ReadAsStringAsync();
Console.WriteLine($"{jsonResponse}\n");

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