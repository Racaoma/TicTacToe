using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameView : MonoBehaviour
{
	//Sprites
	public Sprite circle;
	public Sprite cross;
    public Sprite circle_outline;
    public Sprite cross_outline;

    public Sprite blueGranny;
    public Sprite redGranny;

    //Color of Symbols
    private Color colorCircle;
    private Color colorCross;

    //Players Variables
    public Image player1Granny;
	public Image player2Granny;
    public Text player1Text;
    public Text player2Text;
    public Image player1Symbol;
    public Image player2Symbol;

    //Victory Panel
    public GameObject winnerPanel;

	//Highlight Decay
	private float highlightDecay = 1.75f;

	//Start Method
	void Start() 
	{
		resetBoard();
    }

	//Set Random Colors for Symbols
	public void setSymbols(Symbol p1, PlayerType typeP1, PlayerType typeP2)
	{
		//Define Random Color for Each Player
		if(Random.value >= 0.5)
		{
			colorCircle = Color.red;
			colorCross = Color.blue;
		}
		else
		{
			colorCircle = Color.blue;
			colorCross = Color.red;
		}

        //Update Symbols and Random Color for Each Player
        if (p1 == Symbol.Circle)
        {
            //Update Symbol
            player1Symbol.sprite = circle;
            player2Symbol.sprite = cross;

            //Update Random Color & Granny's Sprites
            if(colorCircle == Color.red)
            {
                player1Granny.sprite = redGranny;
                player1Symbol.color = Color.red;
                player2Granny.sprite = blueGranny;
                player2Symbol.color = Color.blue;
            }
            else
            {
                player1Granny.sprite = blueGranny;
                player1Symbol.color = Color.blue;
                player2Granny.sprite = redGranny;
                player2Symbol.color = Color.red;
            }
        }
        else
        {
            //Update Symbol
            player1Symbol.sprite = cross;
            player2Symbol.sprite = circle;

            //Update Random Color & Granny's Sprites
            if (colorCross == Color.red)
            {
                player1Granny.sprite = redGranny;
                player1Symbol.color = Color.red;
                player2Granny.sprite = blueGranny;
                player2Symbol.color = Color.blue;
            }
            else
            {
                player1Granny.sprite = blueGranny;
                player1Symbol.color = Color.blue;
                player2Granny.sprite = redGranny;
                player2Symbol.color = Color.red;
            }
        }

        //Update Text - Default = Human
        if (typeP1 == PlayerType.AI_Easy) player1Text.text = "Easy AI";
        else if (typeP1 == PlayerType.AI_Medium) player1Text.text = "Medium AI";
        else if (typeP1 == PlayerType.AI_Hard) player1Text.text = "Hard AI";

        if (typeP2 == PlayerType.AI_Easy) player2Text.text = "Easy AI";
        else if (typeP2 == PlayerType.AI_Medium) player2Text.text = "Medium AI";
        else if (typeP2 == PlayerType.AI_Hard) player2Text.text = "Hard AI";
    }

	//Reset Entire Board 
	public void resetBoard()
	{
		for (int i = 0; i < this.gameObject.transform.childCount; i++) 
		{
            Transform cell = this.gameObject.transform.GetChild(i);
            cell.GetComponent<SpriteRenderer>().sprite = null;

            //Remove Outlines
            foreach (Transform child in cell)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        //Disable Victory Panel
        winnerPanel.SetActive(false);

    }

	//Update Exact Cell
	public void updateCell(int cellNumber, Symbol symbol)
	{
		//Update Symbol
		if(symbol == Symbol.Circle)
		{
			this.gameObject.transform.GetChild(cellNumber).GetComponent<SpriteRenderer>().sprite = circle;
			this.gameObject.transform.GetChild (cellNumber).GetComponent<SpriteRenderer>().color = colorCircle;
		}
		else
		{
			this.gameObject.transform.GetChild(cellNumber).GetComponent<SpriteRenderer>().sprite = cross;
			this.gameObject.transform.GetChild (cellNumber).GetComponent<SpriteRenderer>().color = colorCross;
		} 
	}

    //Outline Winning Cell
    public void outlineCell(int cellNumber)
    {
        //Create Object
        GameObject highlight = new GameObject();
        Transform parent = this.gameObject.transform.GetChild(cellNumber);
        highlight.transform.SetParent(parent);

        //Set Correct Position & Scale
        highlight.transform.position = parent.transform.position;
        highlight.transform.localScale = new Vector2(1.1f, 1.1f);

        //Create Outline
        SpriteRenderer spriteRenderer = highlight.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        spriteRenderer.color = Color.yellow;

        //Set Correct Sprite
        if (parent.GetComponent<SpriteRenderer>().sprite == circle) spriteRenderer.sprite = circle_outline;
        else if (parent.GetComponent<SpriteRenderer>().sprite == cross) spriteRenderer.sprite = cross_outline;
    }

    //Create Ghost
    public void createGhost(int cellNumber, Symbol symbol)
	{
		if(symbol == Symbol.Circle) this.gameObject.transform.GetChild(cellNumber).GetComponent<SpriteRenderer>().sprite = circle;
		else this.gameObject.transform.GetChild(cellNumber).GetComponent<SpriteRenderer>().sprite = cross;

		//Make it Transparent
		this.gameObject.transform.GetChild (cellNumber).GetComponent<SpriteRenderer> ().color = new Color (0f,0f,0f,0.3f);
	}

	//Destroy Ghost
	public void destroyGhost(int cellNumber)
	{
		this.gameObject.transform.GetChild(cellNumber).GetComponent<SpriteRenderer>().sprite = null;
	}

	//Highlight Player
	public void highlightPlayer(Player player)
	{
		if(player == Player.Player1) player1Granny.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
		else player2Granny.transform.localScale = new Vector3(-1.3f, 1.3f, 1f);
	}

	//Update Method
	void Update()
	{
        //Decay Highlight
        if (player1Granny.transform.localScale.x > 1.15f || player1Granny.transform.localScale.y > 1.15f) player1Granny.transform.localScale = Vector3.Lerp(player1Granny.transform.localScale, new Vector3(1.15f, 1.15f, 1f), highlightDecay * Time.deltaTime);
        if (player2Granny.transform.localScale.x < -1.15f || player2Granny.transform.localScale.y > 1.15f) player2Granny.transform.localScale = Vector3.Lerp(player2Granny.transform.localScale, new Vector3(-1.15f, 1.15f, 1f), highlightDecay * Time.deltaTime);
	}

	//Display Winner
	public void displayWinner(Player player, PlayerType playerType)
	{
        //Enable Panel
        winnerPanel.SetActive(true);

        //Display Correct Message
        if(player == Player.None) winnerPanel.GetComponentInChildren<Text>().text = "Draw";
        else if(player == Player.Player1 && playerType == PlayerType.Human) winnerPanel.GetComponentInChildren<Text>().text = "Player 1 Victory!";
        else if (player == Player.Player2 && playerType == PlayerType.Human) winnerPanel.GetComponentInChildren<Text>().text = "Player 2 Victory!";
        else winnerPanel.GetComponentInChildren<Text>().text = "Defeat!";

    }
}
