namespace BISBB_SS2023_CB_GA1;
class Program
{
    public static bool isRunning = true;
    public static World? current_world;
    public static World? w1;
    public static World? w2;
    static void Main(string[] args)
    {   
        Console.CursorVisible = false;
        w1 = World.createFromFile("worlds/test/world1.txt", (1, 8));
        w2 = World.createFromFile("worlds/test/level_0.txt", (1, 7));

        (int x, int y)[] e1_1_track = {(5, 9), (6, 9), (7, 9), (8, 9), (8, 10), (8, 11), (9, 11), (10, 11)};
        Enemy e1_1 = new Enemy(w1, 'Z', e1_1_track);

        (int x, int y)[] e1_2_track = {(5, 8), (5, 9), (5, 10), (5, 11), (4, 11), (3, 11), (2, 11), (1, 11), (1, 12)};
        Enemy e1_2 = new Enemy(w2, 'Z', e1_2_track);

        (int x, int y)[] e2_2_track = {(1, 13), (2, 13), (3, 13), (4, 13), (5, 13)};
        Enemy e2_2 = new Enemy(w2, 'Z', e2_2_track);

        Player p1 = new Player(w1, '@');

        w1.enemies = new Enemy[]{e1_1};
        w2.enemies = new Enemy[]{e1_2, e2_2};

        w1.current_player = p1;
        current_world = w1;

        Program.current_world!.current_player.x = Program.current_world!.start_pos.x;
        Program.current_world!.current_player.y = Program.current_world!.start_pos.y;
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
            if (current_world.current_player.score >= 10)
            {
                Console.WriteLine("You won!");
                playWinningBeep();
            }
        }
    }

    private static void playWinningBeep()
    {
        Console.Beep(400, 400);
        Console.Beep(300, 400);
        Console.Beep(500, 400);
    }
    private static void playLosingBeep()
    {
        Console.Beep(500, 400);
        Console.Beep(300, 400);
        Console.Beep(400, 400);
    }
}
