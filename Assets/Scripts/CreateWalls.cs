using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class CreateWalls : MonoBehaviour
{
    public int n;
    public int[,] arr = new int[30, 30]; // The maze ( 0 - free, 1 - wall, 2 - deth)
    public GameObject wall, dethZone; // Referenses to the objects 
    // Start is called before the first frame update

    public int[,] steps = new int[30, 30]; // Needs to find the shortest path
    public int[,] path = new int[30, 30]; // This is the path 

    bool valid(int i,int j) { // Check if coords are valid 
        if (i>=n || j>=n || i<0 || j<0) return false;
        return true;
    }

    void remakeSteps(int i, int j) { // This is need to find the shortest path in the maze 
        if (!valid(i, j)) return;
        int curStep = steps[i, j];
        path[i, j] = curStep;
        if (valid(i + 1, j) && steps[i + 1, j] == curStep - 1) 
            remakeSteps(i + 1, j);
       
        if (valid(i - 1, j) && steps[i - 1, j] == curStep - 1) 
            remakeSteps(i - 1, j);
        
        if (valid(i, j + 1) && steps[i, j + 1] == curStep - 1) 
            remakeSteps(i, j + 1);
        
        if (valid(i, j - 1) && steps[i, j - 1] == curStep - 1) 
            remakeSteps(i, j - 1);
    }

    void bfs(int pref,int i,int j) { // This is wave algorithm to solve the maze 
        pref++;
        if (!valid(i, j)) return;
        if (arr[i, j] == 0 && (steps[i,j] > pref || steps[i,j]==0) ) {
            steps[i, j] = pref;
            if (i == n - 1 && j == n - 1) return;
            bfs(pref, i + 1, j);
            bfs(pref, i - 1, j);
            bfs(pref, i, j + 1);
            bfs(pref, i, j - 1);
        }
        return;
    }

    public void createMaze() {
        GameObject[] obj;

        obj = GameObject.FindGameObjectsWithTag("Cube(Clone)");

        foreach (GameObject ob in obj) {
            Destroy(ob);
        }

        obj = GameObject.FindGameObjectsWithTag("dethZone");
        foreach (GameObject ob in obj) {
            Destroy(ob);
        }
        while (true) { // This while is for making solvable maze
            for (int i = 0; i < n; i++) { // Creating starting maze 
                for (int j = 0; j < n; j++) {
                    steps[i, j] = 0;
                    path[i, j] = 0;
                    int R = Random.Range(0, 100);
                    float rand = R / 100f;
                    arr[i, j] = 0;
                    if (rand > 0.45f) { // The chanse to create a wall = 55% (1-0.45f) 
                        if (i == 0 && j == 0) continue;
                        if (i == n - 1 && j == n - 1) continue;
                        arr[i, j] = 1;
                    }

                }

            }
            steps[0, 0] = 0;
            bfs(0, 0, 0); // Thying to find the path
            if (steps[n - 1, n - 1] != 0) break; // If finish square is non 0, that means the maze is solvable  
        }
        //print(steps[n - 1, n - 1]);
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                print(steps[i, j]);
                if (arr[i, j] == 1) {
                    GameObject spawn = Instantiate(wall);
                    spawn.transform.position = new Vector3(-9.85f + (i), 1.44f, -7.35f + (j)); // Creating walls in the maze 
                } else {
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
                        if (i == n - 1 && j == n - 1) continue;
                        arr[i, j] = 2;
                        GameObject spawn = Instantiate(dethZone);
                        spawn.transform.position = new Vector3(-9.85f + (i), 0.5f, -7.35f + (j)); // Creating deth zones in the maze
                    }
                }
            }
        }
        arr[0, 0] = 0;
        arr[1, 0] = 0;
        arr[0, 1] = 0;
        remakeSteps(n - 1, n - 1); // Find the shortest path
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
