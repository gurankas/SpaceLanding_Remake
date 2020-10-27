using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0;

    [SerializeField]
    private int _shipDirectionRange = 90;

    [SerializeField]
    private int _maxFuel = 1000;

    [SerializeField]
    private float _playerMaxMinScreenHeight = 4.6f;

    [SerializeField]
    private float _angleLandingAllowance = 5f;

    [SerializeField]
    private float _speedLandingAllowance1 = 10f;

    [SerializeField]
    private float _speedLandingAllowance2 = 20f;

    [SerializeField]
    private float _speedLandingAllowance3 = 30f;

    public int Fuel { get; protected set; }
    public int Time { get; protected set; }
    public int Score { get; protected set; }

    [HideInInspector]
    public int ScoreMultiplier = 0;

    private float _currentShipRotation = -90;
    private Vector2 _movement = Vector2.zero;
    private Rigidbody2D _rb;
    private SpriteRenderer _flameSprite;
    private Animator _anim;
    private bool _useFuel = false;

    public Action<bool> onDeathTriggered;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _flameSprite = GetComponentInChildren<Flame>().GetComponent<SpriteRenderer>();
        _flameSprite.gameObject.SetActive(false);
        Fuel = _maxFuel;

        Time = 0;
        InvokeRepeating("AddTime", 1, 1);

        //init player ship rotation to the right
        transform.rotation = Quaternion.Euler(0, 0, _currentShipRotation);

        //TODO apply inital force to make the ship
    }

    private void InitShip()
    {
        InvokeRepeating("AddTime", 1, 1);

    }

    private void SuccessfulLanding()
    {

    }

    private void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetAxisRaw("Vertical") > 0 && Fuel > 0)
        {
            _movement.y = Input.GetAxisRaw("Vertical");
            _useFuel = true;
            _flameSprite.gameObject.SetActive(true);
        }
        else
        {
            _flameSprite.gameObject.SetActive(false);
            _useFuel = false;
            _movement.y = 0;
        }

        //sets rotation of ship
        _currentShipRotation -= _movement.x;
        _currentShipRotation = Mathf.Clamp(_currentShipRotation, -_shipDirectionRange, _shipDirectionRange);
        transform.rotation = Quaternion.Euler(0, 0, _currentShipRotation);
        //transform.Rotate(new Vector3(0, 0, _currentShipRotation) * Time.deltaTime * _movement.x, Space.World);

        //applies vertical force according to direction of ship
        var force = DetermineForceBasedOnRotation();
        _rb.AddForce(new Vector2(force.x, force.y));

        print(_rb.velocity * 100);

        //setting variables in animators to display correct states of animations on the ship
        _anim.SetFloat("VerticalInput", _movement.y);
    }

    private void FixedUpdate()
    {
        if (_useFuel == true)
        {
            Fuel -= 1;
            Fuel = Mathf.Clamp(Fuel, 0, _maxFuel);
        }
    }

    private Vector3 DetermineForceBasedOnRotation()
    {
        return transform.up * _speed * _movement.y;
    }

    public Vector2 GetSpeed()
    {
        return _rb.velocity * 100;
    }

    public float GetAltitude()
    {
        return transform.position.y * 100;
    }

    private void AddTime()
    {
        Time += 1;
    }

    private void onDeath()
    {

        //reload scene
        SceneManager.LoadScene("GurankasScene");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.LogError($"Collided with terrain {other.relativeVelocity}");
        //Check angle, then velocity, then area check
        if (IsBetween((transform.rotation.z * 100), -_angleLandingAllowance, _angleLandingAllowance))
        {
            if (Mathf.Abs(other.relativeVelocity.y * 100) < _speedLandingAllowance1)
            {
                //perfect landing here
                Score += 1 * 100 * ScoreMultiplier;
                SuccessfulLanding();
                return;
            }
            else if (Mathf.Abs(other.relativeVelocity.y * 100) < _speedLandingAllowance2)
            {
                //worse landing here
                Score += (int)(.5 * 100 * ScoreMultiplier);
                SuccessfulLanding();
                return;
            }
            else if (Mathf.Abs(other.relativeVelocity.y * 100) < _speedLandingAllowance3)
            {
                //worst landing here
                Score += (int)(.25 * 100 * ScoreMultiplier);
                SuccessfulLanding();
                return;
            }
        }
        //destroy here
        _anim.SetTrigger("Dead");
    }

    private bool IsBetween(float testValue, float bound1, float bound2)
    {
        return (testValue >= Math.Min(bound1, bound2) && testValue <= Math.Max(bound1, bound2));
    }
}
