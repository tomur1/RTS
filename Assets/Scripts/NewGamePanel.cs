using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGamePanel : MonoBehaviour
{
    private List<GameObject> playerBars;
    private List<Color> colors;
    private int numberOfPlayers;
    [SerializeField] private GameObject InitialPlayerBarPosition;
    [SerializeField] private GameObject PlayerBarPrefab;
    [SerializeField] private GameObject AddPlayerButtonPrefab;
    
    
    
    private void Awake()
    {
        playerBars = new List<GameObject>();
        colors = new List<Color>();
        numberOfPlayers = 1;
        AddColors();
    }
    
    void Start()
    {
        AddPlayer();
    }

    public void AddPlayer()
    {
        
    }
    
    private void AddColors()
    {
        colors.Add(Color.red);
        colors.Add(Color.blue);
        colors.Add(Color.yellow);
        colors.Add(Color.green);
        colors.Add(Color.magenta);
        colors.Add(Color.cyan);
        colors.Add(Color.gray);
        colors.Add(Color.black);
    }



}
