// Copyright (c) 2023 Artemexbits. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace BISBB_SS2023_CB_GA1;
class Player : AsciiShape, IRenderable
{
    private World w;
    public int score = 0;
    public int health = 9;
    public int level = 1;
    private int beep = 0;
    private ConsoleKey lastKey;
    public Player(World w, char c, double x = 1, double y = 7, double vel = 1.0)
    {
        this.w = w;
        this.c = c;
        this.x = x;
        this.y = y;
        this.vel = vel;

        Thread beeperThread = new Thread(new ThreadStart(beeper));
        beeperThread.Start();

        Thread inputThread = new Thread(new ThreadStart(handleInput));
        inputThread.Start();
    }

    public void update()
    {
        if (w.matrix[(int)(y + yVel), (int)x] != World.WALL)
        {
            y += yVel;
        }
        if (w.matrix[(int)y, (int)(x + xVel)] != World.WALL)
        {
            x += xVel;
        }

        if (w.matrix[(int)y, (int)x] == World.COIN)
        {
            w.matrix[(int)y, (int)x] = ' ';
            score++;
            beep = 1;
        }
        if (w.matrix[(int)y, (int)x] == World.ENEMY)
        {
            w.matrix[(int)y, (int)x] = ' ';
            health--;
            beep = 2;
        }
        if(w.matrix[(int)y, (int)x] == World.PORTAL) {
            Console.Clear();
            nextLevel();
        }

        if (health <= 0 || score >= 50)
        {
            Program.isRunning = false;
        }
    }
    public void render()
    {
        Console.SetCursorPosition((int)x, (int)y);
        Console.Write(c);
    }

    private void handleInput()
    {
        while (Program.isRunning)
        {
            ConsoleKeyInfo k = Console.ReadKey(true);
            
            switch (k.Key)
            {
                case ConsoleKey.W:
                    if (lastKey == ConsoleKey.S && yVel != 0)
                    {
                        yVel = 0;
                        xVel = 0;
                    }
                    else
                    {
                        yVel = -vel*0.5;
                        xVel = 0;
                    }

                    lastKey = ConsoleKey.W;
                    break;
                case ConsoleKey.A:
                    if (lastKey == ConsoleKey.D && xVel != 0)
                    {
                        yVel = 0;
                        xVel = 0;
                    }
                    else
                    {
                        xVel = -vel;
                        yVel = 0;
                    }
                    lastKey = ConsoleKey.A;
                    break;
                case ConsoleKey.S:
                    if (lastKey == ConsoleKey.W && yVel != 0)
                    {
                        yVel = 0;
                        xVel = 0;
                    }
                    else
                    {
                        yVel = +vel*0.5;
                        xVel = 0;
                    }
                    lastKey = ConsoleKey.S;
                    break;
                case ConsoleKey.D:
                    if (lastKey == ConsoleKey.A && xVel != 0)
                    {
                        yVel = 0;
                        xVel = 0;
                    }
                    else
                    {
                        xVel = +vel;
                        yVel = 0;
                    }
                    lastKey = ConsoleKey.D;
                    break;
            }
            
        }
    }

    private void beeper()
    {
        while (Program.isRunning)
        {
            if(Program.IS_MUTE) {
                beep = 0;
            } else
            if (beep == 1)
            {   
                Console.Beep(500, 400);
                beep = 0;
            } else
            if (beep == 2)
            {
                Console.Beep(300, 400);
                beep = 0;
            }
        }
    }

    private void nextLevel() {
        if(this.level <= 0) {
            return;
        } else
        if(this.level >= Program.worlds.Length) {
            this.level = 0;
        }
        this.level++;
        Program.current_world = Program.worlds[level-1];
        this.w = Program.worlds[level-1];
        
        Program.current_world!.current_player = this;
        Program.current_world!.current_player.x = Program.current_world!.start_pos.x;
        Program.current_world!.current_player.y = Program.current_world!.start_pos.y;

        if (Program.current_world!.current_player.health < 8) {
            Program.current_world!.current_player.health+=2;
        }
        
        // switch(this.level) {
        //     case 1:  
        //         Program.current_world = Program.w2;
        //         this.w = Program.w2!;
        //         this.level = 2;
        //         break;
        //     case 2:
        //         Program.current_world = Program.w1;
        //         this.w = Program.w1!;
        //         this.level = 1;
        //         break;
        // }
        
    }
}