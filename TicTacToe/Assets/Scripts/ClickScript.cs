using UnityEngine;
using System.Collections;

public class ClickScript : MonoBehaviour 
{
	//Variables
	private ClientNetworking client;

	public int cellNumber = 0;

	//Get Game Logic
	void Start()
	{
        client = ClientNetworking.getLocalClientNetworking();
    }

	//Click
	public void OnMouseDown()
	{
        if (client.isMyTurn())
        {
            if (GameState.Instance.getBoardCell(cellNumber / 3, cellNumber % 3) == Symbol.None) client.CmdSendPlay(cellNumber);
        }
	}

	//Hover
	public void OnMouseOver()
	{
        if (client.isMyTurn())
        {
            //Check if Cell is Empty & Create Ghost
            if (GameState.Instance.getBoardCell(cellNumber / 3, cellNumber % 3) == Symbol.None) GameView.Instance.createGhost(cellNumber, client.localSymbol);
        }
	}

	//Exit Hover
	public void OnMouseExit()
	{
        if (GameState.Instance.getBoardCell(cellNumber / 3, cellNumber % 3) == Symbol.None) GameView.Instance.destroyGhost(cellNumber);
	}
}
