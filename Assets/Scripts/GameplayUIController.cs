using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

// I suppose the better name is GameplayUIManager
public class GameplayUIController : MonoBehaviour
{
    public static GameplayUIController instance;
    public TextMeshProUGUI currentPlayerTurnMessage;
    public Button endTurnButton;
    public Button getCardButton;
    public GameObject Deck;

    [SerializeField] private TextMeshProUGUI numberOfCardsText;

    private void Awake()
    {
        instance = this;
        getCardButton = Deck.GetComponent<Button>();
		SetupButtons();
	}

	private void SetupButtons()
    {
        // Assign an event to the buttons
        endTurnButton.onClick.AddListener(() =>
        {
            TurnManager.instance.EndTurn();
        });

        getCardButton.onClick.AddListener(() =>
        {
            Mediator.OnCardTaken();
        });
    }

	private void Start()
	{
        UpdateCardNumberText(Assets.Scripts.Deck.deck.Count.ToString());
	}

    public void UpdateCardNumberText(string numberOfCards)
    {
		numberOfCardsText.SetText(numberOfCards);
	}

	public void UpdateCurrentPlayerTurn(int playerID)
    {
        currentPlayerTurnMessage.text = $"Player {playerID + 1} turn!";
        StartCoroutine(BlinkLabel());
    }

    private IEnumerator BlinkLabel()
    {
        currentPlayerTurnMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        currentPlayerTurnMessage.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        currentPlayerTurnMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        currentPlayerTurnMessage.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        currentPlayerTurnMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        //currentPlayerTurnMessage.gameObject.SetActive(false);
    }
}
