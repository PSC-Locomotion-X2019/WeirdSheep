using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patte : MonoBehaviour
{
    public bool isFloored=false;
    Collider2D hitBox;
    private void Start()
    {
        hitBox = GetComponent<Collider2D>();
    }
    private void Update()
    {
        isFloored = hitBox.IsTouchingLayers(9);
    }
}
