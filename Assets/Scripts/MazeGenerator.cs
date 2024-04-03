using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour{

    [Range(10, 200)]
    public int mazeWidth, mazeHeight;    // Dimensions of the maze
    public int startX = 0, startY = 0;                  // Start position of the algorithm
    MazeCell[,] maze;                           // An array of MazeCell representing the maze grid

    Vector2Int currentCell;                     // The maze cell we are currently looking at (next cell).

    public MazeCell[,] GetMaze(){
        // Create objet of MazeCell
        Debug.Log(mazeWidth + "x" + mazeHeight);
        maze = new MazeCell[mazeWidth, mazeHeight];
        
        for(int x = 0; x < mazeWidth; x++){
            for(int y = 0; y < mazeHeight; y++){
        
                maze[x, y] = new MazeCell(x, y);
        
            }
        }

        CarvePath(startX, startY);

        return maze;
        
    }

    List<Direction> directions = new List<Direction>{

        Direction.Up, Direction.Down, Direction.Left, Direction.Right,
    
    };

    List<Direction> GetRandomDirections(){

        // Create a copy of our directions list that we can mess around with
        List<Direction> dir = new List<Direction>(directions);

        // Make a directions list to put our randomised directions into
        List<Direction> rndDir = new List<Direction>();

        while(dir.Count > 0){                       // Loop until rndDir list is empty

            int rnd = Random.Range(0, dir.Count);    // Get randonm index in list
            rndDir.Add(dir[rnd]);                   // Add the random direction to our list
            dir.RemoveAt(rnd);                      // Remove that direction so we cant choose it again
        }

        // When we've got all direction in a random order, retunr the queue
        return rndDir;

    }

    bool IsCellValid (int x, int y){

        // If cell is outside of the map or has already been visited it is considered not valid
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight -1 || maze[x, y].visited) return false; 
        else return true;

    }

    Vector2Int CheckNeighbours(){

        List<Direction> rndDir= GetRandomDirections();
        
        for(int i = 0; i < rndDir.Count; i++){

            // Set neighbour coordinates to current cell for now
            Vector2Int neighbour = currentCell;

            // Modify neighbor coordinates bases on the random directions we're currently trying
            switch (rndDir[i]){

                case Direction.Up:
                    neighbour.y++;
                    break;
                case Direction.Down:
                    neighbour.y--;
                    break;
                case Direction.Right:
                    neighbour.x++;
                    break;
                case Direction.Left:
                    neighbour.x--;
                    break;
            }

            // If the neighbor we just tried is valid, we can return that neighbour. If not,we go again
            if(IsCellValid(neighbour.x, neighbour.y)) return neighbour;
        }

        // If we tried all directions and didnt find a valid neighbour, we return the currentCell values
        return currentCell;
    }

    // Takes in two maze positionsa and sets the cells accordingly
    void BreakWalls (Vector2Int primaryCell, Vector2Int secondaryCell){

        // We can only go in one direction at a time so we can handle this using if-else statement
        if(primaryCell.x > secondaryCell.x){ // Primary Cell's left wall
            
            maze[primaryCell.x, primaryCell.y].leftWall = false;
        
        }else if(primaryCell.x < secondaryCell.x){ // Secondary Cell's left wall
            
            maze[secondaryCell.x, secondaryCell.y].leftWall = false;
        
        }else if(primaryCell.y < secondaryCell.y){ // Primary Cell's top wall
            
            maze[primaryCell.x, primaryCell.y].topWall = false;
        
        }else if(primaryCell.x > secondaryCell.x){ // Secondary Cell's top wall
            
            maze[secondaryCell.x, secondaryCell.y].topWall = false;
        
        }
    }

    // Starting at the x, y passed in, carves a path trhough the maze until it encounters a dead end
    // a dead end is a cell with no valid neighbours
    void CarvePath(int x, int y){

        // Perform a quick check to make sure out start position is within the boundries of the map
        // if not, set them to a default (Using 0) and throw a warning
        if(x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1){
            
            x = y = 0;
            Debug.LogWarning("Startin position is out of bounds, default to 0, 0");

        }

        // Set current cell to the starting position we were passed
        currentCell = new Vector2Int(x, y);

        // A list to keep track of our current path
        List<Vector2Int> path = new List<Vector2Int>();

        // Loop until we encounter a dead end
        bool deadEnd = false;
        while (!deadEnd){
            // Get the next cell we're going to try
            Vector2Int nextCell = CheckNeighbours();

            // If that cell has no valid neighbours, set dead end to true so we brak out of the loop
            if(nextCell == currentCell){
                // If that cell has no valid neighbors, set dead end to true so we break out of the loop
                for(int i = path.Count - 1; i >= 0; i--){
                    
                    currentCell = path[i];                           // Set currentCell to the next step back along our path
                    path.RemoveAt(i);                                // Remove this step from the path
                    nextCell = CheckNeighbours();                   // Check that cell to see if amy other neighbours are valid
                
                    // If we find a valid neighbour, break out of the loop
                    if(nextCell != currentCell) break;

                }

                if(nextCell == currentCell){
                    deadEnd = true;
                }
            }else{

                BreakWalls(currentCell, nextCell);                  // Set wall flags on these two cells
                maze[currentCell.x, currentCell.y].visited = true;  // Set cell to visited before moving on
                currentCell = nextCell;                             // Set the current cell to the valid neighbour we found  
                path.Add(currentCell);                              // Add this cell to out path

            }
        }
    }
}
    
public enum Direction{

    Up,
    Down,
    Left,
    Right

}

public class MazeCell
{
    // Atributes
    public bool visited;
    public int x, y;

    public bool topWall;
    public bool leftWall;

    // Returns x and y as a Vector2Int
    public Vector2Int position
    {
        get
        {

            return new Vector2Int(x, y);

        }
    }
    //Constructor for maze cell
    public MazeCell(int x, int y)
    {

        // Coordinates of this cell in the maze grid.
        this.x = x;
        this.y = y;

        // Whether the algorithms has visited this cell or not - false as default
        visited = false;

        // All wall present until algorothim removes them.
        topWall = leftWall = true;
    }
}

public class SafeAreaCell
{
    // Atributes
    public int x, y;

    // Returns x and y as a Vector2Int
    public Vector2Int position
    {
        get
        {
            return new Vector2Int(x, y);
        }
    }
    //Constructor for safe area cell
    public SafeAreaCell(int x, int y)
    {
        // Coordinates of this cell in the maze grid.
        this.x = x;
        this.y = y;
    }
}
