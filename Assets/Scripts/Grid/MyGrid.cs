using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid
{
    private int width;

    public int Width => width;

    public int Height => height;

    public float CellSize => cellSize;

    private int height;
    private float cellSize;
    private Vector3 originPos;
    
    private Cell[,] gridData;
    public HashSet<IPlaceable> ElementsOnMap { get; }

    public MyGrid(int width, int height, float cellSize, Vector3 originPos)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPos = originPos;

        ElementsOnMap = new HashSet<IPlaceable>();
        gridData = new Cell[width, height];
        InitCells();
    }

    private void InitCells()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                gridData[i, j] = new Cell(new Vector2(i, j));
            }
        }
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

    public Cell GetCellWithCoord(Vector2Int coord)
    {
        return gridData[coord.x, coord.y];
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

    public void AddElement(IPlaceable elementToAdd)
    {
        if (!CanPlace(elementToAdd))
        {
            throw new IndexOutOfRangeException(elementToAdd + " Cannot be placed here");
        }
        
        var takenCoords = CoordsTakenByElement(elementToAdd);
        ElementsOnMap.Add(elementToAdd);
        foreach (var takenCoord in takenCoords)
        {
            var cell = GetCellWithCoord(takenCoord);
            cell.Elements.Add(elementToAdd);
        }
    }

    public bool CanPlace(IPlaceable element)
    {
        var takenCoords = CoordsTakenByElement(element);
        foreach (var coord in takenCoords)
        {
            //Check if corner is on grid
            if (coord.x >= Width || coord.x < 0 ||
                coord.y >= Height || coord.y < 0)
            {
                return false;
            }

            var cell = GetCellWithCoord(coord);
            if (!cell.canPass())
            {
                return false;
            }
        }
        

        return true;
    }

    public List<Vector2Int> CoordsTakenByElement(IPlaceable element)
    {
        List<Vector2Int> coordsTakenByElement = new List<Vector2Int>();
        var cornerCoord = element.LeftTopCellCoord;
        
        for (int i = 0; i < element.GridMultiplier * element.GridSize.x; i++)
        {
            for (int j = 0; j < element.GridMultiplier * element.GridSize.y; j++)
            {
                coordsTakenByElement.Add(new Vector2Int(i + cornerCoord.x,j + cornerCoord.y));
            }
        }

        return coordsTakenByElement;
    }
}
