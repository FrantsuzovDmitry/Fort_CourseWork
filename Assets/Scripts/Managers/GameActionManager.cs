using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionManager : MonoBehaviour
{
    public static GameActionManager instance;

	private void Awake()
	{
		instance = this;
	}

	void Start()
    {
        
    }
}
