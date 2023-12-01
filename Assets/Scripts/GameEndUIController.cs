using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Delete the class
public class GameEndUIController : MonoBehaviour
{
    public TextMeshProUGUI winnerName;

    public void Initialize(Player winner)
    {
        winnerName.text = $"Player {winner.ID + 1} has won!"; 
    }
}
