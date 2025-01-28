class Program
{
    static async Task Main(string[] args)
    {
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("API-KEY", "API_KEY_HERE");

        while (true)
        {
            HttpResponseMessage response = await client.GetAsync("https://makers-challenge.altscore.ai/v1/s1/e1/resources/measurement");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            if (responseBody != "{\"distance\":\"failed to measure, try again\",\"time\":\"failed to measure, try again\"}")
                Console.WriteLine("La respuesta ha cambiado:" + responseBody);
            else
                Console.WriteLine("Respuesta no cambió: " + responseBody);

            await Task.Delay(TimeSpan.FromMinutes(1));
        }

        //La respuesta ha cambiado:{"distance":"917 AU","time":"2.2641975308641977 hours"}
        // R: 405
    }
}
