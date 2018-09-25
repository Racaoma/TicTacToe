using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Player
{
    None,
    Player1,
    Player2
};

public enum PlayerType
{
    Human_Local,
    Human_Network,
    AI_Easy,
    AI_Medium,
    AI_Hard,
};

public struct Tuple
{
    public int row;
    public int column;
};

public class GameLogic : MonoBehaviour
{
    //Script & Control Variables
    private GameState gameState;
    private GameView gameView;
    private AudioControl audioControl;

    //Multiplayer
    private ClientNetworking client;

    //Delay for AI
    private float nextPlay;
    public float delayAI = 0.8f;

    //Current Player
    private Player currentTurn;

    //Randomized Symbol for Each Player
    private Symbol symbolPlayer1;
    private Symbol symbolPlayer2;

    //First Move
    private Player firstMove;

    //Human or AI Player
    private PlayerType typePlayer1;
    private PlayerType typePlayer2;

    // Use this for initialization
    void Start()
    {
        //Load Options
        typePlayer1 = GameManager.typePlayer1;
        typePlayer2 = GameManager.typePlayer2;
        firstMove = GameManager.firstMove;

        //Load Scripts
        gameState = GameState.Instance;
        gameView = this.gameObject.GetComponent<GameView>();
        audioControl = FindObjectOfType<AudioControl>();

        //Get Network Client
        client = getClientNetworking();

        //Set First Move
        currentTurn = Player.None;
        updateTurn();
    }

    //Get ClientNetworking
    public ClientNetworking getClientNetworking()
    {
        ClientNetworking[] list = FindObjectsOfType<ClientNetworking>();
        for(int i = 0; i < list.Length; i++)
        {
            if (list[i].hasAuthority) return list[i];
        }
        return null;
    }

    //Get Symbol by Player
    public Symbol getSymbolByPlayer(Player playerTurn)
    {
        if (playerTurn == Player.Player1) return symbolPlayer1;
        else if (playerTurn == Player.Player2) return symbolPlayer2;
        else return Symbol.None;
    }

    //Get Player by Symbol
    public Player getPlayerBySymbol(Symbol symbol)
    {
        if (symbolPlayer1 == symbol) return Player.Player1;
        else if (symbolPlayer2 == symbol) return Player.Player2;
        else return Player.None;
    }

    //Get Type of Player
    public PlayerType getTypeByPlayer(Player playerTurn)
    {
        if (playerTurn == Player.Player1) return typePlayer1;
        else return typePlayer2;
    }

    //Reset Board
    public void resetBoard()
    {
        gameState.resetBoard();
        gameView.resetBoard();

        //Update Player
        updateTurn();
    }

    //Create Ghost Sprite
    public void createGhost(int cellNumber)
    {
        //Check if it's the player's turn
        if (currentTurn == client.getPlayer())
        {
            //Check if Cell is Empty & Create Ghost
            if (gameState.getBoardCell(cellNumber / 3, cellNumber % 3) == Symbol.None) gameView.createGhost(cellNumber, getSymbolByPlayer(currentTurn));
        }
    }

    //Destroy Ghost Sprite
    public void destroyGhost(int cellNumber)
    {
        //Check if Cell is Empty & Destroy Ghost
        if (gameState.getBoardCell(cellNumber / 3, cellNumber % 3) == Symbol.None) gameView.destroyGhost(cellNumber);
    }

    //Update Player
    private void updateTurn()
    {
        //Update Player
        if (currentTurn == Player.Player1) currentTurn = Player.Player2;
        else if (currentTurn == Player.Player2) currentTurn = Player.Player1;
        else
        {
            //First Move!
            if (firstMove == Player.Player1) currentTurn = Player.Player1;
            else if (firstMove == Player.Player2) currentTurn = Player.Player2;
            else currentTurn = Random.value >= 0.5 ? Player.Player1 : Player.Player2;
        }

        //Highlight Player
        gameView.highlightPlayer(currentTurn);

        //Play Sound
        audioControl.playSound();

        //Check if AI and Schedule Next Play
        nextPlay = delayAI;
    }

