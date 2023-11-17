using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject pole;
    public Material shieldMat, defMat;
    public ParticleSystem dethPartical, winPartical;
    public Animator anim;
    public bool pause;

    private CreateWalls script;
    private int[,] steps = new int[30, 30], arr = new int[30,30];

    private int i = 0, j = 0, n;
    private float time = 0, shieldTime = 0, dethTime = 0, winTime=0;

    private bool shieldB = false, deth = false, win = false;
    // Start is called before the first frame update
    void Start()
    {
        script = pole.GetComponent<CreateWalls>();
        script.createMaze();
        steps = script.path;
        n = script.n;
        arr = script.arr;
    }

    bool valid(int i, int j) { // Check if coords are valid
        if (i >= n || j >= n || i < 0 || j < 0) return false;
        return true;
    }

    void nextStep() { // Calculate next coords of player
        int curStep = steps[i, j];
        if (valid(i + 1, j) && steps[i + 1, j] == curStep + 1) {
            i++;
            return;
        }

        if (valid(i - 1, j) && steps[i - 1, j] == curStep + 1) {
            i--;
            return;
        }

        if (valid(i, j + 1) && steps[i, j + 1] == curStep + 1) {
            j++;
            return;
        }

        if (valid(i, j - 1) && steps[i, j - 1] == curStep + 1) {
            j--;
            return;
        }

    }

    public void shield() {
        shieldB = true;
    }
    // Update is called once per frame
    void Update() {
        if (!pause) {
            if (i == n - 1 && j == n - 1) {
                if (!win) {
                    winPartical.Play();
                    win = true;
                    anim.SetBool("Anim", true);
                }
                winTime += Time.deltaTime;
                if (winTime > 2f) {
                    anim.SetBool("Anim", false);
                    win = false;
                    script.createMaze();
                    i = 0; j = 0;
                    winTime = 0;
                }
            }

            if (!win) {
                time += Time.deltaTime;
                if (time >= 0.5f && deth == false) { // Each player`s step per 0.5 second 
                    nextStep();
                    time = 0;
                    this.transform.position = new Vector3(-9.85f + (i), 1.44f, -7.35f + (j));
                }
                if (deth == false) {
                    if (shieldB == true) {
                        this.GetComponent<MeshRenderer>().material = shieldMat;
                        shieldTime += Time.deltaTime;
                    } else this.GetComponent<MeshRenderer>().material = defMat;
                }
                if ((arr[i, j] == 2 && shieldB == false) || deth == true) {
                    if (deth == false) {
                        dethPartical.Play();
                        anim.SetBool("Anim", true);
                    }
                    deth = true;
                    dethTime += Time.deltaTime;
                    this.transform.position = new Vector3(-9.85f + (i), 1.44f, -7.35f + (j));
                    if (dethTime > 2f) {
                        i = 0;
                        j = 0;
                        this.transform.position = new Vector3(-9.85f + (i), 1.44f, -7.35f + (j));
                        dethTime = 0f;
                        deth = false;
                        anim.SetBool("Anim", false);
                    }
                }

                if (shieldTime > 2f) {
                    shieldTime = 0;
                    shieldB = false;
                }
            }
        }
    }
}
