using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    //Variables
    private static GameManager instance;

    //Difficulty
    public static PlayerType typePlayer1;
    public static PlayerType typePlayer2;

    //Start
    public static FirstMove firstMove;

    //Multiplayer Config
    public static Symbol player1Symbol;
    public static Symbol player2Symbol;
    public static Symbol player1Color;
    public static Symbol player2Color;
}