    //Make Play Method (Check Viability and Update Everything)
    public void makePlay(int cellNumber)
    {
        //CellNumber to Row & Column Conversion
        int row = cellNumber / 3;
        int column = cellNumber % 3;

        //Check if Cell is Empty
        if (gameState.getBoardCell(row, column) == Symbol.None)
        {
            //Make Play!
            gameState.updateCell(row, column, getSymbolByPlayer(currentTurn));
            gameView.destroyGhost(cellNumber); //If Any
            gameView.updateCell(cellNumber, getSymbolByPlayer(currentTurn));

            //Check End Game
            Symbol[,] board = gameState.getBoard();
            Symbol winner = checkEndGame(board, false);
            if (winner != Symbol.None)
            {
                Player player = getPlayerBySymbol(winner);
                gameView.displayWinner(player, getTypeByPlayer(player));
                currentTurn = Player.None;
            }
            else
            {
                //Update Player
                updateTurn();
            }
        }
    }

    //Make Play Human
    public void makePlayHuman(int cellNumber)
    {
        if (currentTurn == client.getPlayer())
        {
            if(client == null) client = getClientNetworking();
            client.CmdSendPlay(cellNumber);
        }
    }

    //Get Play AI
    public void getPlayAI()
    {
        //AI Easy
        if (getTypeByPlayer(currentTurn) == PlayerType.AI_Easy)
        {
            //Pick A Random Empty Cell and Play
            List<Tuple> emptyCells = emptyCellsToList(gameState.getBoard());
            Tuple t = emptyCells[Random.Range(0, emptyCells.Count)];
            makePlay((t.row * 3) + (t.column));
        }
        else if (getTypeByPlayer(currentTurn) == PlayerType.AI_Medium)
        {
            if(Random.value > 0.5)
            {
                //Parameter Variables
                Symbol[,] board = gameState.getBoard();
                Tuple move;
                move.row = -1;
                move.column = -1;

                //Run MiniMax!
                miniMax(10, ref board, true, int.MinValue, int.MaxValue, ref move);
                makePlay((move.row * 3) + (move.column));
            }
            else
            {
                //Pick A Random Empty Cell and Play
                List<Tuple> emptyCells = emptyCellsToList(gameState.getBoard());
                Tuple t = emptyCells[Random.Range(0, emptyCells.Count)];
                makePlay((t.row * 3) + (t.column));
            } 
        }
        else if (getTypeByPlayer(currentTurn) == PlayerType.AI_Hard)
        {
            //Parameter Variables
            Symbol[,] board = gameState.getBoard();
            Tuple move;
            move.row = -1;
            move.column = -1;

            //Run MiniMax!
            miniMax(10, ref board, true, int.MinValue, int.MaxValue, ref move);
            makePlay((move.row * 3) + (move.column));
        }
    }

    //Check End Game
    private Symbol checkEndGame(Symbol[,] board, bool minimaxCheck)
    {
        //Draw Test Variable
        bool draw = true;

        for (int i = 0; i < 3; i++)
        {
            //Check Rows
            Symbol cell1 = board[i, 0];
            Symbol cell2 = board[i, 1];
            Symbol cell3 = board[i, 2];

            if (cell1 == Symbol.None) draw = false;
            else
            {
                if (cell2 == Symbol.None) draw = false;
                else
                {
                    if (cell3 == Symbol.None) draw = false;
                    else
                    {
                        if (cell1 == cell2 && cell2 == cell3)
                        {
                            if (!minimaxCheck)
                            {
                                gameView.outlineCell(i * 3 + 0);
                                gameView.outlineCell(i * 3 + 1);
                                gameView.outlineCell(i * 3 + 2);
                            }
                            return cell1;
                        }
                    }
                }
            }

            //Check Columns
            cell1 = board[0, i];
            cell2 = board[1, i];
            cell3 = board[2, i];

            if (cell1 == Symbol.None) draw = false;
            else
            {
                if (cell2 == Symbol.None) draw = false;
                else
                {
                    if (cell3 == Symbol.None) draw = false;
                    else
                    {
                        if (cell1 == cell2 && cell2 == cell3)
                        {
                            if (!minimaxCheck)
                            {
                                gameView.outlineCell(i);
                                gameView.outlineCell(3 + i);
                                gameView.outlineCell(6 + i);
                            }
                            return cell1;
                        }
                    }
                }
            }
        }

        //Check Main Diagonal
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            if (board[1, 1] != Symbol.None)
            {
                if (!minimaxCheck)
                {
                    gameView.outlineCell(0);
                    gameView.outlineCell(4);
                    gameView.outlineCell(8);
                }
                return board[1, 1];
            }
        }

