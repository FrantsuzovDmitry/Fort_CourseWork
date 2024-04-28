using Assets.Scripts.Cards;
using System;
using System.Collections;
using System.Collections.Generic;

public class CurrentUserIntentionState
{
	public GroupOfCharacters GetAttackersGroup() => new GroupOfCharacters(selectedCharacters);
	public Character SelectedCharacter { get; private set; }	// TODO: remove this and make field MakeSelectedCharacters (single character in case exchanging card, so what?)
	public Fortress SelectedFortToAttack { get; private set; }


	private readonly List<Character> selectedCharacters = new List<Character>();

	public void RememberUserFortSelection(Fortress fort)
	{
		SelectedFortToAttack = fort;
	}

	public void OnAttackStopped()
	{
		SelectedFortToAttack = null;
		ClearGroup();
	}

	public void RemoveCharacterFromGroup(Character character)
	{
		selectedCharacters.Remove(character);
	}

	public void AddCharacterToGroup(Character character)
	{
		selectedCharacters.Add(character);
	}

	public void ClearGroup()
	{
		selectedCharacters.Clear();
	}

	public void RememberUserCharacterSelection(Character character)
	{
		if (character != null)
			SelectedCharacter = character;
		else
			throw new Exception("Null character!");
	}
}
