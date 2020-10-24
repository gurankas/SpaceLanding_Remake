using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0;

    [SerializeField]
    private int _shipDirectionRange = 90;

    [SerializeField]
    private int _maxFuel = 1000;

    private int _fuel = 1000;
    private float _currentShipRotation = -90;
    private float _horizontalInput = 0;
    private float _verticalInput = 0;
    private Vector2 _movement = Vector2.zero;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Animator _anim;
    private bool _useFuel = false;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _fuel = _maxFuel;

        //init player ship rotation to the right
        transform.rotation = Quaternion.Euler(0, 0, _currentShipRotation);

        //TODO apply inital force to make the ship
    }

    private void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetAxisRaw("Vertical") > 0 && _fuel > 0)
        {
            _movement.y = Input.GetAxisRaw("Vertical");
            _useFuel = true;
        }
        else
        {
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

        print(_fuel);

        //Debug.DrawRay(transform.position, transform.up * 1000, Color.red, 2f);
    }

    private void FixedUpdate()
    {
        if (_useFuel == true)
        {
            _fuel -= 1;
            _fuel = Mathf.Clamp(_fuel, 0, _maxFuel);
        }
    }

    private Vector3 DetermineForceBasedOnRotation()
    {
        return transform.up * _speed * _movement.y;
    }
}
