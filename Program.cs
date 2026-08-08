using System.Net.Http.Headers;
using osrs_farming_herblore_calc.Models;
using osrs_farming_herblore_calc.Constants;

HttpClient httpClient = new()
{
    BaseAddress = new Uri("https://prices.runescape.wiki")
};

httpClient.DefaultRequestHeaders.UserAgent.Add(
    new ProductInfoHeaderValue(new ProductHeaderValue("farming_herblore_calc_ipopcorn_on_Discord")));

Console.WriteLine("Getting Prices");

List<PotionComponents> potionsToGet = new()
{
    new PotionComponents("prayer", new Item("Prayer potion(4)", ItemIds.PRAYER_POT_4), new Item("Ranarr seed", ItemIds.RANARR_SEED)),
    new PotionComponents("super restore", new Item("Super restore(4)", ItemIds.SUPER_RESTORE_4), new Item("Snapdragon seed", ItemIds.SNAPDRAGON_SEED)),
    new PotionComponents("sara brew", new Item("Saradomin brew(4)", ItemIds.SARADOMIN_BREW_4), new Item("Toadflax seed", ItemIds.TOADFLAX_SEED)),
    new PotionComponents("super att", new Item("Super attack(4)", ItemIds.SUPER_ATTACK_4), new Item("Irit seed", ItemIds.IRIT_SEED)),
};

List<Item> retrievedItems = [];

foreach(var components in potionsToGet)
{
    // for each item, get current price
    await GetPricesForComponents(components, httpClient);

    decimal breakEvenPoint = GetBreakEvenPoint(components);
    Console.WriteLine($"Break Even Point: {breakEvenPoint}");

    decimal profitFor8Herbs = GetProfit(components);
    Console.WriteLine($"Profit per 8 herbs: {profitFor8Herbs}");
}

// helper methods
static decimal GetProfit(PotionComponents components)
{
    // Expected profit for 8 herbs (6 potions)
    decimal potionPrice = components.Potion.Price;
    decimal seedPrice = components.Seed.Price;

    decimal totalCost = seedPrice;
    decimal revenueHerbs = 8m * 0.75m * potionPrice;  // 1 herb makes 0.75 of a 4 dose pot
    
    return revenueHerbs - totalCost;
}

static decimal GetBreakEvenPoint(PotionComponents components)
{
    decimal potionPrice = components.Potion.Price;
    decimal seedPrice = components.Seed.Price;

    Console.WriteLine($"Potion Price: {potionPrice}");
    Console.WriteLine($"Seed Price: {seedPrice}");

    decimal totalCost = seedPrice;
    decimal breakEvenPointPotions = totalCost / potionPrice;
    return Math.Ceiling(breakEvenPointPotions * 1.25m); // It takes 1.25 herbs to make a 4 dose potion
}

async static Task GetPricesForComponents(PotionComponents components, HttpClient httpClient)
{
    await GetPriceForItem(components.Potion, httpClient);
    await GetPriceForItem(components.Seed, httpClient);
}

async static Task GetPriceForItem(Item item, HttpClient httpClient)
{
    using HttpResponseMessage response = await httpClient.GetAsync($"api/v2/osrs/latest?id={item.Id}");

    WriteRequestToConsole(response);
    response.EnsureSuccessStatusCode();

    var jsonResponse = await response.Content.ReadAsStringAsync();
    var price = GetPriceFromResponse(jsonResponse);
    item.Price = price;
}
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

static decimal GetPriceFromResponse(String response)
{
    var stripped = response
        .Replace("{", "")
        .Replace("}", "");

    var tokens = stripped.Split("\"high\":");
    var priceString = tokens[1].Split(",")[0];

    if (!decimal.TryParse(priceString, out decimal price))
    {
        throw new Exception("Failed to get price");
    }

    if (price == 0m)
    {
        throw new Exception("Price is 0 when it should not be");
    }

    return price;
}