using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataHolders;
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
    private Dictionary<IPlaceable, Coroutine> runningRoutines;
    public List<Player> playersInGame;
    public Player player;
    public Mode mode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SelectionManager = new SelectionManager();
            mainCamera = Camera.main;
            player = new Player(Color.red);
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
        runningRoutines = new Dictionary<IPlaceable, Coroutine>();
        NormalInputHandler = FindObjectOfType<NormalInputHandler>();
        GuiManager = FindObjectOfType<GuiManager>();
        new Pathfinding();
        MapCreator.FillGrid(grid);
        playersInGame = new List<Player>();
        playersInGame.Add(player);
        playersInGame.Add(new Player(Color.blue));
        MapCreator.SpawnPlayers(grid, playersInGame);
        Group.InitializeAllGroups();
    }

    public void SelectionChanged()
    {
        if (SelectionManager.selectedTable.Count == 0)
        {
            GuiManager.MultipleObjectInformation.gameObject.SetActive(false);
            GuiManager.SelectedObjectInformation.gameObject.SetActive(true);
            GuiManager.SelectedObjectInformation.EmptyView();
            GuiManager.SelectedObjectMenu.EmptyView();
        } else if (SelectionManager.selectedTable.Count > 1)
        {
            GuiManager.MultipleObjectInformation.gameObject.SetActive(true);
            GuiManager.MultipleObjectInformation.UpdateView(SelectionManager);
            GuiManager.SelectedObjectInformation.EmptyView();
            GuiManager.SelectedObjectInformation.gameObject.SetActive(false);
            GuiManager.SelectedObjectMenu.EmptyView();
        }else
        {
            GuiManager.MultipleObjectInformation.gameObject.SetActive(false);
            GuiManager.SelectedObjectInformation.gameObject.SetActive(true);
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
                
            }else if (element.GetComponent<SoldierUnity>() != null)
            {
                var soldier = element.GetComponent<SoldierUnity>().Soldier;
                GuiManager.SelectedObjectInformation.UpdateView(soldier);
                GuiManager.SelectedObjectMenu.EmptyView();
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
        }else if (selection.GetComponent<SoldierUnity>() != null)
        {
            createdIndicator.GetComponent<SelectionIndicator>().SetElement(selection.GetComponent<SoldierUnity>().Soldier);
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

    public void SaveGame()
    {
        DataSaver.SaveData();
    }

    public void LoadGame()
    {
        var dataHolder = DataSaver.LoadData();
        ClearGame();
        LoadFromDataHolder(dataHolder);
    }

    private void LoadFromDataHolder(DataHolder dataHolder)
    {
        foreach (var playerInfoHolder in dataHolder.PlayersData)
        {
            var player = new Player(playerInfoHolder.Color);
            player.Energy = playerInfoHolder.Energy;
            player.Uranium = playerInfoHolder.Uranium;
            player.Metal = playerInfoHolder.Metal;
            player.Science = playerInfoHolder.Science;
            player.Oil = playerInfoHolder.Oil;

            player.PlayerScore = playerInfoHolder.PlayerScore;
            foreach (var element in playerInfoHolder.Buildings)
            {
                if (element.TypeName == nameof(TownCenter))
                {
                    var newElement = new TownCenter(false);
                    newElement.InitValues(player, element.LeftTopCellCoord);
                    Health.CreateAndAssign(element.Health.CurrentAmount, element.Health.MaxAmount, newElement);
                    newElement.ConstructionCost = element.Cost;
                }
            }
            foreach (var unitInfoHolder in playerInfoHolder.Units)
            {
                Unit unit = null;
                if (unitInfoHolder.TypeName == nameof(Worker))
                {
                    unit = new Worker(player);
                    
                }else if (unitInfoHolder.TypeName == nameof(Soldier))
                {
                    unit = new Soldier(player);
                }
                
                unit.InitValues(player, unitInfoHolder.LeftTopCellCoord);
                Health.CreateAndAssign(unitInfoHolder.Health.CurrentAmount, unitInfoHolder.Health.MaxAmount, unit);
                unit.ConstructionCost = unitInfoHolder.Cost;
                foreach (var groupNumber in unitInfoHolder.Groups)
                {
                    unit.AddToGroup(groupNumber);
                }
            }
            playersInGame.Add(player);
            if (player.Color == dataHolder.MainPlayerColor)
            {
                this.player = player;
            }
        }
        
        foreach (var notPlayerElement in dataHolder.MapElements)
        {
            IPlaceable placeable = null;
            if (notPlayerElement.TypeName == nameof(OilSource))
            {
                placeable = new OilSource();
            }else if (notPlayerElement.TypeName == nameof(UraniumSource))
            {
                placeable = new UraniumSource();
            }
            placeable.InitValues(null, notPlayerElement.LeftTopCellCoord);
        }
        
        mainCamera.transform.position = dataHolder.CameraPosition;
    }

    private void ClearGame()
    {
        foreach (var element in grid.GetElementsOnMap().ToList())
        {
            element.Destroyed();
        }
        playersInGame.Clear();
        GuiManager.SelectedObjectInformation.EmptyView();
        GuiManager.SelectedObjectMenu.EmptyView();
        GuiManager.MultipleObjectInformation.EmptyView();
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

    public void SwitchPauseGame()
    {
        if (mode == Mode.Normal)
        {
            mode = Mode.Paused;
            Time.timeScale = 0;
        }
        else if(mode == Mode.Paused)
        {
            mode = Mode.Normal;
            Time.timeScale = 1;
        }

        GuiManager.SwitchPausePanelActive();
    }

    private void PlacingInputHandle()
    {
        
    }

    private void PauseInputHandle()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SwitchPauseGame();
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
            else if (element.GetComponent<SoldierUnity>() != null)
            {
                var soldier = element.GetComponent<SoldierUnity>().Soldier;
                units.Add(soldier);
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

    public void SetSelectionTo(GameObject gameObject)
    {
        SelectionManager.deselectAll();
        SelectionManager.addSelected(gameObject);
        SelectionChanged();
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
        foreach (var pathPoint in path)
        {
            while (Vector3.Distance(unit.MapObject.transform.position, pathPoint) > 0.1f)
            {
                var unitPosition = unit.MapObject.transform.position;
                var movingTowards = (unitPosition - pathPoint).normalized * (unit.MovementSpeed * Time.deltaTime);
                rb.MovePosition(unitPosition - movingTowards);
                yield return null;
            }
            grid.UpdateElementPosition(unit, grid.GetCell(pathPoint).GridPosition);
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
            else
            {
                var closestCell = grid.GetClosestCellInRangeTo(unit, target);
                var path = Pathfinding.Instance.FindPath(unit.MapObject.transform.position, grid.GetWorldPos(closestCell.GridPosition.x, closestCell.GridPosition.y)).GetRange(0,2);
                yield return moveUnitEnum(unit, path);
            }
            
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
            }else if (target.Health.NotFull)
            {
                target.Health.AddHealth(worker.BuildingSpeed);
            }
            
            yield return new WaitForSeconds(1f);
        }
    }

    public void Attack(Soldier soldier, IPlaceable target)
    {
        if (runningRoutines.ContainsKey(soldier))
        {
            if (runningRoutines[soldier] != null)
            {
                StopCoroutine(runningRoutines[soldier]);
            }
            runningRoutines[soldier] = StartCoroutine(AttackEnum(soldier, target));
        }
        else
        {
            runningRoutines.Add(soldier, StartCoroutine(AttackEnum(soldier, target)));    
        }
    }

    public IEnumerator AttackEnum(Soldier soldier, IPlaceable target)
    {
        while (target.Health.CurrentAmount > 0)
        {
            if (grid.InRange(soldier.LeftTopCellCoord, soldier.getRange(), target))
            {
                target.Health.RemoveHealth(soldier.AttackAbility.attackPower);
                yield return new WaitForSeconds(soldier.AttackAbility.attackDelay);
            }
            else
            {
                var closestCell = grid.GetClosestCellInRangeTo(soldier, target);
                var path = Pathfinding.Instance.FindPath(soldier.MapObject.transform.position, grid.GetWorldPos(closestCell.GridPosition.x, closestCell.GridPosition.y)).GetRange(0,2);
                yield return moveUnitEnum(soldier, path);
            }
        }
        
    }

    public void StopCoroutinesForObject(IPlaceable placeable)
    {
        if (runningRoutines.ContainsKey(placeable))
        {
            if (runningRoutines[placeable] != null)
            {
                StopCoroutine(runningRoutines[placeable]);
            }
        }
    }

    public void PerformAction(String actionName, Object actionObject, Object[] parameters = null)
    {
        Type thisType = actionObject.GetType();
        MethodInfo theMethod = thisType.GetMethod(actionName);
        theMethod.Invoke(actionObject, parameters);
    }
    
}
