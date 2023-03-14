namespace BISBB_SS2023_CB_GA1;
class Program
{
    public static bool isRunning = true;
    public static readonly bool IS_MUTE = true;
    public static World? current_world;
    public static World[] worlds;
    static void Main(string[] args)
    {   
        Console.CursorVisible = false;
        worlds = new World[] {World.createFromFile("worlds/theAmaze_cold.txt", (1, 8)),
                              World.createFromFile("worlds/theAmaze_hot.txt", (1, 7))};

        Player p1 = new Player(worlds[0], '@');
        worlds[0].current_player = p1;
        current_world = worlds[0];

        Program.current_world!.current_player!.x = Program.current_world!.start_pos.x;
        Program.current_world!.current_player!.y = Program.current_world!.start_pos.y;
        Console.Clear();
        try
        {
            while (isRunning)
            {
                current_world.render();
                current_world.update();
                Thread.Sleep(50);
            }
        }
        catch (Exception)
        {
            Console.Clear();
            Console.WriteLine("length0:" + current_world.matrix.GetLength(0));
            Console.WriteLine("length1:" + current_world.matrix.GetLength(1));
            Console.WriteLine("attempted y index: " + current_world.current_player.y);
            Console.WriteLine("attempted x index: " + current_world.current_player.x);
        }
        finally
        {
            Console.Clear();
            if (current_world.current_player.health <= 0)
            {
                Console.WriteLine("You failed!");
                playLosingBeep();
            }
            else
            if (current_world.current_player.score >= 50)
            {
                Console.WriteLine("You won!");
                playWinningBeep();
            }
        }
    }

    private static void playWinningBeep()
    {
        if(!IS_MUTE) {
            Console.Beep(400, 400);
            Console.Beep(300, 400);
            Console.Beep(500, 400);
        }
    }
    private static void playLosingBeep()
    {
        if(!IS_MUTE) {
            Console.Beep(500, 400);
            Console.Beep(300, 400);
            Console.Beep(400, 400);
        }
    }

    private static Enemy[] initEnemies(World w) {
        Enemy[] enemies;

        Enemy e1 = new Enemy(w, 'Z', new (int x, int y)[]{(5, 13), (5, 14), (5, 15), (5, 16), (5, 17), (5, 18)});
        Enemy e2 = new Enemy(w, 'Z', new (int x, int y)[]{(8, 13), (8, 14), (8, 15), (8, 16), (8, 17), (8, 18)});



        enemies = new Enemy[] {e1, e2};

        return enemies;
    }
}
