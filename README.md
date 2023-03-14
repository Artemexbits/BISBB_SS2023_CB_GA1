# run publish.bat to release the project to an .exe file
# released .exe file can be found in bin\Release\net7.0\win-x64\publish

# How to create worlds?
- every world needs an infoboard and a playing area enclosed by walls as '#' -> see examples in worlds/
- enemies placed as 'Z'
- all neighboring enemies will convert into an array of coordinates i.e. track to move along back and forth
- coins placed as 'O'
- portal placed as 'X'
- one world may contain many portals all having the next common target world
- by touching 'X' the player teleports to the next world in repeated order defined in Program.cs
- place <yourcustomworld>.txt inside worlds/ directory
- inside Program.cs set new array containing paths to custom worlds and player starting positions (x, y) beginning at index 0
- example:
  ``` 
  worlds = new World[] {World.createFromFile("worlds/<yourcustomworld1>.txt", (1, 8)),
                        World.createFromFile("worlds/<yourcustomworld2>.txt", (1, 7))};
  ```
