using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected GameObject CharacterGameObject = null;

    [SerializeField]
    protected Transform CharacterTransform = null;

    [SerializeField]
    protected Rigidbody2D CharacterRigidbody2D = null;

    [SerializeField]
    protected Animator CharacterAnimator = null;

    [SerializeField]
    protected float MoveSpeed;

    public Vector2 Direct;

    protected void CharacterMove(Vector2 direct)
    {
        CharacterRigidbody2D.position += direct * MoveSpeed * Time.deltaTime;
        Direct = direct;
    }
}