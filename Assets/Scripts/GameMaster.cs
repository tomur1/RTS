using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnitsAndTechs;
using UnitsAndTechs.Units;
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
    public GameObject SelectionIndicatorPrefab;
    public Camera mainCamera;
    private List<GameObject> DebugObjects;
    private Dictionary<Unit, Coroutine> runningRoutines;
    public Mode mode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SelectionManager = new SelectionManager();
            mainCamera = Camera.main;
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
        runningRoutines = new Dictionary<Unit, Coroutine>();
        
        NormalInputHandler = FindObjectOfType<NormalInputHandler>();
        GuiManager = FindObjectOfType<GuiManager>();
        new Pathfinding();
        MapCreator.FillGrid(grid);
        List<Player> players = new List<Player>();
        players.Add(new Player(Color.blue));
        MapCreator.SpawnPlayers(grid, players);
        var tc = (IMenuContainer) players[0].Buildings[0];
        grid.ShowLines();
    }

    public void SelectionChanged()
    {
        if (SelectionManager.selectedTable.Count == 0)
        {
            GuiManager.SelectedObjectInformation.EmptyView();
            GuiManager.SelectedObjectMenu.EmptyView();
            return;
        }
        //If there is more than 1 unit selected
        if (SelectionManager.selectedTable.Count > 1)
        {
            GuiManager.MultipleObjectInformation.UpdateView(SelectionManager);
        }else
        {
            var element = SelectionManager.selectedTable.ElementAt(0).Value;
            if (element.GetComponent<WorkerUnity>() != null)
            {
                var worker = element.GetComponent<WorkerUnity>().Worker;
                GuiManager.SelectedObjectInformation.UpdateView(worker);
                GuiManager.SelectedObjectMenu.SwitchObject(worker);
            }else if (element.GetComponent<TownCenterUnity>() != null)
            {
                var townCenter = element.GetComponent<TownCenterUnity>().TownCenter;
                GuiManager.SelectedObjectInformation.UpdateView(townCenter);
                GuiManager.SelectedObjectMenu.SwitchObject(townCenter);
            }
        }
    }

    public void AddSelectionIndicator(GameObject selection)
    {
        var createdIndicator = Instantiate(SelectionIndicatorPrefab, selection.transform);
        if (selection.GetComponent<WorkerUnity>() != null)
        {
            createdIndicator.GetComponent<SelectionIndicator>().SetElement(selection.GetComponent<WorkerUnity>().Worker);
        }else if (selection.GetComponent<TownCenterUnity>() != null)
        {
            createdIndicator.GetComponent<SelectionIndicator>().SetElement(selection.GetComponent<TownCenterUnity>().TownCenter);
        }
    }
    
    public void RemoveSelectionIndicator(GameObject selection)
    {
        Destroy(selection.transform.Find("Selection Indicator(Clone)").gameObject);
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
        var path = CalcPathTo(DebugStartPos.transform.position, DebugEndPos.transform.position);
        ShowPath(path);
    }

    public List<Vector3> CalcPathTo(Vector3 startPos, Vector3 endPos)
    {
        return Pathfinding.Instance.FindPath(startPos, endPos);
    }

    public void ShowPath(List<Vector3> path)
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

    public void RightClicked(Vector3 mousePosition)
    {
        List<Unit> units = new List<Unit>();
        List<IUnitSpawner> unitSpawners = new List<IUnitSpawner>();
        
        foreach (var pair in SelectionManager.selectedTable)
        {
            var element = pair.Value;
            if (element.GetComponent<WorkerUnity>() != null)
            {
                var worker = element.GetComponent<WorkerUnity>().Worker;
                units.Add(worker);
            }else if (element.GetComponent<TownCenterUnity>() != null)
            {
                var townCenter = element.GetComponent<TownCenterUnity>().TownCenter;
                unitSpawners.Add(townCenter);
            }
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Cell clickedCell = null;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("hit point: " + hit.point);
            clickedCell = grid.GetCell(hit.point);
            Debug.Log("clicked cell: " + clickedCell.GridPosition);
        }

        // If there are only unitSpawnres change their spawnpoint
        if (units.Count == 0)
        {
            foreach (var spawner in unitSpawners)
            {
                spawner.SpawnPoint = clickedCell.GridPosition;
            }
        }

        foreach (var unit in units)
        {
            unit.HandleRightClick(clickedCell);
        }

    }
    
    public void MoveUnit(Unit unit, List<Vector3> path)
    {
        if (runningRoutines.ContainsKey(unit))
        {
            StopCoroutine(runningRoutines[unit]);
            runningRoutines[unit] = StartCoroutine(moveUnitEnum(unit, path));
        }
        else
        {
            runningRoutines.Add(unit, StartCoroutine(moveUnitEnum(unit, path)));    
        }
        
        
    }

    IEnumerator moveUnitEnum(Unit unit, List<Vector3> path)
    {
        var rb = unit.MapObject.GetComponent<Rigidbody>();
        foreach (var pathpoint in path)
        {
            while (Vector3.Distance(unit.MapObject.transform.position, pathpoint) > 0.1f)
            {
                var unitPosition = unit.MapObject.transform.position;
                var movingTowards = (unitPosition - pathpoint).normalized * (unit.MovementSpeed * Time.deltaTime);
                rb.MovePosition(unitPosition - movingTowards);
                yield return null;
            }
            grid.UpdateElementPosition(unit, grid.GetCell(pathpoint).GridPosition);
        }
        
        yield return null;
    }
}
