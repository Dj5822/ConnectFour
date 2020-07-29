using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Color _playerTurn;
    private char[,] _board;
    private const char RED = 'r';
    private const char YELLOW = 'y';

    public InsertButton[] insertButtons;
    public GameObject gameOverPanel;
    public Text gameOverText;

    // Called at the beginning.
    private void Awake()
    {
        // start with red player.
        _playerTurn = Color.red;
        gameOverPanel.SetActive(false);
        gameControllerReferenceOnInsertButtons();
        setUpBoard();
    }

    private void gameControllerReferenceOnInsertButtons()
    {
        for (int i=0; i<insertButtons.Length; i++)
        {
            insertButtons[i].GetComponentInParent<InsertButton>().setGameControllerReference(this, i);
        }
    }

    private void setUpBoard()
    {
        _board = new char[insertButtons[0].GetComponentInParent<InsertButton>().column.Length, insertButtons.Length];
    }

    public void addCounterToColumn(Image[] column, int position)
    {
        // add a counter to the selected column.
        for (int row=0; row<column.Length; row++)
        {
            Color slotColor = column[row].color;
            
            // adds a counter to the first empty slot.
            if (slotColor == Color.white)
            {
                // color of counter corresponds to the player turn.
                if (_playerTurn == Color.yellow) _board[row, position] = YELLOW;
                else _board[row, position] = RED;
                column[row].color = _playerTurn;

                // check if new counter resulted in gameover.
                if (gameOver(row, position) == true) {
                    if (_playerTurn == Color.red)
                    {
                        Debug.Log("Winner is red!");
                        gameOverText.text = "Winner is red!";
                    }
                    else if (_playerTurn == Color.yellow)
                    {
                        Debug.Log("Winner is yellow!");
                        gameOverText.text = "Winner is yellow!";
                    }

                    foreach (InsertButton button in insertButtons){
                        button.gameObject.SetActive(false);
                    }

                    gameOverPanel.SetActive(true);
                }

                // switch turns.
                turnEnd();
                break;
            }
        }
    }

    public void turnEnd()
    {
        if (_playerTurn == Color.yellow) _playerTurn = Color.red;
        else _playerTurn = Color.yellow;
    }

    public bool gameOver(int row, int col)
    {
        int count = 1;
        char turn;
        if (_playerTurn == Color.yellow) turn = YELLOW;
        else turn = RED;

        // check vertical
        for (int i=row+1; i<_board.GetLength(0); i++)
        {
            if (_board[i, col] == turn) count++;
            else break;
        }

        for (int i = row-1; i >= 0; i--)
        {
            if (_board[i, col] == turn) count++;
            else break;
        }

        Debug.Log("Column count: " + count);
        if (count >= 4) return true;
        else count = 1;

        // check horizontal
        for (int i = col + 1; i < _board.GetLength(1); i++)
        {
            if (_board[row, i] == turn) count++;
            else break;
        }

        for (int i = col - 1; i >= 0; i--)
        {
            if (_board[row, i] == turn) count++;
            else break;
        }

        Debug.Log("Row count: " + count);
        if (count >= 4) return true;
        else count = 1;

        // check diagonal (bot left to bot right)
        for (int i = 1; row + i < _board.GetLength(0) && col + i < _board.GetLength(1); i++)
        {
            if (_board[row + i, col + i] == turn) count++;
            else break;
        }

        for (int i = 1; row - i >= 0 && col - i >= 0; i++)
        {
            if (_board[row - i, col - i] == turn) count++;
            else break;
        }

        Debug.Log("bot left top right diag count: " + count);
        if (count >= 4) return true;
        else count = 1;

        // check digaonal (bot right to bot left)
        for (int i = 1; row - i >= 0 && col + i < _board.GetLength(1); i++)
        {
            if (_board[row - i, col + i] == turn) count++;
            else break;
        }

        for (int i = 1; row + i < _board.GetLength(0) && col - i >= 0; i++)
        {
            if (_board[row + i, col - i] == turn) count++;
            else break;
        }

        Debug.Log("bot right top left diag count: " + count);
        if (count >= 4) return true;
        else return false;
    }
}
