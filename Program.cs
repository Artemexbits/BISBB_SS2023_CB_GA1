// Copyright (c) 2023 Artemexbits. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace BISBB_SS2023_CB_GA1;
using System.Diagnostics;
class Program
{
    public static bool isRunning = true;
    public static readonly bool IS_MUTE = false;
    public static World? current_world;
    public static World[] worlds;
    static void Main(string[] args)
    {   
        Console.CursorVisible = false;
        bool playAgain = true;

        Menu start_menu = new Menu(new[]{"START", "INFO", "EXIT"}, option: 2, selector_h: 5);
        int start_selection = start_menu.waitForSelection();
        Console.Clear();
        if(start_selection == 3) {
            Console.WriteLine("exit selected");
            playAgain = false;
        }else
        if(start_selection == 2) {
            Console.WriteLine("infopage comming soon");
            playAgain = false;
        }

        while(playAgain) {
            isRunning = true;
            string[] w_files = new string[0];
            try {
                string w_dir = Directory.GetCurrentDirectory().ToString()+@"\worlds\";
                w_files = Directory.GetFiles(w_dir, "*.txt");
            } catch (Exception e) {
                Console.WriteLine("ERROR: directory worlds/ not found");
                System.Environment.Exit(1);
            }

            worlds = new World[w_files.Length];
            for(int i = 0; i < worlds.Length; i++) {
                Console.WriteLine($"file_{i}: " + w_files[i]);
                worlds[i] = World.createFromFile(w_files[i]);
            }

            Player p1 = new Player(worlds[0], '@');
            worlds[0].current_player = p1;
            current_world = worlds[0];

            Program.current_world!.current_player!.x = Program.current_world!.start_pos.x;
            Program.current_world!.current_player!.y = Program.current_world!.start_pos.y;
            Console.Clear();
            double time_alive = 0;
            try
            {
                DateTime begin_time = DateTime.Now;
                Program.current_world.init();
                Stopwatch watch = new Stopwatch();
                //int time_count = 0;
                while (isRunning)
                {
                    watch.Start();
                    current_world.update();
                    current_world.render();
                    watch.Stop();
                    World.frametime = (short)watch.Elapsed.TotalMilliseconds;
                    watch.Reset();
                }
                DateTime end_time = DateTime.Now;

                time_alive = (end_time-begin_time).Duration().TotalSeconds;
            }
            catch (Exception e)
            {
                Console.Clear();
                // Console.WriteLine("matrix length0:" + current_world.matrix.GetLength(0));
                // Console.WriteLine("matrix length1:" + current_world.matrix.GetLength(1));
                // Console.WriteLine("last y pos: " + current_world.current_player.y);
                // Console.WriteLine("last x pos: " + current_world.current_player.x);
                // Console.WriteLine("ERROR: " + e);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: your console window is probably too small to play this world!");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(3000);
                playAgain = false;
                continue;
            }
            
            Console.Clear();
            if (current_world.current_player.health <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("YOU FAILED!");
                playLosingBeep();
            }
            else
            if (current_world.current_player.score >= 50)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("YOU WON!");
                playWinningBeep();
            }else
            if (current_world.current_player.health != 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("YOU WON!");
                playWinningBeep();
            }
            Console.ForegroundColor = ConsoleColor.White;

            Thread.Sleep(1000);
            
            Menu end_menu = new Menu(new[]{"RESTART", "STATS", "EXIT"}, option: 2, selector_h: 5);
            int end_selection = end_menu.waitForSelection();
            
            if(end_selection == 1) {
                playAgain = true;
            } else
            if(end_selection == 3) {
                playAgain = false;
            } else
            if(end_selection == 2) {
                Console.Clear();
                printStats(current_world.current_player.score, current_world.current_player.health, current_world.current_player.level, time_alive);
                playAgain = false;
            }
            
        }
    }

    private static void printStats(int score, int health, int level, double time_alive) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"SCORE:  {score}");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"HEALTH: {health}");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"LEVEL:  {level}");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"ALIVE:  {time_alive.ToString("0.00")} seconds");
        Console.ForegroundColor = ConsoleColor.White;
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
