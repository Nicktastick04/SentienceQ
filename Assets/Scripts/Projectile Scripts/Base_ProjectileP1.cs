﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_ProjectileP1 : MonoBehaviour
{
    public float ProjectileSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * ProjectileSpeed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player 2")
        {
            Destroy(gameObject);
        }
    }
}
