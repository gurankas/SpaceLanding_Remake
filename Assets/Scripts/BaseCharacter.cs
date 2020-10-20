using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0;

    private int _shipDirection = 0;         //0=right, 1=left, 2=up, 3=down
    private float _horizontalInput = 0;
    private float _verticalInput = 0;
    private Vector2 _movement = Vector2.zero;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Animator _anim;
    private bool _bFacingLeft;
    private bool _bFacingRight;
    private bool _bFacingDown;
    private bool _bFacingUp;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    void PrintShipDirection(int playerDirection)
    {
        switch (playerDirection)
        {
            case 0:
                {
                    print("right");
                    break;
                }
            case 1:
                {
                    print("left");
                    break;
                }
            case 2:
                {
                    print("up");
                    break;
                }
            case 3:
                {
                    print("down");
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void Update()
    {
        if (_movement.y == 0)
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
        }
        if (_movement.x == 0)
        {
            _movement.y = Input.GetAxisRaw("Vertical");
        }

        print(_rb.velocity);

        SetShipDirection();
        //PrintShipDirection(_shipDirection);
    }

    private void SetShipDirection()
    {
        //We set the velocity based on the input of the player
        //We set the y to rb.velocity.y, because if we set it to 0 our object does not move down with gravity
        _rb.velocity = new Vector2(_movement.x * _speed, _movement.y * _speed);

        //If moving left...
        if (_movement.x < 0 && CheckIfPlayerAlreadyMoving())
        {
            //Flip sprite
            _sr.flipX = true;
            _bFacingLeft = true;
            _shipDirection = 1; //0 = right, 1 = left, 2 = up, 3 = down
        }
        else if (_movement.x == 0)
        {
            _bFacingLeft = false;
        }

        //If moving right...
        if (_movement.x > 0 && CheckIfPlayerAlreadyMoving())
        {
            //Unflip sprite
            _sr.flipX = false;
            _bFacingRight = true;
            _shipDirection = 0; //0 = right, 1 = left, 2 = up, 3 = down
        }
        else if (_movement.x == 0)
        {
            _bFacingRight = false;
        }

        //If moving up...
        if (_movement.y > 0 && CheckIfPlayerAlreadyMoving())
        {
            _bFacingUp = true;
            _shipDirection = 2; //0 = right, 1 = left, 2 = up, 3 = down
        }
        else if (_movement.y == 0)
        {
            _bFacingUp = false;
        }

        //If moving down...
        if (_movement.y < 0 && CheckIfPlayerAlreadyMoving())
        {
            _bFacingDown = true;
            _shipDirection = 3; //0 = right, 1 = left, 2 = up, 3 = down
        }
        else if (_movement.y == 0)
        {
            _bFacingDown = false;
        }

        //We send this information to the animator, which handles the transition between animations
        _anim.SetFloat("PlayerDirection", _shipDirection);
    }

    private bool CheckIfPlayerAlreadyMoving()
    {
        return !(_bFacingLeft || _bFacingRight || _bFacingDown || _bFacingUp);
    }
}
