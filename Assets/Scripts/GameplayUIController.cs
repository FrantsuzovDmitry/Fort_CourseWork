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

    public Action onCardTaken;

    private void Awake()
    {
        instance = this;
        getCardButton = Deck.GetComponent<Button>();
		SetupButtons();
	}

	#region NOTIFICATION CONTROLLER CLASS:
    public void ShowNotification(string message)
    {
        Debug.Log(message);
    }
	#endregion

	private void SetupButtons()
    {
        // Assign an event to the buttons
        endTurnButton.onClick.AddListener(() =>
        {
            TurnManager.instance.EndTurn();
        });

        getCardButton.onClick.AddListener(() =>
        {
            CardManager.instance.TakeCardFromDeck(TurnManager.instance.currentPlayerTurn);
        });
    }

	private void Start()
	{
        UpdateCardNumberText();
	}

    private void UpdateCardNumberText()
    {
		numberOfCardsText.SetText(CardManager.instance.NumberOfCardInDeck.ToString());
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

    public void ChangeCardPosition(CardController card, Transform position)
    {
        card.transform.SetParent(position);
    }

	private void OnEnable()
	{
        Observer.onCardTaken += UpdateCardNumberText;
	}
}
