using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    private int STATE_DRIVING = 0;
    private int STATE_GAMEOVER = 1;
    private int STATE_ADJUSTTABLE = 2;
    private int GameState = 0;

    public float FALL_TIME = 0.9f;
    public float QUICK_FALL_TIME = 0.05f;
    
    private float fallTime = 0;

    public Text scoreText;
    public GameObject restartButton;
    public PressDown downButton;

    private bool isQuickFall = false;
    private GameLogic game;

    void Start()
    {
        game = GetComponent<GameLogic>();
    }
    void Update()
    {
        fallTime += Time.deltaTime;

        if (GameState == STATE_GAMEOVER)
            return;

        Movement();
        if (fallTime > (isQuickFall ? QUICK_FALL_TIME : FALL_TIME))
        {
            fallTime = 0;
            if (GameState == STATE_DRIVING)
            {
                if (!game.collides())
                {
                    game.fall();
                }
                else
                {
                    game.setTableElement(game.color1, (int)game.block1.transform.position.x, (int)game.block1.transform.position.z);
                    game.setTableElement(game.color2, (int)game.block2.transform.position.x, (int)game.block2.transform.position.z);
                    Destroy(game.block1);
                    Destroy(game.block2);
                    game.nextPair();

                    GameState = STATE_ADJUSTTABLE;
                }
                // Если не фигура не сталкивается, пускай падает, иначе фигуру в массив фигур, вызываем новую фигуру, меняем стейт на аджуст тейбл
            }
            else
            {
                if (game.adjustTable())
                {
                    if (game.getScoreAccum()>0)
                    {
                        game.setScore(game.getScore() + game.getScoreAccum());
                        scoreText.text = "Score: " + game.getScore();
                    }
                }
                else
                {
                    if(game.getTableElement((int)game.block1.transform.position.x, (int)game.block1.transform.position.z) != -1 ||
                            game.getTableElement((int)game.block2.transform.position.x, (int)game.block2.transform.position.z) != -1 )
                    {
                        GameState=STATE_GAMEOVER;
                        Debug.Log("Gameover");
                        restartButton.SetActive(true);
                    }
                    else
                    {
                        GameState=STATE_DRIVING;
                    }
                }
            }
        }

    }

    void Movement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            game.moveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            game.moveRight();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            game.rotate();
        }

        isQuickFall = Input.GetKey(KeyCode.DownArrow) || downButton.buttonPressed;
    }
    public void Restart()
    {
        SceneManager.LoadScene("Tetris");
    }
}
