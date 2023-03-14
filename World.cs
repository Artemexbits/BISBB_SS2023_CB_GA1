namespace BISBB_SS2023_CB_GA1;
class World : IRenderable
{
    public static readonly char WALL = '#';
    public static readonly char COIN = 'V';
    public static readonly char ENEMY = 'Z';
    public static readonly char PORTAL = 'X';
    public readonly (int x, int y) start_pos;
    private (int x, int y) scoreboard_pos;
    private (int x, int y) healthboard_pos;
    private (int x, int y) levelboard_pos;
    private int width;
    private int height;
    public char[,] matrix;
    public Player? current_player;
    public World(char[,] matrix, (int x, int y) start_pos)
    {
        this.matrix = matrix;
        this.start_pos = start_pos;
        scoreboard_pos = getLastIndexOfSequenceIn2DArray("SCORE: A", matrix);
        healthboard_pos = getLastIndexOfSequenceIn2DArray("LIFE:  A", matrix);
        levelboard_pos = getLastIndexOfSequenceIn2DArray("WORLD: A", matrix);
    }
    public void update()
    {
        current_player!.update();
    }
    public void render()
    {
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                if (i == scoreboard_pos.x && j == scoreboard_pos.y)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(i, j);
                    Console.Write(current_player!.score);
                }
                else
                if (i == healthboard_pos.x && j == healthboard_pos.y)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(i, j);
                    Console.Write(current_player!.health);
                } else
                if(i == levelboard_pos.x && j == levelboard_pos.y) {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.SetCursorPosition(i, j);
                    Console.Write(current_player!.level);
                }
                else
                if (matrix[j, i] == World.WALL)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(i, j);
                    Console.Write(matrix[j, i]);
                }
                else
                if (matrix[j, i] == World.ENEMY)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(i, j);
                    Console.Write(matrix[j, i]);
                }
                else
                if (matrix[j, i] == World.COIN)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(i, j);
                    Console.Write(matrix[j, i]);
                }else
                if(matrix[j, i] == World.PORTAL) {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.SetCursorPosition(i, j);
                    Console.Write(matrix[j, i]);
                }
                else
                {
                    if (!((int)current_player!.x == i && (int)current_player!.y == j))
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(matrix[j, i]);
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        current_player!.render();
    }
    public static World createFromFile(string filename, (int x, int y) start_pos)
    {
        
        char[,] matrix = scanFile(filename);
        try
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                int x = 0;
                int y = 0;
                while (true)
                {
                    int c = fs.ReadByte();
                    if (c == 10)
                    {
                        y++;
                        x = 0;
                    }
                    else
                    if (c == -1)
                    {
                        y++;
                        break;
                    }
                    else
                    {
                        matrix[y, x] = (char)c;
                        x++;
                    }
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("ERROR: creating world from file: " + filename + " failed");
            System.Environment.Exit(1);
        }
        return new World(matrix, start_pos);
    }

    private static char[,] scanFile(string filename)
    {
        char[,] matrix = new char[0, 0];
        try
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                int width = 0;
                int height = 0;
                int x = 0;
                int y = 0;
                while (true)
                {
                    int c = fs.ReadByte();
                    if (c == 10)
                    {
                        y++;
                        if (x > width)
                        {
                            width = x;
                        }
                        x = 0;
                    }
                    else
                    if (c == -1)
                    {
                        y++;
                        height = y;
                        matrix = new char[height, width];
                        break;
                    }
                    else
                    {
                        x++;
                    }
                }
            }
        }
        catch (Exception)
        {
            return matrix;
        }
        return matrix;
    }

    private static (int, int) getLastIndexOfSequenceIn2DArray(string sequence, char[,] array)
    {
        (int x, int y) pos = (0, 0);
        for (int i = 0; i < array.GetLength(0); i++)
        {
            string line = "";
            for (int j = 0; j < array.GetLength(1); j++)
            {
                line += array[i, j];
            }
            Console.WriteLine(line);
            if (line.Contains(sequence))
            {
                pos.x = (line.IndexOf(sequence) + sequence.Length - 1);
                pos.y = i;
            }
        }
        return pos;
    }
}