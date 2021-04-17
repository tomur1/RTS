using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public List<IPlaceable> Elements { get; set; }
    public Vector2 GridPosition { get; set; }
    public Cell(Vector2 gridPosition, List<IPlaceable> elements)
    {
        GridPosition = gridPosition;
        Elements = elements;
    }
    
    public Cell(Vector2 gridPosition, IPlaceable element)
    {
        GridPosition = gridPosition;
        Elements = new List<IPlaceable>();
        Elements.Add(element);
    }
    
    public Cell(Vector2 gridPosition)
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