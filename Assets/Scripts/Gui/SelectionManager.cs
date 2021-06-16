using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager
{
    public Dictionary<int, GameObject> selectedTable;
    public bool changed;

    public SelectionManager()
    {
        selectedTable = new Dictionary<int, GameObject>();
        changed = false;
    }

    public void addSelected(GameObject go)
    {
        int id = go.GetInstanceID();

        if (!(selectedTable.ContainsKey(id)))
        {
            selectedTable.Add(id, go);
            Debug.Log("Added " + go.name + " to selected dict");
        }
        changed = true;
    }

    public void deselect(int id)
    {
        selectedTable.Remove(id);
        changed = true;
    }

    public void deselectAll()
    {
        selectedTable.Clear();
        changed = true;
    }
}
