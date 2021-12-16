using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject tile;
    public static int WIDTH = 6, HEIGHT = 12;
    public static float STARTX = 0, STARTY = 0;

    void Start()
    {
        Transform _parent = gameObject.GetComponent<Transform>();

        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                var _tile = Instantiate(tile, new Vector3(STARTX + j, 0, STARTY + i), Quaternion.identity);
                _tile.transform.parent = _parent;
            }
        }
    }
}
