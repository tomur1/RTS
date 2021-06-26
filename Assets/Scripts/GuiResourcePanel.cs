using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuiResourcePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MetalAmount;
    [SerializeField] private TextMeshProUGUI OilAmount;
    [SerializeField] private TextMeshProUGUI UraniumAmount;
    [SerializeField] private TextMeshProUGUI EnergyAmount;
    [SerializeField] private TextMeshProUGUI ScienceAmount;
    [SerializeField] private TextMeshProUGUI MetalProductionRate;
    [SerializeField] private TextMeshProUGUI OilProductionRate;
    [SerializeField] private TextMeshProUGUI UraniumProductionRate;
    [SerializeField] private TextMeshProUGUI EnergyProductionRate;
    [SerializeField] private TextMeshProUGUI ScienceProductionRate;
    private Player Player;

    private void Start()
    {
        Player = GameMaster.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        MetalAmount.SetText(Player.Metal.CollectedAmount.ToString());
        OilAmount.SetText(Player.Oil.CollectedAmount.ToString());
        UraniumAmount.SetText(Player.Uranium.CollectedAmount.ToString());
        EnergyAmount.SetText(Player.Energy.CollectedAmount.ToString());
        ScienceAmount.SetText(Player.Science.CollectedAmount.ToString());
        MetalProductionRate.SetText("0");
        OilProductionRate.SetText("0");
        UraniumProductionRate.SetText("0");
        EnergyProductionRate.SetText("0");
        ScienceProductionRate.SetText("0");
    }
}
