// Copyright (c) 2023 Artemexbits. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace BISBB_SS2023_CB_GA1;

public class Menu {
    private int option;
    private int selector_w;
    private int selector_h;
    private (string text, int x, int y)[] items;
    private char[,] matrix;

    public Menu(string[] items, int option = 1, int selector_w = 15, int selector_h = 3) {
        this.option = option;
        this.selector_w = selector_w;
        this.selector_h = selector_h;
        initItems(items);
        matrix = new char[items!.Length*selector_h, selector_w];
    }
    private void initItems(string[] items) {
        this.items = new (string text, int x, int y)[items.Length];
        for(int i = 0; i < items.Length; i++) {
            this.items[i] = (items[i], (selector_w)/2-(items[i].Length)/2, (selector_h/2)+selector_h*i);
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
                    if(option < items.Length) {
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
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
}