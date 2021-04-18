using DefaultNamespace;
using UnitsAndTechs.Units;

namespace UnitsAndTechs
{
    public interface ISpawner
    {
        public void SpawnUnit();
        public Unit SelectedUnit { get; set; }
    }
}
