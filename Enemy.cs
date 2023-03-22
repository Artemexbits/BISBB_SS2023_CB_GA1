// Copyright (c) 2023 Artemexbits. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace BISBB_SS2023_CB_GA1;

public class Enemy : AsciiShape, IRenderable {
    private World w;
    private (int x, int y)[] track;
    private int track_count;
    public Enemy(World w, char c, (int x, int y)[] track, double vel = 1.0) {
        if(track.Length - 2 > 0) {
            track_count = new Random().Next(track.Length - 2);
        } else {
            track_count = 0;
        }
        
        this.w = w;
        base.c = c;
        base.x = track[track_count].x;
        base.y = track[track_count].y;
        base.vel = vel;
        this.track = track;
    }
    public void update() {
        //TODO next-neighbour-movement mechanism
        if(track.Length > 1) {
            (int x, int y) pos = track[track_count];

            x = pos.x;
            y = pos.y;
            
            track_count += (int)vel;
            if(track_count+(int)vel >= track.Length || track_count <= 0) {
                vel *= -1;
            }
        }
    }
    public void render() {
        w.matrix[(int)y, (int)x] = c;
        
        foreach((int x, int y) p in track) {
            if(!(p.x == x && p.y == y)) {
                w.matrix[p.y, p.x] = ' ';
            }
        }
    }
}