        //Check Other Diagonal
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
        {
            if (board[1, 1] != Symbol.None)
            {
                if (!minimaxCheck)
                {
                    gameView.outlineCell(2);
                    gameView.outlineCell(4);
                    gameView.outlineCell(6);
                }
                return board[1, 1];
            }
        }

        //Check for Draw or Continue Game
        if (draw) return Symbol.Draw;
        else return Symbol.None;
    }

    //Get Empty Cells in List
    public List<Tuple> emptyCellsToList(Symbol[,] board)
    {
        List<Tuple> list = new List<Tuple>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == Symbol.None)
                {
                    Tuple t;
                    t.row = i;
                    t.column = j;
                    list.Add(t);
                }
            }
        }

        return list;
    }

    //Shuffle List
    public void shuffleList(ref List<Tuple> list)
    {
        //Fisher–Yates Shuffle
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Tuple value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    //MiniMax
    private int miniMax(int depth, ref Symbol[,] board, bool stepMax, int alpha, int beta, ref Tuple move)
    {
        //Break Condition -> Check Winner
        Symbol winner = checkEndGame(board, true);
        if (winner != Symbol.None)
        {
            if (winner == Symbol.Draw) return 0;
            else if (getPlayerBySymbol(winner) == currentTurn) return depth;
            else return depth * (-1);
        }

        //Not Terminal Node -> Get Possible Moves
        List<Tuple> emptyPositions = emptyCellsToList(board);
        shuffleList(ref emptyPositions);

        //Step Max
        if (stepMax)
        {
            int max = int.MinValue;
            for (int index = 0; index < emptyPositions.Count; index++)
            {
                //Get Tuple
                Tuple t = emptyPositions[index];

                //Temporarily Alter Board
                board[t.row, t.column] = getSymbolByPlayer(currentTurn);

                //Recursion
                int score = miniMax(depth-1, ref board, false, alpha, beta, ref move);

                //Update Max
                if (score > max)
                {
                    //Update Current Max
                    max = score;

                    //Update Final Movement
                    if(depth == 10)
                    {
                        move.row = t.row;
                        move.column = t.column;
                    }
                }

                //Return Board to Original State
                board[t.row, t.column] = Symbol.None;

                //Alpha-Beta Pruning
                alpha = Mathf.Max(alpha, score);
                if (beta <= alpha) break;
            }

            //Finally...
            return max;
        }
        //Step Min
        else
        {
            int min = int.MaxValue;
            for(int index = 0; index < emptyPositions.Count; index++)
            {
                //Get Tuple
                Tuple t = emptyPositions[index];

                //Define Enemy Player
                Player otherPlayer;
                if (currentTurn == Player.Player1) otherPlayer = Player.Player2;
                else otherPlayer = Player.Player1;

                //Temporarily Alter Board
                board[t.row, t.column] = getSymbolByPlayer(otherPlayer);

                //Recursion
                int score = miniMax(depth - 1, ref board, true, alpha, beta, ref move);

                //Update Max
                if (score < min) min = score;

                //Return Board to Original State
                board[t.row, t.column] = Symbol.None;

                //Alpha-Beta Pruning
                beta = Mathf.Min(beta, score);
                if (beta <= alpha) break;
            }

            //Finally...
            return min;
        }
    }

    //Unity Update Method
    public void LateUpdate()
    {
        if (currentTurn != Player.None && getTypeByPlayer(currentTurn) != PlayerType.Human_Local)
        {
            nextPlay -= Time.deltaTime;
            if(nextPlay < 0f) getPlayAI();
        }
    }
}