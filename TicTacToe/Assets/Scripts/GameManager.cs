using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
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
        firstMove = Player.None;
        player1Symbol = Symbol.None;
        player2Symbol = Symbol.None;
        player1Color = Color.white;
        player2Color = Color.white;
    }
}
