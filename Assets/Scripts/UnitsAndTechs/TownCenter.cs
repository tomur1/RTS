using UnityEngine;

namespace UnitsAndTechs
{
    public class TownCenter : Building
    {
        public TownCenter()
        {
            GridSize = new Vector2(3, 3) * GRIDMULTIPLIER;
            constructionCost = new ConstructionCost(500, 200,
                0, 0, 0, 500);
            
        }

        public override Vector2 GridSize { get; set; }
        public override ConstructionCost constructionCost { get; set; }
    }
}