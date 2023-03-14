// Copyright (c) 2023 Artemexbits. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace BISBB_SS2023_CB_GA1;

class Menu {
    public static readonly short START = 1;
    public static readonly short INFO = 2;
    public static readonly short EXIT = 3;
    private int option;
    private int selector_w;
    private int selector_h;
    private int item_margin_left;
    private (string text, int x, int y)[] items;
    private char[,] matrix;
    private int last_option;

    public Menu(string[] items, int option = 1, int selector_w = 15, int selector_h = 3, int item_margin_left = 5) {
        this.option = option;
        this.selector_w = selector_w;
        this.selector_h = selector_h;
        this.item_margin_left = item_margin_left;
        initItems(items);
        last_option = items!.Length;
        matrix = new char[last_option*selector_h, selector_w];
    }
    private void initItems(string[] items) {
        this.items = new (string text, int x, int y)[items.Length];
        for(int i = 0; i < items.Length; i++) {
            this.items[i] = (items[i], item_margin_left, (selector_h/2)+selector_h*i);
        }
    }
    public int waitForSelection() {
        ConsoleKeyInfo k;
        Console.Clear();
        printSelector();
        printItems();
        do {
            k = Console.ReadKey(true);
            switch(k.Key) {
                case ConsoleKey.S:
                    if(option < last_option) {
                        option++;
                    }
                    break;
                case ConsoleKey.W:
                    if(option > 1) {
                        option--;
                    }
                    break;
            }
            printSelector();
            printItems();
        } while(k.Key != ConsoleKey.Enter);
        Console.Clear();
        return option;
    }

    private void printItems() {
        for(int i = 0; i < items.Length; i++) {
            (string text, int x, int y) item = items[i];
            if(i == option-1) {
                Console.SetCursorPosition(item.x, item.y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(item.text);
            } else {
                Console.SetCursorPosition(item.x, item.y);
                Console.Write(item.text);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    private void printSelector() {
        Console.Clear();
        ConsoleColor selector_color = ConsoleColor.Blue;
        Console.ForegroundColor = selector_color;
        for(int i = 0; i < matrix.GetLength(0); i++) {
            for(int j = 0; j < matrix.GetLength(1); j++) {
                if(i == (option*selector_h-selector_h) || i == ((selector_h-1)+option*selector_h-selector_h)) {
                    Console.SetCursorPosition(j, i);
                    Console.Write('#');
                } else
                if(((option*selector_h-selector_h) < i && i < ((selector_h-1)+option*selector_h-selector_h)) && (j == 0 || j == matrix.GetLength(1)-1)) {
                    Console.SetCursorPosition(j, i);
                    Console.Write('#');
                }

                // if(((option*selector_h-selector_h) < i && i < ((selector_h-1)+option*selector_h-selector_h)) && j == matrix.GetLength(1)-1) {
                //     Console.SetCursorPosition(j+2, i);
                //     Console.ForegroundColor = ConsoleColor.Green;
                //     Console.Write("ENTER");
                //     Console.ForegroundColor = selector_color;
                // }
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
}