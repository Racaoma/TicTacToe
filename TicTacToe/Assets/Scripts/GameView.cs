using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameView : MonoBehaviour
{
    //Board Reference
    public GameObject board;

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

    //Panels
    public GameObject winnerPanel;
    public GameObject standbyPanel;
    public GameObject p1Panel;
    public GameObject p2Panel;

    //Highlight Decay
    private float highlightDecay = 1.75f;

    private static GameView instance;
    public static GameView Instance
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

    //Start Method
    void Start() 
	{
        //Setup
        resetBoard();
        setSymbols();

        //Enable Panels
        standbyPanel.SetActive(false);
        p1Panel.SetActive(true);
        p2Panel.SetActive(true);
    }

	//Set Random Colors for Symbols
	public void setSymbols()
	{
        //Check Which Player You Are (Player 1)
        if (ClientNetworking.getLocalClientNetworking().localPlayer == Player.Player1)
        {
            //Color Red
            if (ClientNetworking.getLocalClientNetworking().localColor == Color.red)
            {
                //Update Random Color & Granny's Sprites
                player1Granny.sprite = redGranny;
                player1Symbol.color = Color.red;
                player2Granny.sprite = blueGranny;
                player2Symbol.color = Color.blue;

                //Update Symbols
                if (ClientNetworking.getLocalClientNetworking().localSymbol == Symbol.Circle)
                {
                    player1Symbol.sprite = circle;
                    player2Symbol.sprite = cross;
                    colorCircle = Color.red;
                    colorCross = Color.blue;
                }
                else
                {
                    player1Symbol.sprite = cross;
                    player2Symbol.sprite = circle;
                    colorCircle = Color.blue;
                    colorCross = Color.red;
                }
            }
            //Color Blue
            else
            {
                //Update Random Color & Granny's Sprites
                player1Granny.sprite = blueGranny;
                player1Symbol.color = Color.blue;
                player2Granny.sprite = redGranny;
                player2Symbol.color = Color.red;

                //Update Symbols
                if (ClientNetworking.getLocalClientNetworking().localSymbol == Symbol.Circle)
                {
                    player1Symbol.sprite = circle;
                    player2Symbol.sprite = cross;
                    colorCircle = Color.blue;
                    colorCross = Color.red;
                }
                else
                {
                    player1Symbol.sprite = cross;
                    player2Symbol.sprite = circle;
                    colorCircle = Color.red;
                    colorCross = Color.blue;
                }
            }
        }
        //Check Which Player You Are (Player 2)
        else
        {
            //Color Red
            if (ClientNetworking.getLocalClientNetworking().localColor == Color.red)
            {
                //Update Random Color & Granny's Sprites
                player1Granny.sprite = blueGranny;
                player1Symbol.color = Color.blue;
                player2Granny.sprite = redGranny;
                player2Symbol.color = Color.red;

                //Update Symbols
                if (ClientNetworking.getLocalClientNetworking().localSymbol == Symbol.Circle)
                {
                    player1Symbol.sprite = cross;
                    player2Symbol.sprite = circle;
                    colorCircle = Color.red;
                    colorCross = Color.blue;
                }
                else
                {
                    player1Symbol.sprite = cross;
                    player2Symbol.sprite = circle;
                    colorCircle = Color.blue;
                    colorCross = Color.red;
                }
            }
            //Color Blue
            else
            {
                //Update Random Color & Granny's Sprites
                player1Granny.sprite = redGranny;
                player1Symbol.color = Color.red;
                player2Granny.sprite = blueGranny;
                player2Symbol.color = Color.blue;

                //Update Symbols
                if (ClientNetworking.getLocalClientNetworking().localSymbol == Symbol.Circle)
                {
                    player1Symbol.sprite = cross;
                    player2Symbol.sprite = circle;
                    colorCircle = Color.blue;
                    colorCross = Color.red;
                }
                else
                {
                    player1Symbol.sprite = cross;
                    player2Symbol.sprite = circle;
                    colorCircle = Color.red;
                    colorCross = Color.blue;
                }
            }
        }
    }

	//Reset Entire Board 
	public void resetBoard()
	{
		for (int i = 0; i < board.transform.childCount; i++)
		{
            Transform cell = board.transform.GetChild(i);
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
            board.transform.GetChild(cellNumber).GetComponent<SpriteRenderer>().sprite = circle;
            board.transform.GetChild (cellNumber).GetComponent<SpriteRenderer>().color = colorCircle;
		}
		else
		{
            board.transform.GetChild(cellNumber).GetComponent<SpriteRenderer>().sprite = cross;
            board.transform.GetChild (cellNumber).GetComponent<SpriteRenderer>().color = colorCross;
		} 
	}

    //Outline Winning Cell
    public void outlineCell(int cellNumber)
    {
        //Create Object
        GameObject highlight = new GameObject();
        Transform parent = board.transform.GetChild(cellNumber);
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
		if(symbol == Symbol.Circle) board.transform.GetChild(cellNumber).GetComponent<SpriteRenderer>().sprite = circle;
		else board.transform.GetChild(cellNumber).GetComponent<SpriteRenderer>().sprite = cross;

        //Make it Transparent
        board.transform.GetChild (cellNumber).GetComponent<SpriteRenderer> ().color = new Color (0f,0f,0f,0.3f);
	}

	//Destroy Ghost
	public void destroyGhost(int cellNumber)
	{
        board.transform.GetChild(cellNumber).GetComponent<SpriteRenderer>().sprite = null;
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
    public void displayWinner(VictoryType victoryType, bool winner)
	{
        //Outline Cells
        if (victoryType == VictoryType.Line1)
        {
            outlineCell(0);
            outlineCell(1);
            outlineCell(2);
        }
        else if(victoryType == VictoryType.Line2)
        {
            outlineCell(3);
            outlineCell(4);
            outlineCell(5);
        }
        else if (victoryType == VictoryType.Line3)
        {
            outlineCell(6);
            outlineCell(7);
            outlineCell(8);
        }
        else if (victoryType == VictoryType.Column1)
        {
            outlineCell(0);
            outlineCell(3);
            outlineCell(6);
        }
        else if (victoryType == VictoryType.Column2)
        {
            outlineCell(1);
            outlineCell(4);
            outlineCell(7);
        }
        else if (victoryType == VictoryType.Column3)
        {
            outlineCell(2);
            outlineCell(5);
            outlineCell(8);
        }
        else if (victoryType == VictoryType.MainDiagonal)
        {
            outlineCell(0);
            outlineCell(4);
            outlineCell(8);
        }
        else if (victoryType == VictoryType.SecondaryDiagonal)
        {
            outlineCell(2);
            outlineCell(4);
            outlineCell(6);
        }

        //Display Correct Message
        if (victoryType == VictoryType.Draw) winnerPanel.GetComponentInChildren<Text>().text = "Draw";
        else if (winner) winnerPanel.GetComponentInChildren<Text>().text = "Victory!";
        else winnerPanel.GetComponentInChildren<Text>().text = "Defeat!";

        //Enable Panel
        winnerPanel.SetActive(true);
    }
}
