using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultipleObjectInformation : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private int size = 300;
    private List<GameObject> buttons;


    private void Awake()
    {
        buttons = new List<GameObject>();
    }

    public void UpdateView(SelectionManager selectionManager)
    {
        RemoveOldButtons();
        int numberOfElements = selectionManager.selectedTable.Count;
        float step = size * 2 / (float) numberOfElements;
        float positionX = -size + step/2;
        foreach (var pair in selectionManager.selectedTable)
        {
            var element = pair.Value;
            String buttonName = "";
            if (element.GetComponent<WorkerUnity>() != null)
            {
                var worker = element.GetComponent<WorkerUnity>().Worker;
                buttonName = worker.GetType().Name;

            }else if (element.GetComponent<SoldierUnity>() != null)
            {
                var soldier = element.GetComponent<SoldierUnity>().Soldier;
                buttonName = soldier.GetType().Name;
            }else if (element.GetComponent<TownCenterUnity>() != null)
            {
                var townCenter = element.GetComponent<TownCenterUnity>().TownCenter;
                buttonName = townCenter.GetType().Name;
            }
            CreateButton(positionX, buttonName, element);
            positionX += step;
        }
        
    }

    public void CreateButton(float positionX, string text, GameObject element)
    {
        var newButton = Instantiate(buttonPrefab, new Vector3(0, 0 ,0), Quaternion.identity, transform);
        newButton.transform.localPosition = new Vector3(positionX, 0, 0);
        newButton.GetComponentInChildren<TextMeshProUGUI>().SetText(text);
        newButton.GetComponent<Button>().onClick.AddListener(delegate { GameMaster.Instance.SetSelectionTo(element); });
        buttons.Add(newButton);
    }

    private void RemoveOldButtons()
    {
        foreach (var gameObject in buttons)
        {
            Destroy(gameObject);
        }
    }

    public void EmptyView()
    {
        RemoveOldButtons();
    }
}
