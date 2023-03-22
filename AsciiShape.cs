// Copyright (c) 2023 Artemexbits. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace BISBB_SS2023_CB_GA1;

public abstract class AsciiShape {
    public char c;
    public double x;
    public double y;
    public double vel;
    protected double xVel = 0;
    protected double yVel = 0;
    protected bool isCollision(AsciiShape shape) {
        if((int)this.x == (int)shape.x && (int)this.y == (int)shape.y) {
            return true;
        }
        return false;
    }
    protected bool isCollision(AsciiShape[] shapes) {
        foreach(AsciiShape shape in shapes) {
            if((int)this.x == (int)shape.x && (int)this.y == (int)shape.y) {
                return true;
            }
        }
        return false;
    }
}