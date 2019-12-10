using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instance;
    //public GameObject player;
    public enum PlayerState { Alive, Dead }

    public PlayerState CurrentState
    {
        get { return _currentState; }
        private set { _currentState = value; }
    }

    private PlayerState _currentState;

	void Start () {
		if (Instance != null)
        {
            //If instance already exists, get rid of this object
            Destroy(gameObject);
            return;
        }
        //initialize the player manager if it does not exist
        Init();
	}

    private void Init()
    {
        Instance = this;
        CurrentState = PlayerState.Alive;
    }

    //sets the Player Manager to be in the game over state

    public void GameOver()
    {
        CurrentState = PlayerState.Dead;
    }


    // Update is called once per frame
    void Update () {
		
	}
}
