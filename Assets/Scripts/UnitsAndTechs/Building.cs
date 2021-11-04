using System.Collections.Generic;
using DefaultNamespace;
using UnitsAndTechs;
using UnityEngine;

public abstract class Building : IPlaceable
{
    public abstract int ConstructionMultiplier { get; set; }

    public abstract Vector2 GridSize { get; set; }

    public GridMode GridMode
    {
        get => GridMode.blocking;
    }

    public abstract int GridMultiplier { get; set; }
    public string AssetName { get; set; }
    public Vector2Int LeftTopCellCoord { get; set; }
    public ConstructionCost ConstructionCost { get; set; }

    private Player player;

    public Player Player
    {
        get => player;
        set => SetPlayer(value);
    }

    public Health Health { get; set; }
    public GameObject MapObject { get; set; }
    public abstract void InitValues(Player player, Vector2Int coord);

    public abstract void InitValuesFoundation(Player player, Vector2Int coord);

    public void SetPlayer(Player player)
    {
        if (player == null)
        {
            if (this.player != null)
            {
                Player tmp = this.player;
                this.player = null;
                tmp.RemoveBuilding(this);
            }
        }
        else
        {
            if (this.player != player)
            {
                if (this.player != null)
                {
                    this.player.RemoveBuilding(this);
                }

                player.AddBuilding(this);
                this.player = player;
            }
        }
    }

    public void AddConstructionPoints(int amount)
    {
        ConstructionCost.ConstructionPoints += amount;
        Health.AddHealth(amount);

        if (!ConstructionCost.InConstruction)
        {
            ConstructionFinished();
        }
    }

    public void Destroyed()
    {
        GameMaster.Instance.grid.RemoveElement(this);
        Player.RemoveBuilding(this);
        GameMaster.Instance.DestroyMapObject(MapObject);
    }

    protected abstract void ConstructionFinished();
}