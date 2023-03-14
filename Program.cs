namespace BISBB_SS2023_CB_GA1;
class Program
{
    public static bool isRunning = true;
    public static World current_world;
    public static World w1;
    public static World w2;
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        w1 = World.createFromFile("world1.txt");
        w2 = World.createFromFile("world2.txt");
        Player p1 = new Player(w1, '@');
        w1.current_player = p1;
        current_world = w1;
        Console.Clear();
        try
        {
            while (isRunning)
            {
                current_world.render();
                current_world.update();
                Thread.Sleep(20);
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
