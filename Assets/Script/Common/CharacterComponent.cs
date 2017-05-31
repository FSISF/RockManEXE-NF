using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterComponent
{
    public GameObject ThisGameObject;
    public Transform ThisTransform;
    public Rigidbody2D ThisRigidbody2D;
    public Animator ThisAnimator;

    public float MoveSpeed;
    public Vector2 Direct;

    public void MoveTransform(Vector3 direct)
    {
        ThisTransform.position += direct * MoveSpeed * Time.deltaTime;
    }

    public void MoveRigidbody2D(Vector2 direct)
    {
        ThisRigidbody2D.position += direct * MoveSpeed * Time.deltaTime;
    }
}