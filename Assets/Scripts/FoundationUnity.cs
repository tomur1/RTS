using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnitsAndTechs;
using UnityEngine;

public class FoundationUnity : MonoBehaviour
{
    public Building Element { get; set; }
    public Worker Worker { get; set; }

    RaycastHit hit;
    bool following;
    private Color originalColor;
    private Renderer[] renderers;
    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalColor = renderers[0].material.color;
    }

    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //raycast from previous mouse pointer position
        var grid = GameMaster.Instance.grid;
        Cell choosenCell = null;
        if (Element != null)
        {
            if (Physics.Raycast(ray, out hit, 50000.0f, LayerMask.GetMask("Ground"))) //if we hit ground
            {
                var cell = grid.GetCell(hit.point);
                choosenCell = cell;
                transform.position = grid.UnityPlacePosition(Element, cell.GridPosition);
            }
            else
            {
                return;
            }
        }

        if (grid.CanPlace(Element, choosenCell.GridPosition))
        {
            foreach (var renderer in renderers)
            {
                renderer.material.color = originalColor;
            }
        }
        else
        {
            foreach (var renderer in renderers)
            {
                renderer.material.color = Color.red;
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (grid.CanPlace(Element))
            {
                following = false;
                Element.InitValuesFoundation(GameMaster.Instance.player, choosenCell.GridPosition);
                GameMaster.Instance.MoveUnitWithAction(Worker, "RepairOrBuild", Element);
                GameMaster.Instance.mode = Mode.Normal;
                Destroy(this);
            }
            
        }else if (Input.GetButtonDown("Cancel"))
        {
            GameMaster.Instance.mode = Mode.Normal;
            Destroy(gameObject);
            
        }
    }
}
