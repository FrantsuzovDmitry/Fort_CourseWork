using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIButtons
{
	StartAttackButton,
	AttackConfirmationButton
}

public class UIManager : MonoBehaviour
{
	public static UIManager instance;
	public GameObject winnerPanel;

	[SerializeField] private TextMeshProUGUI winnerName;
	[SerializeField] private Button startAttackButton;
	[SerializeField] private Button attackConfirmationButton;

	private List<Button> UIbuttons;

	private void Awake()
	{
		instance = this;
		UIbuttons = new List<Button>() { startAttackButton, attackConfirmationButton };

		startAttackButton.onClick.AddListener(() =>
		{
			TurnManager.instance.StartCreatingOfGroupOfCharacters();
			ShowButton(UIButtons.AttackConfirmationButton);
		});
		attackConfirmationButton.onClick.AddListener(() =>
		{
			//TODO: Сделать так, чтобы эта кнопка вызывала все необходимые вещи. 
			//		Наверное, это надо сделать после создания класса намерений пользователя (CurrentState* который).
			//Observer.onFortressAttacked();
		});
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

	public void ShowButton(UIButtons button)
	{
		foreach (Button btn in UIbuttons)
		{
			btn.gameObject.SetActive(false);
		}

		switch (button)
		{
			case UIButtons.StartAttackButton:
				startAttackButton.gameObject.SetActive(true);
				break;
			case UIButtons.AttackConfirmationButton:
				attackConfirmationButton.gameObject.SetActive(true);
				break;
		}
	}
}
