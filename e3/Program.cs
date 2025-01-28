using System.Text;
using System.Text.Json;

namespace e3;

internal class Program
{
    static async Task Main(string[] args)
    {
        using HttpClient client = new();

        List<People> people = new();
        List<Planet> planets = new();
        int index;

        //Obtenemos todos los personajes
        index = 1;

        while (true)
        {
            HttpResponseMessage response = await client.GetAsync($"https://swapi.dev/api/people/?page={index}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var page = JsonSerializer.Deserialize<Page<People>>(responseBody);

            people.AddRange(page!.results);

            if (page.next == null)
                break;
            index++;
        }

        //Obtenemos todos los planetas
        index = 1;

        while (true)
        {
            HttpResponseMessage response = await client.GetAsync($"https://swapi.dev/api/planets/?page={index}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var page = JsonSerializer.Deserialize<Page<Planet>>(responseBody);

            planets.AddRange(page!.results);

            if (page.next == null)
                break;
            index++;
        }

        //Calculamos para cada personaje su lado (luminoso/oscuro)
        client.DefaultRequestHeaders.Add("API-KEY", "API_KEY_HERE");
        foreach (var person in people)
        {
            HttpResponseMessage response = await client.GetAsync($"https://makers-challenge.altscore.ai/v1/s1/e3/resources/oracle-rolodex?name={person.name}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var oracle = JsonSerializer.Deserialize<Oracle>(responseBody);
            string decodedText = Encoding.UTF8.GetString(Convert.FromBase64String(oracle!.oracle_notes));

            if (decodedText.Contains("belongs to the Dark Side of the Force."))
                person.side = Side.Dark;
            else if (decodedText.Contains("belongs to the Light Side of the Force."))
                person.side = Side.Light;
            else
                Console.WriteLine(decodedText);
        }

        //Calculamos para cada planeta su equilibrio
        foreach (var planet in planets)
        {
            var planetPeople = planet.residents.Select(resident => people.Single(p => p.url == resident));
            var lightCount = planetPeople.Where(p => p.side == Side.Light).Count();
            var darkCount = planetPeople.Where(p => p.side == Side.Dark).Count();
            float indiceFuerza = (float) (lightCount - darkCount) / (lightCount + darkCount);
            if (indiceFuerza == 0)
                break;
        }

        //R: Ryloth

        Console.ReadLine();
    }
}

enum Side
{
    Light,
    Dark,
}

class Page<T>
{
    public string next { get; set; }
    public List<T> results { get; set; }
}

class People
{
    public string url { get; set; }
    public string name { get; set; }
    public string homeworld { get; set; }
    public Planet planet { get; set; }
    public Side? side { get; set; }
}
class Planet
{
    public string url { get; set; }
    public string name { get; set; }
    public string homeworld { get; set; }
    public List<string> residents { get; set; }
}
class Oracle
{
    public string oracle_notes { get; set; }
}