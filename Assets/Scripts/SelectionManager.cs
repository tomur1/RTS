using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager
{
    public Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();

    public SelectionManager()
    {
        selectedTable = new Dictionary<int, GameObject>();
    }

    public void addSelected(GameObject go)
    {
        int id = go.GetInstanceID();

        if (!(selectedTable.ContainsKey(id)))
        {
            selectedTable.Add(id, go);
            Debug.Log("Added " + id + " to selected dict");
        }
    }

    public void deselect(int id)
    {
        selectedTable.Remove(id);
    }

    public void deselectAll()
    {
        selectedTable.Clear();
    }
}
