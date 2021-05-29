using System;
using System.Collections;
using System.Collections.Generic;
using UnitsAndTechs;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    private static Dictionary<string , int> resourceRarity = new Dictionary<string, int>()
    {
        {"empty", 10000},
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
                var number = rng.Next(1, 100000);
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

    //For each player spawn TC and a worker
    public static void SpawnPlayers(MyGrid grid, List<Player> players)
    {
        var points = CirclePoints(players.Count, grid);

        for (int i = 0; i < players.Count; i++)
        {
            players[i].Buildings.Add(new TownCenter(points[i], players[i], true));
        }
    }

    private static List<Vector2Int> CirclePoints(int numberOfPoints, MyGrid grid)
    {
        var points = new List<Vector2Int>();
        var radius = Mathf.Floor((float) (Math.Min(grid.Width, grid.Height) / 2.0));
        var step = 360 / numberOfPoints;
        var angle = 45;
        for (int i = 0; i < numberOfPoints; i++)
        {
            var x = grid.Width / 2 + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            var y = grid.Height / 2  + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
            points.Add(new Vector2Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y)));
            angle += step;
        }

        return points;
    }
}
