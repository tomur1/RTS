using System.Collections;
using UnityEngine;

namespace UnitsAndTechs
{
    public interface IUnitSpawner
    {
        public Vector2Int SpawnPoint { get; set; }
    }
}