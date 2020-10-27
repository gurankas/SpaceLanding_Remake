using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{

    [SerializeField]
    private Text _score;
    [SerializeField]
    private Text _time;
    [SerializeField]
    private Text _fuel;
    [SerializeField]
    private Text _altitude;
    [SerializeField]
    private Text _horizontalSpeed;
    [SerializeField]
    private Text _verticalSpeed;
    [SerializeField]
    private Text _startText;
    [SerializeField]
    private Text _noFuelText;

    [SerializeField]
    private BaseCharacter _ship;

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "GurankasScene")
        {
            _startText.enabled = false;
        }
        _noFuelText.enabled = false;
    }

    private void FixedUpdate()
    {
        if (_ship != null)
        {
            _score.text = "" + _ship.Score;
            _time.text = "" + _ship.Time;
            _fuel.text = "" + _ship.Fuel;
            _altitude.text = "" + Mathf.Ceil(_ship.GetAltitude());
            _horizontalSpeed.text = "" + Mathf.Ceil(_ship.GetSpeed().x);
            _verticalSpeed.text = "" + Mathf.Ceil(_ship.GetSpeed().y);

            if (_ship.FuelEmpty == true)
            {
                _noFuelText.enabled = true;
            }
            else
            {
                _noFuelText.enabled = false;
            }
        }
    }

}
