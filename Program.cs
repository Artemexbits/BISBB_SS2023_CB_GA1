// Copyright (c) 2023 Artemexbits. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace BISBB_SS2023_CB_GA1;
class Program
{
    public static bool isRunning = true;
    public static readonly bool IS_MUTE = true;
    public static World? current_world;
    public static World[] worlds;
    static void Main(string[] args)
    {   
        string[] w_files = new string[0];
        try {
            string w_dir = Directory.GetCurrentDirectory().ToString()+@"\worlds\";
            w_files = Directory.GetFiles(w_dir, "*.txt");
        } catch (Exception e) {
            Console.WriteLine("ERROR: dir worlds/ not found");
            System.Environment.Exit(1);
        }
        Console.CursorVisible = false;

        worlds = new World[w_files.Length];
        for(int i = 0; i < worlds.Length; i++) {
            worlds[i] = World.createFromFile(w_files[i], (1, 8));
        }

        // worlds = new World[] {World.createFromFile("worlds/theAmaze_cold.txt", (1, 8)),
        //                       World.createFromFile("worlds/theAmaze_hot.txt", (1, 7))};

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
}
