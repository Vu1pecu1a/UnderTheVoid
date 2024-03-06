using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGrid : MonoBehaviour
{

    MapGrid[,] _grids= null;
    [SerializeField]
    int gridx, girdy,Scale = 1;
    [SerializeField]
    GameObject MapTile;
    private void Awake()
    {
        _grids = new MapGrid[gridx,girdy];//그리드 생성

        for(int i =0;i<gridx;i++)
        {
            for(int j=0;j<girdy;j++)
            {
                MapAwake(i,j);
            }
        }
    }

    void MapAwake(int i, int j)
    {
        GameObject grid = Instantiate(MapTile, gameObject.transform);
        _grids[i, j] = grid.GetComponent<MapGrid>();
        grid.transform.localPosition = new Vector3(i*Scale, 0.02f, j*Scale);
        grid.name = "[" + i + "," + j + "]";
        _grids[i, j].coordinateReturn(i, j);
    }
}
