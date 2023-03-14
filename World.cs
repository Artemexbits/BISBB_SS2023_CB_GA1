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
    public Enemy[]? enemies;
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
        foreach(Enemy e in enemies!) {
            e.update();
        }
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
        foreach(Enemy e in enemies!) {
            e.render();
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
        catch (Exception e)
        {
            Console.WriteLine("ERROR: creating world from file: " + filename + " failed \n" + e);
            System.Environment.Exit(1);
        }
        World w =  new World(matrix, start_pos);
        Enemy[] enemies = createEnemies(matrix, w);
        w.enemies = enemies;
        return w;
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

    
    public static Enemy[] createEnemies(char[,] matrix, World w) {
        List<List<(int x, int y)>> tracks = new List<List<(int x, int y)>>();
        for(int i = 0; i < matrix.GetLength(0); i++) {
            for(int j = 0; j < matrix.GetLength(1); j++) {
                if(isInTracks(tracks, j, i)) {
                    continue;
                }
                List<(int x, int y)> track = new List<(int x, int y)>();
                char cell = matrix[i,j];
                if(cell == 'Z') {
                    (int x, int y) next = (j, i);
                    //Console.WriteLine($"firstnext: {next.x} {next.y}");
                    while(next != (-1, -1)) {
                        track.Add(next);
                        next = nextNeighbor(track, matrix, next.x, next.y);
                        //Console.WriteLine($"next: {next.x} {next.y}");
                    }
                    tracks.Add(track);
                }
            }
        }
        Enemy[] enemies = new Enemy[tracks.Count];
        for(int i = 0; i < tracks.Count; i++) {
            (int x, int y)[] track = tracks[i].ToArray();
            enemies[i] = new Enemy(w, 'Z', track);
        }
        
        return enemies;
    }

    private static bool isInTracks(List<List<(int x, int y)>> tracks, int x, int y) {
        foreach(List<(int x, int y)> track in tracks) {
            if(isTrack(track, x, y)) {
                return true;
            }
        }
        return false;
    }
    private static bool isTrack(List<(int x, int y)> track, int x, int y) {
        foreach((int x, int y) pos in track) {
            if(pos.x == x && pos.y == y) {
                return true;
            }
        }
        return false;
    }
    private static (int x, int y) nextNeighbor(List<(int x, int y)> track, char[,] matrix, int x, int y) {
        if(matrix[y+1, x] == 'Z' && !isTrack(track, x, y+1)) {
            return (x, y+1);
        } else 
        if(matrix[y, x+1] == 'Z' && !isTrack(track, x+1, y)) {
            return (x+1, y);
        } else 
        if(matrix[y-1, x] == 'Z' && !isTrack(track, x, y-1)) {
            return (x, y-1);
        } else 
        if(matrix[y, x-1] == 'Z' && !isTrack(track, x-1, y)) {
            return (x-1, y);
        } else {
            return (-1, -1);
        }
    }
}