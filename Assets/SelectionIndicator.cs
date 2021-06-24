using System.Collections;
using System.Collections.Generic;
using UnitsAndTechs;
using UnityEngine;
using UnityEngine.UI;

public class SelectionIndicator : MonoBehaviour
{
    private IPlaceable element;
    [SerializeField] private Slider HealthBar;

    private Camera mainCamera;
    // Start is called before the first frame update

    public IPlaceable Element
    {
        get => element;
        set => element = value;
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void SetElement(IPlaceable element)
    {
        this.element = element;
        var size = element.GridMultiplier * element.GridSize;
        transform.localScale = new Vector3(size.x, transform.localScale.y, transform.localScale.z);
    }


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCamera.transform);
        if (element != null)
        {
            HealthBar.value = element.Health.CurrentAmount / (float) element.Health.MaxAmount;    
        }
        
    }
}
