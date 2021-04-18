using System;
using System.Collections.Generic;
using UnitsAndTechs;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameMaster Instance { get; private set; }
    public MyGrid grid;
    public GameObject DebugPrefab;
    public GameObject DebugStartPos;
    public GameObject DebugEndPos;
    private List<GameObject> DebugObjects;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        grid = new MyGrid(100, 100, 1, Vector3.zero);
        DebugObjects = new List<GameObject>();
        grid.ShowLines();
        Debug.DrawLine(Vector3.zero, Vector3.one, Color.green, 100f, false);
        MapCreator.FillGrid(grid);
        MapCreator.InitMapObjects(grid);
        new Pathfinding();
        
    }

    public void CalcPathToDebug()
    {
        CalcPathTo(DebugStartPos.transform.position, DebugEndPos.transform.position);
    }

    public void CalcPathTo(Vector3 startPos, Vector3 endPos)
    {
        var path = Pathfinding.Instance.FindPath(startPos, endPos);
        ShowPath(path);
    }

    private void ShowPath(List<Vector3> path)
    {
        foreach (var debugObject in DebugObjects)
        {
            Destroy(debugObject);
        }
        
        foreach (var vec3 in path)
        {
            DebugObjects.Add(Instantiate(DebugPrefab, vec3, Quaternion.identity));
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(grid.GetCellPos(hit.point));
            }
            
        }
        
    }
}
