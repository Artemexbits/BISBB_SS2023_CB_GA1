// Copyright (c) 2023 Artemexbits. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace BISBB_SS2023_CB_GA1;
class World : IRenderable
{
    public static readonly char WALL = '#';
    public static readonly char COIN = 'O';
    public static readonly char ENEMY = 'Z';
    public static readonly char PORTAL = 'X';
    public static readonly char START = 'Y';
    public static readonly char SPACE = ' ';
    public static short frametime = 1;
    public readonly (int x, int y) start_pos;
    public char[,] matrix;
    public Player? current_player;
    public Enemy[]? enemies;
    public List<Coin>? coins;
    private (int x, int y) scoreboard_pos;
    private (int x, int y) healthboard_pos;
    private (int x, int y) levelboard_pos;
    private (int x, int y) frameboard_pos;
    private int width;
    private int height;
    public World(char[,] matrix, (int x, int y) start_pos)
    {
        this.matrix = matrix;
        this.start_pos = start_pos;
        scoreboard_pos = getLastIndexOfSequenceIn2DArray("score:  a", matrix);
        healthboard_pos = getLastIndexOfSequenceIn2DArray("health: a", matrix);
        levelboard_pos = getLastIndexOfSequenceIn2DArray("level:  a", matrix);
        frameboard_pos = getLastIndexOfSequenceIn2DArray("frame:  a", matrix);
    }
    public void update()
    {
        current_player!.update();

        foreach(Enemy enemy in enemies!) {
            enemy.update();
        }
        
        // nothing to update yet
        // foreach(Coin coin in coins!) {
        //     coin.update();
        // }
    }
    public void render()
    {
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                drawCell(i, j);
            }
        }

        foreach(Enemy enemy in enemies!) {
            enemy.render();
        }
        foreach(Coin coin in coins!) {
            coin.render();
        }

        current_player!.render();
    }

    public void init() {
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                colorizedOutput(i, j);

                Console.SetCursorPosition(i, j);
                Console.Write(matrix[j, i]);
            }
        }
    }

    private void drawCell(int i, int j) {
        if(matrix[j, i] != World.WALL && matrix[j, i] != World.SPACE) {
            colorizedOutput(i, j);
        }

        //Below condition ensures that the placeholder for each of the boards is not rendered and the positions for the actual value stay free
        if(notBoard(j, i, scoreboard_pos) && notBoard(j, i, healthboard_pos) && notBoard(j, i, levelboard_pos) && notBoard(j, i, frameboard_pos) && matrix[j, i] != World.WALL) {//&& ((int)current_player!.x, (int)current_player!.y) != (i, j) && (current_player!.lastPos.x, current_player!.lastPos.y) != (i, j)) {
            
            Console.SetCursorPosition(i, j);
            Console.Write(matrix[j, i]);
            //colorizedOutput(i, j);
        }
        Console.ForegroundColor = ConsoleColor.White;
    }

    private void colorizedOutput(int i, int j) {
        if ((i, j) == (frameboard_pos.x, frameboard_pos.y) && frameboard_pos != (0, 0))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(i, j);
            Console.Write(World.frametime.ToString("000"));
        }
        else
        if ((i, j) == (scoreboard_pos.x, scoreboard_pos.y))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(i, j);
            Console.Write(current_player!.score);
        }
        else
        if ((i, j) == (healthboard_pos.x, healthboard_pos.y))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(i, j);
            Console.Write(current_player!.health);
        } else
        if((i, j) == (levelboard_pos.x, levelboard_pos.y)) {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(i, j);
            Console.Write(current_player!.level);
        }
        else
        if (matrix[j, i] == World.WALL)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        if(matrix[j, i] == World.PORTAL) {
            Console.ForegroundColor = ConsoleColor.Magenta;
        }
        else
        {
            if (matrix[j, i] == World.ENEMY) {
                Console.ForegroundColor = ConsoleColor.Red;
            } else
            if (matrix[j, i] == World.COIN) {
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }
    }

    private bool notBoard(int j, int i, (int x, int y) pos) {
        if (!(i == pos.x+2 && j == pos.y) && !(i == pos.x+1 && j == pos.y) && !(i == pos.x && j == pos.y)) {
            return true;
        }
        return false;
    }
    public static World createFromFile(string filename)
    {
        
        (char[,] matrix, (int x, int y) start_pos) = scanFile(filename);
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
                        break;
                    }
                    else
                    {
                        if(y >= matrix.GetLength(0)) {
                            break;
                        } else
                        if(y == start_pos.y && x == start_pos.x) {
                            matrix[y, x] = ' ';
                        } else {
                            matrix[y, x] = (char)c;
                        }
                        x++;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR: creating world from file: " + filename + "\n" + e);
            System.Environment.Exit(1);
        }
        World w =  new World(matrix, start_pos);
        Enemy[] enemies = detectEnemies(matrix, w);
        List<Coin> coins = detectCoins(matrix, w);
    
        w.enemies = enemies;
        w.coins = coins;
        return w;
    }

    private static (char[,], (int, int)) scanFile(string filename)
    {
        char[,] matrix = new char[0, 0];
        (int x, int y) start_pos = (0, 0);
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
                        height = y;
                        matrix = new char[height+1, width+1];
                        break;
                    }
                    else
                    {
                        if(c == World.START) {
                            start_pos.x = x;
                            start_pos.y = y;
                        }
                        x++;
                    }
                }
            }
        }
        catch (Exception)
        {
            return (matrix, start_pos);
        }
        return (matrix, start_pos);
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
            if (line.Contains(sequence))
            {
                pos.x = (line.IndexOf(sequence) + sequence.Length - 1);
                pos.y = i;
            }
        }
        return pos;
    }

    private static List<Coin> detectCoins(char[,] matrix, World w) {
        List<Coin> coins = new List<Coin>();
        for(int i = 0; i < matrix.GetLength(0); i++) {
            for(int j = 0; j < matrix.GetLength(1); j++) {
                if(matrix[i, j] == World.COIN) {
                    coins.Add(new Coin(w, World.COIN, j, i));
                }
            }
        }
        return coins;
    }

    
    private static Enemy[] detectEnemies(char[,] matrix, World w) {
        Console.WriteLine("detecting enemy tracks");
        List<List<(int x, int y)>> tracks = new List<List<(int x, int y)>>();
        for(int i = 0; i < matrix.GetLength(0); i++) {
            for(int j = 0; j < matrix.GetLength(1); j++) {
                if(isInTracks(tracks, j, i)) {
                    continue;
                }
                List<(int x, int y)> track = new List<(int x, int y)>();
                char cell = matrix[i,j];
                if(cell == World.ENEMY) {
                    (int x, int y) next = (j, i);
                    while(next != (-1, -1)) {
                        track.Add(next);
                        next = nextNeighbor(track, matrix, next.x, next.y);
                    }
                    tracks.Add(track);
                }
            }
        }
        
        tracks = mergeTracks(tracks);

        Console.WriteLine($"total tracks: {tracks.Count}");

        Enemy[] enemies = new Enemy[tracks.Count];
        for(int i = 0; i < tracks.Count; i++) {

            (int x, int y)[] track = tracks[i].Distinct().ToArray();

            enemies[i] = new Enemy(w, World.ENEMY, track);
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
    public static bool isTrack(List<(int x, int y)> track, int x, int y) {
        foreach((int x, int y) pos in track) {
            if(pos.x == x && pos.y == y) {
                return true;
            }
        }
        return false;
    }
     private static (int x, int y) nextNeighbor(List<(int x, int y)> track, char[,] matrix, int x, int y) {
        if(x >= matrix.GetLength(1)-1 || y >= matrix.GetLength(0)-1) {
            return (-1, -1);
        }
        if(matrix[y+1, x] == World.ENEMY && !isTrack(track, x, y+1)) {
            return (x, y+1);
        } else 
        if(matrix[y, x+1] == World.ENEMY && !isTrack(track, x+1, y)) {
            return (x+1, y);
        } else 
        if(matrix[y-1, x] == World.ENEMY && !isTrack(track, x, y-1)) {
            return (x, y-1);
        } else 
        if(matrix[y, x-1] == World.ENEMY && !isTrack(track, x-1, y)) {
            return (x-1, y);
        } else {
            return (-1, -1);
        }
    }
    private static List<List<(int x, int y)>> mergeTracks(List<List<(int x, int y)>> tracks) {
        for(int i = 0; i < tracks.Count; i++) {
            List<(int x, int y)> track = tracks[i];
            (List<(int x, int y)> next, bool append_end) = nextTrack(tracks, track);
            if(next == null) {
                continue;
            }
            if(append_end) {
                track.AddRange(next);
            } else {
                tracks.Remove(next);
                next.AddRange(track);
                track = next;
            }
            tracks[i] = track;      
        }
        return tracks;
    }
    private static (List<(int x, int y)>, bool) nextTrack(List<List<(int x, int y)>> tracks, List<(int x, int y)> track_a) {

        foreach(List<(int x, int y)> track_b in tracks) {
            if(track_b == track_a) {
                continue;
            }

            if(track_b.Contains((track_a[^1].x, track_a[^1].y+1))) {
                return (track_b, true);
            } else 
            if(track_b.Contains((track_a[^1].x+1, track_a[^1].y))) {
                return (track_b, true);
            } else
            if(track_b.Contains((track_a[^1].x, track_a[^1].y-1))) {
                return (track_b, true);
            } else 
            if(track_b.Contains((track_a[^1].x-1, track_a[^1].y))) {
                return (track_b, true);
            } else
            if(track_b.Contains((track_a[0].x, track_a[0].y+1))) {
                return (track_b, false);
            } else 
            if(track_b.Contains((track_a[0].x+1, track_a[0].y))) {
                return (track_b, false);
            } else
            if(track_b.Contains((track_a[0].x, track_a[0].y-1))) {
                return (track_b, false);
            } else 
            if(track_b.Contains((track_a[0].x-1, track_a[0].y))) {
                return (track_b, false);
            }
        }
        return (null, false);
    }
}