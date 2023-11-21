using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class CreateWalls : MonoBehaviour // CREATING MAZE THIS WALLS AND DETH ZONES
{
    public int mazeSize; // maze size
    public int[,] maze = new int[30, 30]; // The maze ( 0 - free, 1 - wall, 2 - deth)
    public GameObject wallGameOBJ, dethZoneGameOBJ; // Referenses to the objects 
    // Start is called before the first frame update

    public int[,] mazeSolver = new int[30, 30]; // Needs to find the shortest path
    public int[,] path = new int[30, 30]; // This is the path 

    bool valid(int i,int j) { // Check if coords are valid 
        if (i>=mazeSize || j>=mazeSize || i<0 || j<0) return false;
        return true;
    }

    void findTheShortestPath(int i, int j) { // This is need to find the shortest path in the maze 
        if (!valid(i, j)) return;
        int curStep = mazeSolver[i, j];
        path[i, j] = curStep;
        if (valid(i + 1, j) && mazeSolver[i + 1, j] == curStep - 1) 
            findTheShortestPath(i + 1, j);
       
        if (valid(i - 1, j) && mazeSolver[i - 1, j] == curStep - 1) 
            findTheShortestPath(i - 1, j);
        
        if (valid(i, j + 1) && mazeSolver[i, j + 1] == curStep - 1) 
            findTheShortestPath(i, j + 1);
        
        if (valid(i, j - 1) && mazeSolver[i, j - 1] == curStep - 1) 
            findTheShortestPath(i, j - 1);
    }

    void thereIsApath(int pref,int i,int j) { // This is wave algorithm to solve the maze 
        pref++;
        if (!valid(i, j)) return; // is coords is not valid we have to break the func
        if (maze[i, j] == 0 && (mazeSolver[i,j] > pref || mazeSolver[i,j]==0) ) { // if there is no wall and we didn`t be here so we can go to that cookds
            mazeSolver[i, j] = pref; // set steps number to reach this coords
            if (i == mazeSize - 1 && j == mazeSize - 1) return; // if we are at the top right corner so we have reached our goal

            // run func at all neighbour cells 

            thereIsApath(pref, i + 1, j); 
            thereIsApath(pref, i - 1, j);
            thereIsApath(pref, i, j + 1);
            thereIsApath(pref, i, j - 1);
        }
        return;
    }

    public void createMaze() { // creating the maze
        GameObject[] obj; // array to store previous maze walls and deth zones

        obj = GameObject.FindGameObjectsWithTag("Cube(Clone)"); // finding all wallls 

        foreach (GameObject ob in obj) { // delete them
            Destroy(ob);
        }

        obj = GameObject.FindGameObjectsWithTag("dethZone"); // finding all deth zones
        foreach (GameObject ob in obj) { // delete them
            Destroy(ob);
        }
        while (true) { // This while is for making solvable maze
            for (int i = 0; i < mazeSize; i++) { // Creating starting maze 
                for (int j = 0; j < mazeSize; j++) {
                    mazeSolver[i, j] = 0; // set to 0 for normal path find
                    path[i, j] = 0; // set to 0 for normal path with no colisions with another maze
                    int R = Random.Range(0, 100);
                    float rand = R / 100f; 
                    maze[i, j] = 0; // set 0 to maze. It means at position [i,j] there is no wall and no deth zones
                    if (rand > 0.45f) { // The chanse to create a wall = 55% (1-0.45f) 
                        if (i == 0 && j == 0) continue;
                        if (i == mazeSize - 1 && j == mazeSize - 1) continue;
                        maze[i, j] = 1; // set 1 to maze. It means at position [i,j] there is a wall
                    }

                }

            }
            mazeSolver[0, 0] = 0; // in case if at position [0,0] wall 
            thereIsApath(0, 0, 0); // Thying to find the path
            if (mazeSolver[mazeSize - 1, mazeSize - 1] != 0) break; // If finish square is non 0, that means the maze is solvable  
        }
        // loop for all maze to spawn walls and deth zones
        for (int i = 0; i < mazeSize; i++) {
            for (int j = 0; j < mazeSize; j++) {

                if (maze[i, j] == 1) { // if it is 1, i spawn a wall
                    GameObject spawn = Instantiate(wallGameOBJ); // it is just an wall object
                    spawn.transform.position = new Vector3(-9.85f + (i), 1.44f, -7.35f + (j)); // Creating walls in the maze 
                } 
                else {
                    int R = Random.Range(0, 100);
                    float rand = R / 100f;
                    if (rand > 0.85f) { // Chance to be a deth zone = 15%
                        
                        if (i == 0 && j == 0) continue;
                        if (i == 1 && j == 0) continue;
                        if (i == 0 && j == 1) continue;
                        if (i == 2 && j == 0) continue;
                        if (i == 0 && j == 2) continue;
                        if (i == 1 && j == 1) continue;
                        if (i == 2 && j == 2) continue;
                        if (i == mazeSize - 1 && j == mazeSize - 1) continue;
                        
                        maze[i, j] = 2; // set 2. So it means at position [i,j] deth zone
                        GameObject spawn = Instantiate(dethZoneGameOBJ);  // it is just an deth zone object
                        spawn.transform.position = new Vector3(-9.85f + (i), 0.5f, -7.35f + (j)); // Creating deth zones in the maze
                    }
                }
            }
        }
        maze[0, 0] = 0;
        maze[1, 0] = 0;
        maze[0, 1] = 0;
        findTheShortestPath(mazeSize - 1, mazeSize - 1); // Find the shortest path
    }

}
