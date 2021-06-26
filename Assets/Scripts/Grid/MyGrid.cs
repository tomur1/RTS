using System;
using System.Collections;
using System.Collections.Generic;
using UnitsAndTechs;
using UnitsAndTechs.Units;
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
                gridData[i, j] = new Cell(new Vector2Int(i, j));
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
        var y = cellPos.z;
        // Don't go over
        if (x >= width || x < 0 || y >= height || y < 0)
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

    public void UpdateElementPosition(IPlaceable element, Vector2Int newPos)
    {
        var takenCoords = CoordsTakenByElement(element);
        foreach (var coord in takenCoords)
        {
            GetCellWithCoord(coord).Elements.Remove(element);
        }
        
        var newTakenCoords = CoordsTakenByElement(element, newPos);
        foreach (var coord in newTakenCoords)
        {
            GetCellWithCoord(coord).Elements.Add(element);
        }

        element.LeftTopCellCoord = newPos;
    }

    public void ShowLines()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Debug.DrawLine(GetWorldPos(i,j), GetWorldPos(i,j+1), Color.blue, Time.deltaTime);
                Debug.DrawLine(GetWorldPos(i,j), GetWorldPos(i+1,j), Color.blue, Time.deltaTime);
                var cell = GetCellWithCoord( new Vector2Int(i,j));
                if (!cell.canPass())
                {
                    Debug.DrawLine(GetWorldPos(i,j), GetWorldPos(i+1,j+1), Color.red, Time.deltaTime);
                    Debug.DrawLine(GetWorldPos(i,j+1), GetWorldPos(i+1,j), Color.red, Time.deltaTime);
                }else if (cell.Elements.Count != 0)
                {
                    Debug.DrawLine(GetWorldPos(i,j), GetWorldPos(i+1,j+1), Color.green, Time.deltaTime);
                    Debug.DrawLine(GetWorldPos(i,j+1), GetWorldPos(i+1,j), Color.green, Time.deltaTime);
                }
            }
        }
        Debug.DrawLine(GetWorldPos(0,height), GetWorldPos(width,height), Color.blue, Time.deltaTime);
        Debug.DrawLine(GetWorldPos(width,0), GetWorldPos(width,height), Color.blue, Time.deltaTime);
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

    public void RemoveElement(IPlaceable elementToRemove)
    {
        var takenCoords = CoordsTakenByElement(elementToRemove);
        
        foreach (var takenCoord in takenCoords)
        {
            var cell = GetCellWithCoord(takenCoord);
            cell.Elements.Remove(elementToRemove);
        }
    }

    public Cell FindClosestEmptyPos(IPlaceable element, Vector2Int targetCoord)
    {
        bool found = false;
        Vector2Int currentCoord = targetCoord;
        var steps = 1;
        int direction = 1;
        var addSteps = 0;
        while (!found)
        {
            if (CanPlace(element, currentCoord))
            {
                break;
            }

            for (int i = 0; i < steps; i++)
            {
                //If we are going up
                if (direction == 1)
                {
                    currentCoord += new Vector2Int(0, 1);
                }else if (direction == 2)
                {
                    currentCoord += new Vector2Int(1, 0);
                }else if (direction == 3)
                {
                    currentCoord += new Vector2Int(0, -1);
                }else if (direction == 4)
                {
                    currentCoord += new Vector2Int(-1, 0);
                }

                if (CanPlace(element, currentCoord))
                {
                    return GetCellWithCoord(currentCoord);
                }
            }

            direction++;
            if (addSteps == 2)
            {
                addSteps = 0;
                steps++;
            }
            
            addSteps++;
            if (direction > 4)
            {
                direction = 1;
            }

        }

        return GetCellWithCoord(currentCoord);
    }

    public bool CanPlace(IPlaceable element)
    {
        var takenCoords = CoordsTakenByElement(element);
        foreach (var coord in takenCoords)
        {
            //Check if corner is on grid
            if (!InBounds(coord))
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
    
    public bool CanPlace(IPlaceable element, Vector2Int wantedCoord)
    {
        var takenCoords = CoordsTakenByElement(element, wantedCoord);
        foreach (var coord in takenCoords)
        {
            //Check if corner is on grid
            if (!InBounds(coord))
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
    
    public List<Vector2Int> CoordsTakenByElement(IPlaceable element, Vector2Int wantedCoord)
    {
        List<Vector2Int> coordsTakenByElement = new List<Vector2Int>();
        var cornerCoord = wantedCoord;
        
        for (int i = 0; i < element.GridMultiplier * element.GridSize.x; i++)
        {
            for (int j = 0; j < element.GridMultiplier * element.GridSize.y; j++)
            {
                coordsTakenByElement.Add(new Vector2Int(i + cornerCoord.x,j + cornerCoord.y));
            }
        }

        return coordsTakenByElement;
    }

    public bool InBounds(Vector2Int coord)
    {
        if (coord.x >= Width || coord.x < 0 ||
            coord.y >= Height || coord.y < 0)
        {
            return false;
        }

        return true;
    }

    // Return closest cell to unit from which unit can do action. IE. Attack or build
    public Cell GetClosestCellInRangeTo(Unit unit, IPlaceable placeable)
    {
        var range = unit.getRange();

        if (range == 0)
        {
            Debug.LogWarning("Range should not be 0");
        }

        if (InRange(unit.LeftTopCellCoord, range, placeable))
        {
            return GetCellWithCoord(unit.LeftTopCellCoord);
        }

        var closestCell = GetClosestCellTo(unit.LeftTopCellCoord, placeable);

        Vector2Int lastCoord = new Vector2Int();
        bool? isFirstInRange = null;
        foreach (var coordOnLine in GetPointsOnLine(unit.LeftTopCellCoord.x, unit.LeftTopCellCoord.y, closestCell.GridPosition.x, closestCell.GridPosition.y))
        {
            if (isFirstInRange == null)
            {
                if (InRange(coordOnLine, range, placeable))
                {
                    isFirstInRange = true;
                }
                else
                {
                    isFirstInRange = false;
                }
            }

            if (isFirstInRange == true)
            {
                if (!InRange(coordOnLine, range, placeable))
                {
                    return GetCellWithCoord(lastCoord);
                }
            }
            else
            {
                if (InRange(coordOnLine, range, placeable))
                {
                    return GetCellWithCoord(coordOnLine);
                }
            }
            lastCoord = coordOnLine;
        }

        return null;
    }
    
    public Cell GetClosestCellTo(Vector2 unitPos, IPlaceable placeable){
        Vector2Int closestCoord = placeable.LeftTopCellCoord;
        float minDistance = Single.MaxValue;
        foreach (var coord in CoordsTakenByElement(placeable))
        {
            var distance = Vector2.Distance(coord, unitPos);
            if (minDistance > distance )
            {
                minDistance = distance;
                closestCoord = coord;
            }
        }

        return GetCellWithCoord(closestCoord);
    }

    public bool InRange(Vector2Int unitPos, int range, IPlaceable placeable)
    {
        var distance = unitPos - GetClosestCellTo(unitPos, placeable).GridPosition;
        if (Mathf.Abs(distance.x) > range || Mathf.Abs(distance.y) > range)
        {
            return false;
        }

        return true;
    }

    public Vector3 UnityPlacePosition(IPlaceable elementToAdd, Vector2Int wantedCoord)
    {
        return GetWorldPos(wantedCoord.x, wantedCoord.y) +
               new Vector3(elementToAdd.GridSize.x * elementToAdd.GridMultiplier, 0,
                   elementToAdd.GridSize.y * elementToAdd.GridMultiplier) / 2;
    }

    public static IEnumerable<Vector2Int> GetPointsOnLine(int x0, int y0, int x1, int y1)
    {
        bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        if (steep)
        {
            int t;
            t = x0; // swap x0 and y0
            x0 = y0;
            y0 = t;
            t = x1; // swap x1 and y1
            x1 = y1;
            y1 = t;
        }
        if (x0 > x1)
        {
            int t;
            t = x0; // swap x0 and x1
            x0 = x1;
            x1 = t;
            t = y0; // swap y0 and y1
            y0 = y1;
            y1 = t;
        }
        int dx = x1 - x0;
        int dy = Math.Abs(y1 - y0);
        int error = dx / 2;
        int ystep = (y0 < y1) ? 1 : -1;
        int y = y0;
        for (int x = x0; x <= x1; x++)
        {
            yield return new Vector2Int((steep ? y : x), (steep ? x : y));
            error = error - dy;
            if (error < 0)
            {
                y += ystep;
                error += dx;
            }
        }
    }
}
