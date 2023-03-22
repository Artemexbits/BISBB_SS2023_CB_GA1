// Copyright (c) 2023 Artemexbits. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.s
namespace BISBB_SS2023_CB_GA1;

public class Coin : AsciiShape, IRenderable {

    private World w;
    public Coin(World w, char c, double x = 1, double y = 7) {
        base.c = c;
        base.x = x;
        base.y = y;
        this.w = w;
    }

    public void update() {

    }

    public void render() {
        w.matrix[(int)y, (int)x] = c;
    }

}