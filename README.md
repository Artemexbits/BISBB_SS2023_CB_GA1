# run release.bat to release the project to an .exe file

# How to create worlds?
- every world needs an infoboard and a playing area enclosed by walls as '#' -> see examples in worlds/
- enemies placed as 'Z'
- all neighboring enemies will convert into an array of coordinates i.e. track to move along back and forth
- coins placed as 'O'
- exit-portal placed as 'X'
- entry-portal placed as 'Y'
- one world may contain many exit-portals all having the next common target world
- by touching 'X' the player teleports to the next world to the position of the entry-portal 'Y' in repeated order
- place <yourcustomworld>.txt files inside worlds/ directory
- every world needs an entry point and exit point!
``` ewewewew```
