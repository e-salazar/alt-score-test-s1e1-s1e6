class Program
{
    static async Task Main(string[] args)
    {
        int count = 35;
        int total = 0;
        float mean = 0;

        for (int i = 1; i <= 100; i++)
        {
            count = count + 7;
            total = total + count;
            mean = total / (float)i;
            Console.WriteLine($"i: {i} - Count: {count} - Total: {total} - Mean: {mean}");
        }

        // R: 388

        Console.ReadLine();
    }
}
