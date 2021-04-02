using UnitsAndTechs;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update

    private MyGrid grid;
    void Start()
    {
        grid = new MyGrid(10, 10, 1, Vector3.zero);
        grid.ShowLines();
        Debug.DrawLine(Vector3.zero, Vector3.one, Color.green, 100f, false);

        TownCenter tc = new TownCenter();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mousePos: " + Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("worldMousePos: " + hit.point);
                Debug.Log(grid.GetCellPos(hit.point));
            }
            
        }
        
    }
}
