using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour // CUBE OPERARION 
{
    public GameObject poleGameOBJ; // pole game object to have access to CreateWalls script
    public Material shieldMat, defMat; // cube`s matertials 
    public ParticleSystem dethPartical, winPartical; // particals for win and death
    public Animator cameraAnimation; // camera fading animation
    public bool isPause; // is game on paues ot not
    public float cubeSpeed; 

    private CreateWalls createWallScript;
    private int[,] path = new int[30, 30], maze = new int[30,30]; // path - cube`s path to win

    private int i = 0, j = 0, mazeSize; // (i,j) - cube`s position at the maze
    // shieldTime - how long shield is on 
    private float shieldTime = 0, dethTimeAnimation = 0, winTime=0;

    private bool isShieldPressed = false, isDead = false, isWin = false;
    // Start is called before the first frame update
    void Start()
    {
        createWallScript = poleGameOBJ.GetComponent<CreateWalls>(); // get CreateWalls script
        createWallScript.createMaze(); // create maze
        path = createWallScript.path; // get the path
        mazeSize = createWallScript.mazeSize; // get maze size
        maze = createWallScript.maze; // get maze
    }

    bool valid(int i, int j) { // Check if coords are valid
        if (i >= mazeSize || j >= mazeSize || i < 0 || j < 0) return false;
        return true;
    }

    void nextStep() { // Calculate next coords of player

        // if neighbour cell is valid and number of steps from start = current cell number to start + 1, so we can move there 

        int curStep = path[i, j]; // get current cell number to start

        /// MOVE CUBE

        if (valid(i + 1, j) && path[i + 1, j] == curStep + 1) {
            i++;
            return;
        }

        if (valid(i - 1, j) && path[i - 1, j] == curStep + 1) {
            i--;
            return;
        }

        if (valid(i, j + 1) && path[i, j + 1] == curStep + 1) {
            j++;
            return;
        }

        if (valid(i, j - 1) && path[i, j - 1] == curStep + 1) {
            j--;
            return;
        }

    }

    public void shield() {
        isShieldPressed = true;
    }
    // Update is called once per frame
    void Update() {
        if (!isPause) { // if pause is not clicked
            if (i == mazeSize - 1 && j == mazeSize - 1) { // if player at top right corner (win corner) 
                Vector3 winPosition = new Vector3(-9.85f + (mazeSize-1), 1.44f, -7.35f + (mazeSize-1));
                if (Vector3.Distance(winPosition, this.transform.position) <= 0.1f) {// check if cube is not near the win postion 
                    if (!isWin) { // if win animations is not playing yet
                        winPartical.Play(); // start win partical
                        isWin = true;
                        cameraAnimation.SetBool("Anim", true); // start fading 
                    }
                    winTime += Time.deltaTime;
                    if (winTime > 2f) { // if win partical is over so create next maze 
                        cameraAnimation.SetBool("Anim", false); // end camera fading
                        isWin = false;
                        createWallScript.createMaze(); // create new maze 
                        i = 0; j = 0; // set cube`s position at start 
                        winTime = 0;
                    }
                }
            }

            if (!isWin) { // if player isn`t at win position 
                if (isDead == false) { // if cube is not dead
                    Vector3 newPosition = new Vector3(-9.85f + (i), 1.44f, -7.35f + (j)); // cube`s position at the next step
                    if (Vector3.Distance(this.transform.position, newPosition) >= 0.1f) // if cube is not enough close to next position, keep moving it 
                        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(-9.85f + (i), 1.44f, -7.35f + (j)), Time.deltaTime*cubeSpeed); // move it
                    else { // if enough close so calculate next step
                        nextStep();
                    }
                }

                if (isDead == false) { // if cube isn`t dead 
                    if (isShieldPressed == true) { // if shield button was clicked to we need to set new material
                        this.GetComponent<MeshRenderer>().material = shieldMat; // set new material
                        shieldTime += Time.deltaTime; 
                    } else this.GetComponent<MeshRenderer>().material = defMat; // if sheild button wasn`t clicked so set material to default 
                }

                if ((maze[i, j] == 2 && isShieldPressed == false) || isDead == true) { // if cube at deth zone and it has not shield or dead
                    Vector3 newPosition = new Vector3(-9.85f + (i), 1.44f, -7.35f + (j));
                    if (Vector3.Distance(this.transform.position, newPosition) <= 0.1f) { // if cube enough close to deth zone so we need to start deth animations 

                        if (isDead == false) { // if i enter this at first time 
                            dethPartical.Play(); // start deht particals 
                            cameraAnimation.SetBool("Anim", true); // start camera fading 
                        }
                        isDead = true;
                        dethTimeAnimation += Time.deltaTime;
                        if (dethTimeAnimation > 2f) {
                            // set cube`s coord to start
                            i = 0;
                            j = 0;
                            this.transform.position = new Vector3(-9.85f + (i), 1.44f, -7.35f + (j)); // move cube to start
                            dethTimeAnimation = 0f;
                            isDead = false;
                            cameraAnimation.SetBool("Anim", false); // end camera fading 
                        }
                    }
                }

                if (shieldTime > 2f) { // if shield is on more than 2 seconds, i need to cancel it 
                    shieldTime = 0; 
                    isShieldPressed = false; // cancel the shield 
                }
            }
        }
    }
}
