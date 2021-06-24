using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DefaultNamespace;
using UnitsAndTechs;
using UnitsAndTechs.Units;
using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;

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
    private List<Player> playersInGame;
    public Player player;
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
        playersInGame = new List<Player>();
        player = new Player(Color.blue);
        playersInGame.Add(player);
        MapCreator.SpawnPlayers(grid, playersInGame);
        
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
                if (!townCenter.ConstructionCost.InConstruction)
                {
                    GuiManager.SelectedObjectMenu.SwitchObject(townCenter);
                }
                
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
        if (selection != null)
        {
            Destroy(selection.transform.Find("Selection Indicator(Clone)").gameObject);    
        }
    }

    public GameObject AddElementToGrid(IPlaceable elementToAdd)
    {
        grid.AddElement(elementToAdd);
        return CreateObject(elementToAdd);
    }

    public GameObject CreateObject(IPlaceable elementToAdd)
    {
        var elementUnityObject = Resources.Load<GameObject>(elementToAdd.AssetName);
        return Instantiate(elementUnityObject, grid.UnityPlacePosition(elementToAdd, elementToAdd.LeftTopCellCoord), Quaternion.identity);
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
        grid.ShowLines();
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
            clickedCell = grid.GetCell(hit.point);
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
            if (runningRoutines[unit] != null)
            {
                StopCoroutine(runningRoutines[unit]);
            }
            
            runningRoutines[unit] = StartCoroutine(moveUnitEnum(unit, path));
        }
        else
        {
            runningRoutines.Add(unit, StartCoroutine(moveUnitEnum(unit, path)));    
        }
    }
    
    public void MoveUnitWithAction(Unit unit, String actionName, IPlaceable target)
    {
        if (runningRoutines.ContainsKey(unit))
        {
            if (runningRoutines[unit] != null)
            {
                StopCoroutine(runningRoutines[unit]);
            }
            runningRoutines[unit] = StartCoroutine(moveUnitWithActionEnum(unit, actionName, target));
        }
        else
        {
            runningRoutines.Add(unit, StartCoroutine(moveUnitWithActionEnum(unit, actionName, target)));    
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
    
    
    
    IEnumerator moveUnitWithActionEnum(Unit unit, String actionName, IPlaceable target)
    {
        while (true)
        {
            if (grid.InRange(unit.LeftTopCellCoord, unit.getRange(), target))
            {
                PerformAction(actionName, unit, new object[]{new object[]{target}});
                break;
            }
            var closestCell = grid.GetClosestCellInRangeTo(unit, target);
            var path = Pathfinding.Instance.FindPath(unit.MapObject.transform.position, grid.GetWorldPos(closestCell.GridPosition.x, closestCell.GridPosition.y)).GetRange(0,2);
            yield return moveUnitEnum(unit, path);
        }
    }
    
    public void RepairBuilding(Unit unit, Building target)
    {
        if (runningRoutines.ContainsKey(unit))
        {
            if (runningRoutines[unit] != null)
            {
                StopCoroutine(runningRoutines[unit]);
            }
            runningRoutines[unit] = StartCoroutine(RepairBuildingEnum( (Worker) unit, target));
        }
        else
        {
            runningRoutines.Add(unit, StartCoroutine(RepairBuildingEnum((Worker) unit, target)));    
        }
    }

    public void DestroyMapObject(GameObject mapObject)
    {
        Destroy(mapObject);
    }
    
    IEnumerator RepairBuildingEnum(Worker worker, Building target)
    {
        while (grid.InRange(worker.LeftTopCellCoord, worker.getRange(), target))
        {
            if (target.ConstructionCost.InConstruction)
            {
                target.AddConstructionPoints(worker.BuildingSpeed);
                Debug.Log(target.ConstructionCost.ConstructionPoints);
            }else if (target.Health.NotFull)
            {
                target.Health.AddHealth(worker.BuildingSpeed);
            }
            
            yield return new WaitForSeconds(1f);
        }
    }

    public UnityAction PerformAction(String actionName, Object actionObject, Object[] parameters = null)
    {
        Type thisType = actionObject.GetType();
        MethodInfo theMethod = thisType.GetMethod(actionName);
        theMethod.Invoke(actionObject, parameters);
        return null;
    } 
}
