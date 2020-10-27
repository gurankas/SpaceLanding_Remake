using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Asteroids : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed;
    private Rigidbody2D rb;


    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        //Invoke("DestroySelf", 10);


    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(-Speed, 0);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Main");

        }

    }
    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
