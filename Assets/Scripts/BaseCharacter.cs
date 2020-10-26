using System;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0;

    [SerializeField]
    private int _shipDirectionRange = 90;

    [SerializeField]
    private int _maxFuel = 1000;

    public int Fuel { get; protected set; }
    public int Time { get; protected set; }
    public int Score { get; protected set; }

    private float _currentShipRotation = -90;
    private Vector2 _movement = Vector2.zero;
    private Rigidbody2D _rb;
    private SpriteRenderer _flameSprite;
    private Animator _anim;
    private bool _useFuel = false;
    private bool _isDead = false;

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

        //print(_movement.y);

        //Debug.DrawRay(transform.position, transform.up * 1000, Color.red, 2f);

        //setting variables in animators to display correct states of animations on the ship
        _anim.SetFloat("VerticalInput", _movement.y);

        //test code
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _anim.SetTrigger("Dead");
            _isDead = true;
        }
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
        _isDead = true;
        onDeathTriggered.Invoke(_isDead);
    }
}
