using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPos;
    
    private Cell[,] gridData;

    public MyGrid(int width, int height, float cellSize, Vector3 originPos)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPos = originPos;
        
        gridData = new Cell[width, height];
    }

    public Vector3Int GetCellPos(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x + originPos.x / cellSize);
        int z = Mathf.FloorToInt(worldPos.z + originPos.z / cellSize);
        return new Vector3Int(x,0, z);
    }

    public Cell GetCell(Vector3 worldPos)
    {
        Vector3Int cellPos = GetCellPos(worldPos);
        var x = cellPos.x;
        var y = cellPos.y;
        // Don't go over
        if (x > width || x < 0 || y > height || y < 0)
        {
            throw new IndexOutOfRangeException("Wanted Cell out of bounds");
        }
        return gridData[x, y];
    }

    public Vector3 GetWorldPos(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + originPos;
    }

    public void ShowLines()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Debug.DrawLine(GetWorldPos(i,j), GetWorldPos(i,j+1), Color.blue, 100f);
                Debug.DrawLine(GetWorldPos(i,j), GetWorldPos(i+1,j), Color.blue, 100f);
            }
        }
        Debug.DrawLine(GetWorldPos(0,height), GetWorldPos(width,height), Color.blue, 100f);
        Debug.DrawLine(GetWorldPos(width,0), GetWorldPos(width,height), Color.blue, 100f);
    }
}
