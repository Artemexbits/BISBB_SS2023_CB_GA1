# run publish.bat to release the project to an .exe file
# released .exe file can be found in bin\Release\net7.0\win-x64\publish

# How to create worlds?
- every world needs an infoboard and a playing area enclosed by walls as '#' -> see examples in worlds/
- enemies placed as 'Z'
- all neighboring enemies will convert into an array of coordinates i.e. track to move along back and forth
- coins placed as 'O'
- portal placed as 'X'
- one world may contain many portals all having the next common target world
- by touching 'X' the player teleports to the next world in repeated order
- place <yourcustomworld>.txt files inside worlds/ directory
- the last line of every world.txt file must have a player entry point
- example: last line -> ```start:1,8``` sets entry point for player to start at the 2nd row and 9th column of this world 
