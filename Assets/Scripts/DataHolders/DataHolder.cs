using System.Collections.Generic;
using UnityEngine;

namespace DataHolders
{
    public class DataHolder
    {
        public List<PlayerInfoHolder> PlayersData { get; set; }

        public List<MapElementInfoHolder> MapElements { get; set; }
        public Vector3 CameraPosition { get; set; }
    
        public Color MainPlayerColor { get; set; }
    
        public DataHolder()
        {
            PlayersData = new List<PlayerInfoHolder>();
            MapElements = new List<MapElementInfoHolder>();
        }
    
        public void GetData()
        {
            var gameMaster = GameMaster.Instance;
            foreach (var player in gameMaster.playersInGame)
            {
                PlayersData.Add(new PlayerInfoHolder(player));
            }

            foreach (var placeable in gameMaster.grid.GetElementsOnMap())
            {
                if (placeable.Player == null)
                {
                    MapElements.Add(new MapElementInfoHolder(placeable));
                }
            }
            CameraPosition = gameMaster.mainCamera.transform.position;
            MainPlayerColor = gameMaster.player.Color;
        }
    }
}
