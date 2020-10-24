using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0;

    [SerializeField]
    private int _shipDirectionRange = 90;

    private float _currentShipRotation = -90;

    private float _horizontalInput = 0;
    private float _verticalInput = 0;
    private Vector2 _movement = Vector2.zero;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Animator _anim;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();

        transform.rotation = Quaternion.Euler(0, 0, _currentShipRotation);
    }

    private void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            _movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
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

        //Debug.DrawRay(transform.position, transform.up * 1000, Color.red, 2f);
    }

    private Vector3 DetermineForceBasedOnRotation()
    {

        return transform.up * _speed * _movement.y;
    }
}
