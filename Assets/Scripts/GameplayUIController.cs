using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

// I suppose the better name is GameplayUIManager
public class GameplayUIController : MonoBehaviour
{
    public static GameplayUIController instance;
    public TextMeshProUGUI currentPlayerTurn;
    public Button endTurnButton;
    public Button getCardButton;

    private void Awake()
    {
        instance = this;
        SetupButton();
    }

    private void SetupButton()
    {
        // Assign an event to the buttons
        endTurnButton.onClick.AddListener(() =>
        {
            TurnManager.instance.EndTurn();
        });

        getCardButton.onClick.AddListener(() =>
        {
            CardManager.instance.GiveCardToPlayer(TurnManager.instance.currentPlayerTurn);
        });
    }

    public void UpdateCurrentPlayerTurn(int playerID)
    {
        currentPlayerTurn.text = $"Player {playerID + 1} turn!";
        StartCoroutine(BlinkLabel());
    }

    private IEnumerator BlinkLabel()
    {
        currentPlayerTurn.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        currentPlayerTurn.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        currentPlayerTurn.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        currentPlayerTurn.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        currentPlayerTurn.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        currentPlayerTurn.gameObject.SetActive(false);
    }

    public void ChangeCardPosition(CardController card, Transform position)
    {
        card.transform.SetParent(position);
    }
}
