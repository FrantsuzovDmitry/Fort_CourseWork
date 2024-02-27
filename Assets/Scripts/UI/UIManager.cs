using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIButtons : byte
{
	StartAttack,
	AttackConfirmation,
	SelectionCharacterToGiveConfirmation
}

public enum UIElementState : byte
{
	ON, OFF
}

/// <summary>
/// Buttons, hints, warnings and gameplay events
/// </summary>
public class UIManager : MonoBehaviour
{
	public static UIManager instance;
	public GameObject winnerPanel;

	[SerializeField] private TextMeshProUGUI winnerName;
	[SerializeField] private TextMeshProUGUI numberOfCardsText;
	[SerializeField] private TextMeshProUGUI currentPlayerTurnMessage;
	[SerializeField] private Button startAttackButton;
	[SerializeField] private Button attackConfirmationButton;
	[SerializeField] private Button exchangeConfirmationButton;
	[SerializeField] private GameObject Deck;
	[SerializeField] private GameObject hintPanel;
	[SerializeField] private Button endTurnButton;

	private List<Button> UIbuttons;
	private Button getCardButton;
	private TextMeshProUGUI hintMessage;

	private void Awake()
	{
		instance = this;
		UIbuttons = new List<Button>() { startAttackButton, attackConfirmationButton, exchangeConfirmationButton};
		getCardButton = Deck.GetComponent<Button>();
		hintMessage = hintPanel.GetComponentInChildren<TextMeshProUGUI>();
		SetupButtons();
	}

	private void SetupButtons()
	{
		endTurnButton.onClick.AddListener(() =>
		{
			Mediator.OnTurnEnded();
		});

		getCardButton.onClick.AddListener(() =>
		{
			Mediator.OnCardTaken();
		});

		startAttackButton.onClick.AddListener(() =>
		{
			Mediator.OnCreatingAttackersGroupStarted();
		});

		attackConfirmationButton.onClick.AddListener(() =>
		{
			Mediator.OnFortressTriedAttacked();
		});

		exchangeConfirmationButton.onClick.AddListener(() =>
		{
			Mediator.OnCardGiven();
		});
	}

	public void UpdateCardNumberText(int numberOfCards)
	{
		numberOfCardsText.SetText(numberOfCards.ToString());
	}

	public void UpdateCurrentPlayerTurnLabel(int playerID)
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
	}

	public void ShowHint(string message)
	{
		hintMessage.SetText(message);
	}

	public void ToggleCardSelectionPanelVisibility(UIElementState state)
	{
		if (state == UIElementState.ON)
			hintPanel.SetActive(true);
		else
			hintPanel.SetActive(false);
	}

	public void ShowWinnerPanel()
	{
		var winner = PlayerManager.instance.DefineWinner();
		winnerPanel.SetActive(true);
		winnerName.text = $"Player {winner.ID + 1} has won!";
	}

	public void ShowButton(UIButtons button)
	{
		foreach (Button btn in UIbuttons)
		{
			btn.gameObject.SetActive(false);
		}

		switch (button)
		{
			case UIButtons.StartAttack:
				startAttackButton.gameObject.SetActive(true);
				break;
			case UIButtons.AttackConfirmation:
				attackConfirmationButton.gameObject.SetActive(true);
				break;
			case UIButtons.SelectionCharacterToGiveConfirmation:
				exchangeConfirmationButton.gameObject.SetActive(true);
				break;
		}
	}

	// Probably should remember active button and set inactive only it
	public void HideAllButtons() 
	{
		foreach (Button btn in UIbuttons)
		{
			btn.gameObject.SetActive(false);
		}
	}

	public void DebugNotification(string message)
	{
		Debug.Log(message);
	}

	public void ShowWarningMessage(string message)
	{
		// TODO: make more cute. Panel for example.
		DebugNotification(message);
	}
}
