using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject winnerPanel;

    [SerializeField] private TextMeshProUGUI winnerName;

    private void Awake()
    {
        instance = this;
    }

    // I suppose the better name is ShowWinner[Panel/Screen]
    // And maybe this function is better to be in GameplayUIController/Manager OR Move methods from the controller here
    public void ShowWinnerPanel()
    {
        var winner = PlayerManager.instance.DefineWinner();
        //winnerPanel.gameObject.SetActive(true);
        winnerPanel.SetActive(true);
        //gameEndUI.Initialize(winner);
        winnerName.text = $"Player {winner.ID + 1} has won!";
    }

    private void OnEnable()
    {
        Observer.onGameStopped += ShowWinnerPanel;
    }
}
