using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject MazeCellPrefab;

    // This is the physical size of the cells. Getting this wrong will result in overlapping
    // or visible gaps between each cell
    public float CellSize = 1f;

    private void Start(){

        // Get the MazeGenerator to creaate a maze
        MazeCell[,] maze = mazeGenerator.GetMaze();

        // Lopp trhough every cell in the maze
        for (int x = 0; x < mazeGenerator.mazeWidth; x++){
            for (int y = 0; y < mazeGenerator.mazeHeight; y++){

                // Instantiate a new maze cell prefab of the MazeRenderer object
                GameObject newCell = Instantiate(MazeCellPrefab, new Vector3((float)x * CellSize, 0f, (float)y * CellSize), Quaternion.identity, transform);
                
                // Get reference to the cell's MazeCellPrefabscript
                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();
                
                // Determine which walls need to be active. Onli top and left walls are activated
                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;

                // Bottom and right walls are deactivated by default unless we are at the buttom or right
                // edge of the maze
                bool right = false;
                bool bottom = false;
                if (x == mazeGenerator.mazeWidth - 1) right = true;
                if (y == 0) bottom = true;

                // Drop walls on the middle of the map 5x5
                // Safe are is also added along the area
                int planeFieldWL = 5;
                int middleX = (int)(mazeGenerator.mazeWidth / 2) - (planeFieldWL/2);
                int middleY = (int)(mazeGenerator.mazeHeight / 2) - (planeFieldWL/2);
                if (x >= middleX && x <= (middleX + planeFieldWL) && y >= middleY && y <= (middleY + planeFieldWL))
                {
                    mazeCell.Init(false, false, false, false, true);
                }
                else
                {
                    mazeCell.Init(top, bottom, right, left, false);
                }
            }
        }
    }

}
