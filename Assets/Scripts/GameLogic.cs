using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Color[] colors;

    public GameObject block;

    private List<GameObject> tableObjects = new List<GameObject>();
    private int[] tableColors = new int [Board.WIDTH * Board.HEIGHT];

    public Transform grid;

    private int score = 0;
    private int scoreAccum = 0;

    [HideInInspector]
    public GameObject block1, block2;
    [HideInInspector]
    public int color1, color2;

    private int rotationAngle = 0;

    void Start()
    {
        block1 = Instantiate(block, new Vector3(Board.WIDTH / 2 - 1, 0, Board.HEIGHT - 1), Quaternion.identity);
        block2 = Instantiate(block, new Vector3(Board.WIDTH / 2, 0, Board.HEIGHT - 1), Quaternion.identity);

        color1 = Random.Range(0, colors.Length);
        color2 = Random.Range(0, colors.Length);

        block1.GetComponent<Renderer>().material.color = colors[color1];
        block2.GetComponent<Renderer>().material.color = colors[color2];

        for (int i = 0; i < tableColors.Length; i ++)
        {
            tableColors[i] = -1;
        }
        
        Transform _parent = gameObject.GetComponent<Transform>();
        for (int i = 0; i < Board.HEIGHT; i++)
        {
            for (int j = 0; j < Board.WIDTH; j++)
            {
                tableObjects.Add(Instantiate(block, new Vector3(Board.STARTX + j, 0, Board.STARTY + i), Quaternion.identity));
                tableObjects[i * Board.WIDTH + j].transform.parent = _parent;
                tableObjects[i * Board.WIDTH + j].GetComponent<Renderer>().enabled = false;
            }
        }
    }

    public void setScore(int score) {
        this.score=score;
    }
    public int getScore() {
        return score;
    }
    public int getScoreAccum() {
        return scoreAccum;
    }


    public void fall()
    {        
        block1.transform.position -= new Vector3(0, 0, 1);
        block2.transform.position -= new Vector3(0, 0, 1);
    }
    public void moveLeft()
    {
        var block1Pos = block1.transform.position;
        var block2Pos = block2.transform.position;
        if ((getPairLeft()>0) && (getTableElement((int)block1Pos.x-1, (int)block1Pos.z) == -1) && (getTableElement((int)block2Pos.x-1, (int)block2Pos.z) == -1))
        {
            block1.transform.position -= new Vector3(1, 0, 0);
            block2.transform.position -= new Vector3(1, 0, 0);
        }
    }
    public void moveRight()
    {
        var block1Pos = block1.transform.position;
        var block2Pos = block2.transform.position;
        if (getPairRight()<Board.WIDTH-1 && getTableElement((int)block1Pos.x+1, (int)block1Pos.z) == -1 && getTableElement((int)block2Pos.x+1, (int)block2Pos.z) == -1)
        {
            block1.transform.position += new Vector3(1, 0, 0);
            block2.transform.position += new Vector3(1, 0, 0);
        }
    }
    public void rotate()
    {
        rotationAngle=(rotationAngle + 90)%360;

        var block2Pos = getRotationPosition();
        
        if(block2Pos.x < 0 || block2Pos.x >= Board.WIDTH || block2Pos.z < 0 || getTableElement((int)block2Pos.x, (int)block2Pos.z)!=-1)
        {
            rotationAngle=(rotationAngle - 90)%360;
        }
        else
        {
            calcRotation();
        }
    }
    public void nextPair()
    {
        block1 = Instantiate(block, new Vector3(Board.WIDTH / 2 - 1, 0, Board.HEIGHT - 1), Quaternion.identity);
        block2 = Instantiate(block, new Vector3(Board.WIDTH / 2, 0, Board.HEIGHT - 1), Quaternion.identity);

        color1 = Random.Range(0, colors.Length);
        color2 = Random.Range(0, colors.Length);

        block1.GetComponent<Renderer>().material.color = colors[color1];
        block2.GetComponent<Renderer>().material.color = colors[color2];

        rotationAngle = 0;
    }
    int getPairTop()
    {
        if(block1.transform.position.z<block2.transform.position.z) {
            return (int)block1.transform.position.z;
        } else {
            return (int)block2.transform.position.z;
        }
    }
    int getPairBottom()
    {
        if(block1.transform.position.z>block2.transform.position.z) {
            return (int)block1.transform.position.z;
        } else {
            return (int)block2.transform.position.z;
        }
    }
    int getPairLeft()
    {
        if(block1.transform.position.x<block2.transform.position.x) {
            return (int)block1.transform.position.x;
        } else {
            return (int)block2.transform.position.x;
        }
    }
    int getPairRight()
    {
        if(block1.transform.position.x>block2.transform.position.x) {
            return (int)block1.transform.position.x;
        } else {
            return (int)block2.transform.position.x;
        }
    }
    void calcRotation()
    {
        switch(rotationAngle)
        {
            case 0:
                block2.transform.position = block1.transform.position + new Vector3(1, 0, 0);
                break;
            case 90:
                block2.transform.position = block1.transform.position + new Vector3(0, 0, -1);
                break;
            case 180:
                block2.transform.position = block1.transform.position + new Vector3(-1, 0, 0);
                break;
            case 270:
                block2.transform.position = block1.transform.position + new Vector3(0, 0, 1);
                break;
        }
    }

    Vector3 getRotationPosition()
    {
        switch(rotationAngle)
        {
            case 0:
                return block1.transform.position + new Vector3(1, 0, 0);
            case 90:
                return block1.transform.position + new Vector3(0, 0, -1);
            case 180:
                return block1.transform.position + new Vector3(-1, 0, 0);
            case 270:
                return block1.transform.position + new Vector3(0, 0, 1);
        }
        return block1.transform.position + new Vector3(1, 0, 0);
    }

    public int getTableElement(int x,int y) {
        if(y<0 || y>=Board.HEIGHT || x<0 || x>=Board.WIDTH) {
            return -1;
        }
        return tableColors[y*Board.WIDTH+x];
    }
    public void setTableElement(int col, int x, int y)
    {
        if (y >= 0 || y < Board.HEIGHT-1 || x >= 0 || x < Board.WIDTH-1)
        {
            tableColors[y * Board.WIDTH + x] = col;
            if (col >= 0)
            {
                tableObjects[y * Board.WIDTH + x].GetComponent<Renderer>().enabled = true;
                tableObjects[y * Board.WIDTH + x].GetComponent<Renderer>().material.color = colors[col];
            }
            else
            {
                tableObjects[y * Board.WIDTH + x].GetComponent<Renderer>().enabled = false;
            }
        }
    }

    public bool collides() {
        var block1Pos = block1.transform.position;
        var block2Pos = block2.transform.position;
        if (block1Pos.z == 0 || getTableElement((int)block1Pos.x, (int)block1Pos.z - 1)>=0 ||
            block2Pos.z == 0 || getTableElement((int)block2Pos.x, (int)block2Pos.z - 1)>=0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool adjustTable()
    {
        scoreAccum = 0;
        bool changes = false;
        for (int y = Board.HEIGHT - 2; y>=0; y--)
        {
            for (int x = 0; x<Board.WIDTH; x++)
            {
                if (getTableElement(x, y+1)!=-1 && getTableElement(x, y)==-1)
                {
                    setTableElement(getTableElement(x, y+1), x, y);
                    setTableElement(-1, x, y+1);
                    changes = true;
                }
            }
        }
        if(!changes) {
            for(int x=0; x<Board.WIDTH; x++) {
                for(int y=0; y<Board.HEIGHT; y++) {
                    int element=getTableElement(x, y);
                    if(element!=-1) {
                        initMarkTable();
                        int piezas=markGroups(element,x,y);
                        if(piezas>3) {
                            scoreAccum+=piezas;
                            for(int xx=0; xx<Board.WIDTH; xx++) {
                                for(int yy=0; yy<Board.HEIGHT; yy++) {
                                    if(mark[xx, yy]) {
                                        mark[xx, yy]=false;
                                        setTableElement(-1, xx, yy);
                                        changes = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return changes;
    }

    //auxiliary table used in the piece agrouppations test
    private bool[,] mark = new bool[Board.WIDTH, Board.HEIGHT];
    private void initMarkTable() {
        for(int x=0;x<Board.WIDTH;x++) {
            for(int y=0;y<Board.HEIGHT;y++) {
                mark[x, y]=false;
            }
        }
    }

    //Recursively, search for piece groups of the same color of element
    //returns the number of pieces in the groups
    private int markGroups(int element,int x, int y) {
        int ret=0;
        if(getTableElement(x,y)==element && !mark[x, y]) {
            mark[x, y]=true;
            ret=1+markGroups(element,x-1,y)+markGroups(element,x+1,y)+markGroups(element,x,y-1)+markGroups(element,x,y+1);
        }
        return ret;
    }
}
