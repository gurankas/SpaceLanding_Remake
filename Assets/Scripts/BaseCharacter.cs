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

    [SerializeField]
    private AudioClip _rocketLaunch;

    [SerializeField]
    private AudioClip _deathAudio;

    public int Fuel { get; protected set; }
    public int Time { get; protected set; }
    public int Score { get; protected set; }

    [HideInInspector]
    public int ScoreMultiplier = 0;

    [HideInInspector]
    public bool FuelEmpty = false;

    private float _currentShipRotation = -90;
    private Vector2 _movement = Vector2.zero;
    private Rigidbody2D _rb;
    private SpriteRenderer _flameSprite;
    private Animator _anim;
    private bool _useFuel = false;
    private Vector3 _startPos;
    private AudioSource _as;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _flameSprite = GetComponentInChildren<Flame>().GetComponent<SpriteRenderer>();
        _as = GetComponent<AudioSource>();
        _flameSprite.gameObject.SetActive(false);
        Fuel = _maxFuel;
        _startPos = transform.position;

        Time = 0;
        InvokeRepeating("AddTime", 1, 1);

        //init player ship rotation to the right
        transform.rotation = Quaternion.Euler(0, 0, _currentShipRotation);

        //TODO apply inital force to make the ship
        _rb.AddForce(new Vector2(10, 0));
    }

    private void SuccessfulLanding()
    {
        transform.position = _startPos;
        _rb.velocity = Vector2.zero;
        //init player ship rotation to the right
        transform.rotation = Quaternion.Euler(0, 0, _currentShipRotation);

        //TODO apply inital force to make the ship
        _rb.AddForce(new Vector2(10, 0));
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
            _as.Stop();
        }

        //sets rotation of ship
        _currentShipRotation -= _movement.x;
        _currentShipRotation = Mathf.Clamp(_currentShipRotation, -_shipDirectionRange, _shipDirectionRange);
        transform.rotation = Quaternion.Euler(0, 0, _currentShipRotation);

        //applies vertical force according to direction of ship
        var force = DetermineForceBasedOnRotation();
        _rb.AddForce(new Vector2(force.x, force.y));

        if (Fuel == 0)
        {
            FuelEmpty = true;
        }

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
        SceneManager.LoadScene("MainMenu");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.LogError($"Collided with terrain {other.relativeVelocity}");
        //Check angle, then velocity, then area check
        if (IsBetween((transform.rotation.z * 100), -_angleLandingAllowance, _angleLandingAllowance))
        {
            if (Mathf.Abs(other.relativeVelocity.y * 100) < _speedLandingAllowance1)
            {
                //perfect landing here
                Score += 1 * 100 * ScoreMultiplier;
                Invoke("SuccessfulLanding", 1);
                return;
            }
            else if (Mathf.Abs(other.relativeVelocity.y * 100) < _speedLandingAllowance2)
            {
                //worse landing here
                Score += (int)(.5 * 100 * ScoreMultiplier);
                Invoke("SuccessfulLanding", 1);
                return;
            }
            else if (Mathf.Abs(other.relativeVelocity.y * 100) < _speedLandingAllowance3)
            {
                //worst landing here
                Score += (int)(.25 * 100 * ScoreMultiplier);
                Invoke("SuccessfulLanding", 1);
                return;
            }
        }
        //destroy here
        _anim.SetTrigger("Dead");
        _as.PlayOneShot(_deathAudio);
    }

    private bool IsBetween(float testValue, float bound1, float bound2)
    {
        return (testValue >= Math.Min(bound1, bound2) && testValue <= Math.Max(bound1, bound2));
    }

    private void PlayRocketAudio()
    {
        _as.PlayOneShot(_rocketLaunch);
    }
}
