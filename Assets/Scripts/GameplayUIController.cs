using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class GameplayUIController : MonoBehaviour
{
	public static GameplayUIController instance;

	//[SerializeField] private GameObject backgroundBlurPanel;
	//[SerializeField] private GameObject Deck;
	//[SerializeField] private TextMeshProUGUI numberOfCardsText;
	//[SerializeField] private TextMeshProUGUI currentPlayerTurnMessage;
	//[SerializeField] private Button endTurnButton;

	//private TextMeshProUGUI hintMessage;
	//private Button getCardButton;

	//private void Awake()
	//{
	//	instance = this;
	//	getCardButton = Deck.GetComponent<Button>();
	//	hintMessage = backgroundBlurPanel.GetComponentInChildren<TextMeshProUGUI>();
	//	//SetupButtons();
	//}

	//private void SetupButtons()
	//{
	//	endTurnButton.onClick.AddListener(() =>
	//	{
	//		Mediator.OnTurnEnded();
	//	});

	//	getCardButton.onClick.AddListener(() =>
	//	{
	//		Mediator.OnCardTaken();
	//	});
	//}

	//private void Start()
	//{
	//	UpdateCardNumberText(Assets.Scripts.Deck.deck.Count.ToString());
	//}

	//public void UpdateCardNumberText(string numberOfCards)
	//{
	//	numberOfCardsText.SetText(numberOfCards);
	//}

	//public void UpdateCurrentPlayerTurnLabel(int playerID)
	//{
	//	currentPlayerTurnMessage.text = $"Player {playerID + 1} turn!";
	//	StartCoroutine(BlinkLabel());
	//}

	//private IEnumerator BlinkLabel()
	//{
	//	currentPlayerTurnMessage.gameObject.SetActive(true);
	//	yield return new WaitForSeconds(0.5f);

	//	currentPlayerTurnMessage.gameObject.SetActive(false);
	//	yield return new WaitForSeconds(0.5f);

	//	currentPlayerTurnMessage.gameObject.SetActive(true);
	//	yield return new WaitForSeconds(0.5f);

	//	currentPlayerTurnMessage.gameObject.SetActive(false);
	//	yield return new WaitForSeconds(0.5f);

	//	currentPlayerTurnMessage.gameObject.SetActive(true);
	//	yield return new WaitForSeconds(0.5f);

	//	//currentPlayerTurnMessage.gameObject.SetActive(false);
	//}

	//public void ShowHint(string message)
	//{
	//	hintMessage.SetText(message);
	//}

	//public void BlurBackground()
	//{
	//	backgroundBlurPanel.SetActive(true);
	//}
}
