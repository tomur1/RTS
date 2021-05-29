using System;
using System.Collections.Generic;
using DefaultNamespace;
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
    public GuiManager GuiManager;
    public SelectionManager SelectionManager;
    public NormalInputHandler NormalInputHandler;
    private List<GameObject> DebugObjects;
    public Mode mode;

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
        mode = Mode.Normal;
        DebugObjects = new List<GameObject>();
        SelectionManager = new SelectionManager();
        NormalInputHandler = FindObjectOfType<NormalInputHandler>();
        GuiManager = FindObjectOfType<GuiManager>();
        new Pathfinding();
        
        MapCreator.FillGrid(grid);
        List<Player> players = new List<Player>();
        players.Add(new Player(Color.blue));
        MapCreator.SpawnPlayers(grid, players);
        var tc = (IMenuContainer) players[0].Buildings[0];
        GuiManager.SelectedObjectMenu.SwitchObject(tc);
        grid.ShowLines();
    }

    public GameObject AddElementToGrid(IPlaceable elementToAdd)
    
    {
        var elementUnityObject = Resources.Load<GameObject>(elementToAdd.AssetName);
        grid.AddElement(elementToAdd);
        return Instantiate(elementUnityObject, grid.GetWorldPos(elementToAdd.LeftTopCellCoord.x, elementToAdd.LeftTopCellCoord.y) +
                                               new Vector3(elementToAdd.GridSize.x * elementToAdd.GridMultiplier, 0, elementToAdd.GridSize.y * elementToAdd.GridMultiplier)/2, Quaternion.identity);
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
        switch (mode)
        {
            case Mode.Normal:
                NormalInputHandler.Handle();
                break;
            case Mode.Placing:
                PlacingInputHandle();
                break;
            case Mode.Paused:
                PauseInputHandle();
                break;
        }
    }

    private void PlacingInputHandle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Place the structure    
        }
    }

    private void PauseInputHandle()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }
}
