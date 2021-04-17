using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    private static Dictionary<string , int> resourceRarity = new Dictionary<string, int>()
    {
        {"empty", 1000},
        {"oil", 30},
        {"uranium", 5},
    };

    public static void FillGrid(MyGrid grid)
    {
        System.Random rng = new System.Random(); 
        for (int i = 0; i < grid.Width; i++)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                var number = rng.Next(1, 1000);
                var cellCoord = new Vector2Int(i, j);
                if (number <= resourceRarity["uranium"])
                {
                    var uranium = new UraniumSource(cellCoord);
                    if (grid.CanPlace(uranium))
                    {
                        grid.AddElement(uranium);                        
                    }
                    
                }else if (number <= resourceRarity["oil"])
                {
                    var oil = new OilSource(cellCoord);
                    if (grid.CanPlace(oil))
                    {
                        grid.AddElement(oil);                        
                    }
                }
                else
                {
                    //Nothing
                }
                
            }
        }
    }

    public static void InitMapObjects(MyGrid grid)
    {
        foreach (var element in grid.ElementsOnMap)
        {
            var elemetUnityObject = Resources.Load<GameObject>(element.AssetName);
            Instantiate(elemetUnityObject, grid.GetWorldPos(element.LeftTopCellCoord.x, element.LeftTopCellCoord.y), Quaternion.identity);
        }
    }
}
