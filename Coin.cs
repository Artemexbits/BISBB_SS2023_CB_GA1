// Copyright (c) 2023 Artemexbits. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.s
namespace BISBB_SS2023_CB_GA1;

class Coin : AsciiShape, IRenderable {

    public World w;
    public Coin(World w, char c, double x = 1, double y = 7) {
        this.c = c;
        this.x = x;
        this.y = y;
        this.w = w;
    }

    public void update() {

    }

    public void render() {
        w.matrix[(int)y, (int)x] = c;
    }

}