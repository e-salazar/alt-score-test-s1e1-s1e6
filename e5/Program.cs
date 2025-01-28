namespace e5;

internal class Program
{
    static string radarData1 = "a01b^1c01d01e01f01g01h01|a02b02c02d$2e02f02g02h02|a03b03c$3d03e03f03g03h03|a04b04c$4d04e04f04g04h04|a05b05c05d05e05f05g05h05|a06b06c06d$6e06f06g06h06|a07b07c07d07e07f07g07h07|a08b08c08d08e#8f08g08h08|";
    static string radarData2 = "a01b01c01d01e01f01g01h01|a02b02c02d$2e02f02g02h02|a^3b03c$3d03e03f03g03h03|a04b04c$4d04e04f04g04h04|a05b05c05d05e05f05g05h05|a06b06c06d$6e06f06g06h06|a07b07c07d07e07f07g07h07|a08b08c08d08e#8f08g08h08|";
    static string radarData3 = "a01b01c01d01e01f01g01h01|a02b02c02d$2e02f02g02h02|a03b03c$3d03e03f03g03h03|a04b04c$4d04e04f04g04h04|a05b^5c05d05e05f05g05h05|a06b06c06d$6e06f06g06h06|a07b07c07d07e07f07g07h07|a08b08c08d08e#8f08g08h08|";

    static void Main(string[] args)
    {
        DrawRadar(radarData1);
        Console.WriteLine();
        Console.WriteLine();
        DrawRadar(radarData2);
        Console.WriteLine();
        Console.WriteLine();
        DrawRadar(radarData3);

        //R: attack c7

        Console.ReadLine();
    }

    static void DrawRadar(string radarData)
    {
        List<List<Cell>> grid = [];
        List<string> lines = radarData.Split('|').ToList();
        lines.Reverse();
        foreach (string line in lines)
        {
            if (String.IsNullOrEmpty(line))
                continue;

            List<Cell> row = [];

            for (int i = 0; i < line.Length; i += 3)
                row.Add(new Cell(line.Substring(i, 3)));

            grid.Add(row);
        }

        if (grid.Count == 0)
        {
            Console.WriteLine("    a b c d e f g h");
            Console.WriteLine("   -----------------");
            Console.WriteLine("8 |                 | 8");
            Console.WriteLine("7 |                 | 7");
            Console.WriteLine("6 |                 | 6");
            Console.WriteLine("5 |                 | 5");
            Console.WriteLine("4 |                 | 4");
            Console.WriteLine("3 |                 | 3");
            Console.WriteLine("2 |                 | 2");
            Console.WriteLine("1 |                 | 1");
            Console.WriteLine("   -----------------");
            Console.WriteLine("    a b c d e f g h");
            return;
        }

        Console.Write($"    ");
        foreach (var cell in grid[0])
            Console.Write($"{cell.x} ");
        Console.WriteLine("\n   -----------------");
        
        foreach (var row in grid)
        {
            Console.Write($"{row[0].y} | ");

            foreach (var cell in row)
            {
                switch (cell.content)
                {
                    case Content.Enemy:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    case Content.Friend:
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        break;
                    case Content.Obstacle:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    case Content.Empty:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                }

                Console.Write($"{cell.status} ");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine($"| {row[0].y}");
        }

        Console.WriteLine("   -----------------");
        Console.Write($"    ");
        foreach (var cell in grid[0])
            Console.Write($"{cell.x} ");
        Console.WriteLine();
    }
    static void DrawCharRadar(string radarData)
    {
        List<List<Cell>> grid = [];
        List<string> lines = radarData.Split('|').ToList();
        lines.Reverse();
        foreach (string line in lines)
        {
            if (String.IsNullOrEmpty(line))
                continue;

            List<Cell> row = [];

            for (int i = 0; i < line.Length; i += 3)
                row.Add(new Cell(line.Substring(i, 3)));

            grid.Add(row);
        }

        Console.Write($"    ");
        foreach (var cell in grid[0])
            Console.Write($"{cell.data[0]}  ");
        Console.WriteLine("\n   -------------------------");

        foreach (var row in grid)
        {
            Console.Write($"{row[0].data[2]} | ");

            foreach (var cell in row)
            {
                switch (cell.content)
                {
                    case Content.Enemy:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    case Content.Friend:
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        break;
                    case Content.Obstacle:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    case Content.Empty:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                }

                Console.Write($"{(int)cell.data[1]} ");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine($"| {row[0].y}");
        }

        Console.WriteLine("   -------------------------");
        Console.Write($"    ");
        foreach (var cell in grid[0])
            Console.Write($"{cell.x}  ");
    }
}

class Cell
{
    public char x;
    public int y;
    public char status;
    public char[] data;
    public Content? content;

    public Cell(string data)
    {
        this.x = data[0];
        this.y = (int)char.GetNumericValue(data[2]);
        this.status =  data[1];
        this.data = data.ToCharArray();
        this.content =
            this.status == '^' ? Content.Enemy :
            this.status == '#' ? Content.Friend :
            this.status == '$' ? Content.Obstacle :
            this.status == '0' ? Content.Empty :
            throw new Exception();
    }
}

enum Content
{
    Enemy,
    Friend,
    Obstacle,
    Empty,
}
