using System.Text.Json;

namespace e6;

internal class Program
{
    static async Task Main(string[] args)
    {
        using HttpClient client = new();

        List<Type> types = new();
        List<Pokemon> pokemons = new();

        //Obtenemos la lista de todos los types
        {
            HttpResponseMessage response = await client.GetAsync($"https://pokeapi.co/api/v2/type/?limit=100");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<Response<Type>>(responseBody);

            types.AddRange(responseData!.results);
        }

        //Obtenemos el detalle (pokemon) de cada type
        foreach (var type in types)
        {
            HttpResponseMessage response = await client.GetAsync(type!.url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<Type>(responseBody);
            type.pokemon = responseData!.pokemon;
        }

        //Obtenemos la lista de todos los pokemon
        {
            HttpResponseMessage response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon/?limit=2000");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<Response<Pokemon>>(responseBody);

            pokemons.AddRange(responseData!.results);
        }

        //Obtenemos el detalle (height) de cada pokemon
        foreach (var pokemon in pokemons)
        {
            HttpResponseMessage response = await client.GetAsync(pokemon!.url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<Pokemon>(responseBody);
            pokemon.height = responseData!.height;
        }

        foreach (var type in types)
        {
            var pokemonsInType = type.pokemon.Select(p => pokemons.Single(p2 => p2.url == p.pokemon.url));
            var heightSum = pokemonsInType.Sum(p => p.height);
            var heightMean = heightSum / pokemonsInType.Count();
            Console.WriteLine($"{type.name}: {heightMean}");
        }

        /*
        R:
        {
          "heights": {
            "bug": 19.528,
            "dark": 20.063,
            "dragon": 43.373,
            "electric": 16.490,
            "fairy": 19.409,
            "fighting": 22.600,
            "fire": 28.990,
            "flying": 16.597,
            "ghost": 14.728,
            "grass": 16.822,
            "ground": 19.322,
            "ice": 18.272,
            "normal": 15.664,
            "poison": 33.705,
            "psychic": 16.205,
            "rock": 18.019,
            "steel": 27.758,
            "water": 22.758
          }
        }
        */

        Console.ReadLine();
    }
}

class Response<T>
{
    public string next { get; set; }
    public List<T> results { get; set; }
}

class Type
{
    public string url { get; set; }
    public string name { get; set; }
    public List<WrapedPokemon> pokemon { get; set; }
}
class Pokemon
{
    public string url { get; set; }
    public string name { get; set; }
    public float height { get; set; }
}
class WrapedPokemon
{
    public Pokemon pokemon { get; set; }
}
