using UnityEngine;
using System.Collections;

public class ClickScript : MonoBehaviour 
{
	//Variables
	private GameLogic logicScript;
	public int cellNumber = 0;

	//Get Game Logic
	void Start()
	{
		logicScript = GetComponentInParent<GameLogic>();
	}

	//Click
	public void OnMouseDown()
	{
		logicScript.makePlayHuman(cellNumber);
	}

	//Hover
	public void OnMouseOver()
	{
		logicScript.createGhost(cellNumber);
	}

	//Exit Hover
	public void OnMouseExit()
	{
		logicScript.destroyGhost(cellNumber);
	}
}
