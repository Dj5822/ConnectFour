using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsertButton : MonoBehaviour
{
    public Image[] column;
    private GameController _gameController;
    private int _position;

    public void setGameControllerReference(GameController gameController, int position)
    {
        _gameController = gameController;
        _position = position;
    }

    // when the button is pressed, add a counter to the column.
    public void buttonPressed()
    {
        _gameController.addCounterToColumn(column, _position);
    }
}
