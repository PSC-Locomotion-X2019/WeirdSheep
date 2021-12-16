using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class DeathWallController : MonoBehaviour
{
    private float speed;
    public bool accelerate = true;
    public float acceleration= Mathf.Pow(10, -3); // Percentage more per frame
    public Vector3 initialPosition= new Vector3(-15f, -5f, -0.5f);

    public void Begin()
    {
        speed = GameManager.WALLSPEED;
        gameObject.transform.position = initialPosition;
    }
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
        speed *= (accelerate)? 1f + acceleration : 1f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rotules") || collision.CompareTag("Patte"))
        {
            if (collision.gameObject.GetComponentInParent<Creature>() != null)
            {
                collision.gameObject.GetComponentInParent<Creature>().kill();
            }
        }
        if (collision.CompareTag("Ground"))
        {
            Destroy(collision.gameObject);
        }
    }
}