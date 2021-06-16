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
        //Renderer parentRenderer = transform.parent.GetComponent<Renderer>();
        //Debug.Log(parentRenderer.bounds);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCamera.transform);
        if (element != null)
        {
            HealthBar.value = element.Health.CurrentAmount / (float) element.Health.CurrentAmount;    
        }
        
    }
}
