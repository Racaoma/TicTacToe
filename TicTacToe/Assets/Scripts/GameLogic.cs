using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Player
{
    None,
    Player1,
    Player2
};

public enum VictoryType
{
    None,
    Draw,
    Line1,
    Line2,
    Line3,
    Column1,
    Column2,
    Column3,
    MainDiagonal,
    SecondaryDiagonal
}

public class GameLogic : MonoBehaviour
{
    //Make Play Method (Check Viability and Update Everything)
    public bool makePlay(int cellNumber, Symbol symbol)
    {
        //CellNumber to Row & Column Conversion
        int row = cellNumber / 3;
        int column = cellNumber % 3;

        //Check if Cell is Empty
        if (GameState.Instance.getBoardCell(row, column) == Symbol.None)
        {
            //Make Play!
            GameState.Instance.updateCell(row, column, symbol);
            return true;
        }

        //Default
        return false;
    }

    //Check End Game
    public VictoryType checkEndGame()
    {
        //Check Variables
        Symbol cell1;
        Symbol cell2;
        Symbol cell3;

        //Draw Test Variable
        bool draw = true;

        for (int i = 0; i < 3; i++)
        {
            //Check Rows
            cell1 = GameState.Instance.getBoardCell(i, 0);
            cell2 = GameState.Instance.getBoardCell(i, 1);
            cell3 = GameState.Instance.getBoardCell(i, 2);

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
                            if (i == 0) return VictoryType.Line1;
                            else if (i == 1) return VictoryType.Line2;
                            else if (i == 2) return VictoryType.Line3;
                        }
                    }
                }
            }

            //Check Columns
            cell1 = GameState.Instance.getBoardCell(0, i);
            cell2 = GameState.Instance.getBoardCell(1, i);
            cell3 = GameState.Instance.getBoardCell(2, i);

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
                            if (i == 0) return VictoryType.Column1;
                            else if (i == 1) return VictoryType.Column2;
                            else if (i == 2) return VictoryType.Column3;
                        }
                    }
                }
            }
        }

        //Check Main Diagonal
        cell1 = GameState.Instance.getBoardCell(0, 0);
        cell2 = GameState.Instance.getBoardCell(1, 1);
        cell3 = GameState.Instance.getBoardCell(2, 2);

        if (cell1 == cell2 && cell2 == cell3)
        {
            if (cell1 != Symbol.None)
            {
                return VictoryType.MainDiagonal;
            }
        }

        //Check Other Diagonal
        cell1 = GameState.Instance.getBoardCell(0, 2);
        cell2 = GameState.Instance.getBoardCell(1, 1);
        cell3 = GameState.Instance.getBoardCell(2, 0);

        if (cell1 == cell2 && cell2 == cell3)
        {
            if (cell1 != Symbol.None)
            {
                return VictoryType.SecondaryDiagonal;
            }
        }

        //Check for Draw or Continue Game
        if (draw) return VictoryType.Draw;
        else return VictoryType.None;
    }
}