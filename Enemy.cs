namespace BISBB_SS2023_CB_GA1;

class Enemy : AsciiShape, IRenderable {
    private World w;
    private (int x, int y)[] track;
    private int track_count = 0;
    public Enemy(World w, char c, (int x, int y)[] track, double vel = 1.0) {
        this.w = w;
        this.c = c;
        this.x = track[0].x;
        this.y = track[0].y;
        this.vel = vel;
        this.track = track;
    }
    public void update() {
        // if (w.matrix[(int)(y + yVel), (int)x] != World.WALL)
        // {
        //     y += yVel;
        // }
        // if (w.matrix[(int)y, (int)(x + xVel)] != World.WALL)
        // {
        //     x += xVel;
        // }
        (int x, int y) pos = track[track_count];
        x = pos.x;
        y = pos.y;

        track_count += (int)vel;
        if(track_count+(int)vel >= track.Length || track_count <= 0) {
             vel *= -1;
        }
    }
    public void render() {
        // Console.SetCursorPosition((int)x, (int)y);
        // Console.Write(c);
        w.matrix[(int)y, (int)x] = c;
        
        foreach((int x, int y) p in track) {
            if(!(p.x == x && p.y == y)) {
                w.matrix[p.y, p.x] = ' ';
            }
        }
    }
}