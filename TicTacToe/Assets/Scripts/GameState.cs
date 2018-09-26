using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Symbol
{
	None,
	Circle,
	Cross,
    Draw
};

public class GameState : MonoBehaviour
{
	private static GameState instance;
    public static GameState Instance
    {
        get
        {
            return instance;
        }
    }

    //On Object Awake
    private void Awake()
    {
        //Check Singleton
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    //On Object Destroy (Safeguard)
    public void OnDestroy()
    {
        instance = null;
    }

    //Variables
    private Symbol[,] board;

    //Singleton Constructor
    private GameState()
    {
        board = new Symbol[3, 3];
    }

    //Reset Board
    public void resetBoard()
	{
		for(int i = 0; i < 3; i++)
		{
			for(int j = 0; j < 3; j++)
			{
				board[i, j] = Symbol.None;
			}
		}
	}

	//Get Cell
	public Symbol getBoardCell(int row, int column)
	{
		return board[row, column];
	}

	//Get Entire Board
	public Symbol[,] getBoard()
	{
		return board;
	}

	//Update Cell
	public void updateCell(int row, int column, Symbol symbol)
	{
		board[row, column] = symbol;
	}
}
