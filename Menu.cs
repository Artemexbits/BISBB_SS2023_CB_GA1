namespace BISBB_SS2023_CB_GA1;

class Menu {
    public static readonly short START = 1;
    public static readonly short INFO = 2;
    public static readonly short EXIT = 3;
    private int option = 1;
    private (string text, int x, int y)[] items;
    private char[,] matrix = new char[9, 15];
    private int last_option;

    public Menu(int option) {
        this.option = option;
        initItems();
        last_option = items!.Length;
    }
    private void initItems() {
        items = new (string text, int x, int y)[] {("START", 5, 1), ("INFO", 5, 4), ("EXIT", 5, 7)};
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
        Console.ForegroundColor = ConsoleColor.Blue;
        for(int i = 0; i < matrix.GetLength(0); i++) {
            for(int j = 0; j < matrix.GetLength(1); j++) {
                if(i == (option*3-3) || i == (2+option*3-3)) {
                    Console.SetCursorPosition(j, i);
                    Console.Write('#');
                } else
                if(((option*3-3) < i && i < (2+option*3-3)) && (j == 0 || j == matrix.GetLength(1)-1)) {
                    Console.SetCursorPosition(j, i);
                    Console.Write('#');
                }
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
}