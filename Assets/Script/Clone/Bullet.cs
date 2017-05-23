﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D BulletRigidbody = null;

    [SerializeField]
    private float Speed = 0;

    private Vector2 direct;
    public Vector2 Direct
    {
        set
        {
            direct = value;
        }
    }

    public int Damage = 0;

    void Start()
    {
    }

    private void FixedUpdate()
    {
        BulletRigidbody.position += direct * Speed * Time.deltaTime;
    }

    private Enemy.EnemyState EnemyStateScript;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == this.gameObject.layer || collision.gameObject.tag.Contains("Wall"))
        {
            EnemyStateScript = collision.transform.parent.GetComponent<Enemy.EnemyState>();
            if (EnemyStateScript != null)
            {
                EnemyStateScript.EnemyInjurd(Damage);
            }

            Destroy(this.gameObject);
        }
    }
}
