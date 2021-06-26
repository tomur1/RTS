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
            GameMaster.Instance.AddSelectionIndicator(go);
        }
        changed = true;
    }

    public void deselect(int id)
    {
        GameMaster.Instance.RemoveSelectionIndicator(selectedTable[id]);
        selectedTable.Remove(id);
        changed = true;
    }

    public void deselectAll()
    {
        foreach (var element in selectedTable.Values)
        {
            GameMaster.Instance.RemoveSelectionIndicator(element);
        }
        selectedTable.Clear();
        changed = true;
    }
}
