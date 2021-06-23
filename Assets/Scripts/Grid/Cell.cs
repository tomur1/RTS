using System.Collections.Generic;
using UnitsAndTechs;
using UnityEngine;

public class Cell
{
    public List<IPlaceable> Elements { get; set; }
    public Vector2Int GridPosition { get; set; }
    
    public Building Building { get; set; }

    public Cell(Vector2Int gridPosition, List<IPlaceable> elements)
    {
        GridPosition = gridPosition;
        Elements = elements;
    }
    
    public Cell(Vector2Int gridPosition, IPlaceable element)
    {
        GridPosition = gridPosition;
        Elements = new List<IPlaceable>();
        Elements.Add(element);
    }
    
    public Cell(Vector2Int gridPosition)
    {
        GridPosition = gridPosition;
        Elements = new List<IPlaceable>();
    }

    // TODO If the performance is low then change this logic
    public bool canPass()
    {
        foreach (var element in Elements)
        {
            if (element.GridMode == GridMode.blocking)
            {
                return false;
            }    
        }
        return true;
    }
    
}