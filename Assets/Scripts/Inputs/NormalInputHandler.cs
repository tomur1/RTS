using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnitsAndTechs;
using UnityEngine;

public class NormalInputHandler : MonoBehaviour
{
    SelectionManager SelectionManager;
    RaycastHit hit;

    bool dragSelect;

    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector3 p1;
    Vector3 p2;

    //the corners of our 2d selection box
    Vector2[] corners;

    //the vertices of our meshcollider
    Vector3[] verts;
    Vector3[] vecs;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        dragSelect = false;
        SelectionManager = GameMaster.Instance.SelectionManager;
    }

    public void Handle()
    {
        SelectionManager.changed = false;
        if (Input.GetButtonDown("Cancel"))
        {
            GameMaster.Instance.SwitchPauseGame();
        }

        //1. when left mouse button clicked (but not released)
        if (Input.GetMouseButtonDown(0))
        {
            p1 = Input.mousePosition;
        }

        //4. Right click is released
        if (Input.GetMouseButtonUp(1))
        {
            GameMaster.Instance.RightClicked(Input.mousePosition);
        }

        //Group management
        int pressedNumber = PressedNumber();
        if (Input.GetKey(KeyCode.G) && pressedNumber != 0)
        {
            GameMaster.Instance.GuiManager.ShowMessage("Adding to group " + pressedNumber);
            //Add to group
            foreach (var pair in SelectionManager.selectedTable)
            {
                var element = pair.Value;
                if (element.GetComponent<WorkerUnity>() != null)
                {
                    var worker = element.GetComponent<WorkerUnity>().Worker;
                    worker.AddToGroup(pressedNumber);
                }else if (element.GetComponent<SoldierUnity>() != null)
                {
                    var soldier = element.GetComponent<SoldierUnity>().Soldier;
                    soldier.AddToGroup(pressedNumber);
                }
            }
        }
        
        if (Input.GetKey(KeyCode.R) && pressedNumber != 0)
        {
            GameMaster.Instance.GuiManager.ShowMessage("Removing from group " + pressedNumber);
            foreach (var pair in SelectionManager.selectedTable)
            {
                var element = pair.Value;
                if (element.GetComponent<WorkerUnity>() != null)
                {
                    var worker = element.GetComponent<WorkerUnity>().Worker;
                    worker.RemoveFromGroup(pressedNumber);
                }else if (element.GetComponent<SoldierUnity>() != null)
                {
                    var soldier = element.GetComponent<SoldierUnity>().Soldier;
                    soldier.RemoveFromGroup(pressedNumber);
                }
            }
        }

        if (pressedNumber != 0)
        {
            GameMaster.Instance.GuiManager.ShowMessage("Showing group " + pressedNumber);
            SelectionManager.deselectAll();
            var units = Group.GetGroupWithNumber(pressedNumber).Units;
            foreach (var unit in units)
            {
                SelectionManager.addSelected(unit.MapObject);
            }
            
        }
        
        //Click in menu. Ignore
        if (p1.y / Screen.height < 0.18981481)
        {
            return;
        }

        //2. while left mouse button held
        if (Input.GetMouseButton(0))
        {
            if ((p1 - Input.mousePosition).magnitude > 40)
            {
                dragSelect = true;
            }
        }

        //3. when mouse button comes up
        if (Input.GetMouseButtonUp(0))
        {
            if (dragSelect == false) //single select
            {
                Ray ray = Camera.main.ScreenPointToRay(p1);

                if (Physics.Raycast(ray, out hit, 50000.0f, LayerMask.GetMask("IPlaceable")))
                {
                    if (Input.GetKey(KeyCode.LeftShift)) //inclusive select
                    {
                        SelectionManager.addSelected(hit.transform.gameObject);
                    }
                    else //exclusive selected
                    {
                        SelectionManager.deselectAll();
                        SelectionManager.addSelected(hit.transform.gameObject);
                    }
                }
                else //if we didnt hit something
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //do nothing
                    }
                    else
                    {
                        SelectionManager.deselectAll();
                    }
                }
            }
            else //marquee select
            {
                verts = new Vector3[4];
                vecs = new Vector3[4];
                int i = 0;
                p2 = Input.mousePosition;
                corners = getBoundingBox(p1, p2);

                foreach (Vector2 corner in corners)
                {
                    Ray ray = _camera.ScreenPointToRay(corner);

                    if (Physics.Raycast(ray, out hit, 50000.0f, LayerMask.GetMask("Ground")))
                    {
                        verts[i] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        vecs[i] = ray.origin - hit.point;
                        Debug.DrawLine(_camera.ScreenToWorldPoint(corner), hit.point, Color.red, 1.0f);
                    }

                    i++;
                }

                //generate the mesh
                selectionMesh = generateSelectionMesh(verts, vecs);

                selectionBox = gameObject.AddComponent<MeshCollider>();
                var rigid = selectionBox.gameObject.AddComponent<Rigidbody>();
                selectionBox.sharedMesh = selectionMesh;
                selectionBox.convex = true;
                selectionBox.isTrigger = true;

                Debug.DrawLine(selectionBox.sharedMesh.bounds.min, selectionBox.sharedMesh.bounds.max, Color.green,
                    10.0f);

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    SelectionManager.deselectAll();
                }

                Destroy(selectionBox, 0.02f);
                Destroy(rigid, 0.02f);
            } //end marquee select

            dragSelect = false;
        }
        
        if (SelectionManager.changed)
        {
            GameMaster.Instance.SelectionChanged();
        }
    }


    private int PressedNumber()
    {
        KeyCode[] keyCodes =
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
        };

        int res = 0;
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                res = i + 1;
            }
        }

        return res;
    }


    private void OnGUI()
    {
        if (dragSelect == true)
        {
            var rect = ScreenDrawUtils.GetScreenRect(p1, Input.mousePosition);
            ScreenDrawUtils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            ScreenDrawUtils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

//create a bounding box (4 corners in order) from the start and end mouse position
    Vector2[] getBoundingBox(Vector2 p1, Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x) //if p1 is to the left of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else //if p1 is below p2
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else //if p1 is to the right of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else //if p1 is below p2
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }
        }

        Vector2[] corners = {newP1, newP2, newP3, newP4};
        return corners;
    }

//generate a mesh from the 4 bottom points
    Mesh generateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris =
        {
            0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5,
            7
        }; //map the tris of our cube

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            SelectionManager.addSelected(other.gameObject);
        }
        
        if (SelectionManager.changed)
        {
            GameMaster.Instance.SelectionChanged();
        }
    }
}