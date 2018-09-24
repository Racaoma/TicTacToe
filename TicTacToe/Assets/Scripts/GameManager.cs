using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    //Difficulty
    public static PlayerType typePlayer1;
    public static PlayerType typePlayer2;

    //Start
    public static Player firstMove;

    //Multiplayer Config
    public static Symbol player1Symbol;
    public static Symbol player2Symbol;
    public static Color player1Color;
    public static Color player2Color;

    //Reset
    public static void reset()
    {
        typePlayer1 = PlayerType.Human_Local;
        typePlayer2 = PlayerType.Human_Local;
        firstMove = Player.None;
        player1Symbol = Symbol.None;
        player2Symbol = Symbol.None;
        player1Color = Color.white;
        player2Color = Color.white;
    }
}
