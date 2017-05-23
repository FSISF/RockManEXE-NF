using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacterComponent
{
    public GameObject ThisGameObject;
    public Transform ThisTransform;
    public Rigidbody2D ThisRigidbody2D;
    public Animator ThisAnimator;

    public float MoveSpeed;
    public Vector2 Direct;

    public void MoveRigidbody2D(Vector2 direct)
    {
        ThisRigidbody2D.position += direct * MoveSpeed * Time.deltaTime;
        Direct = direct;

        if (direct == Vector2.left)
        {
            ThisTransform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ThisTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
