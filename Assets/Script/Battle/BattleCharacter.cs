using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacter : MonoBehaviour
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
    protected float CharacterSpeed = 0;

    [SerializeField]
    protected float CharacterJumpForce = 0;

    [SerializeField]
    protected bool CharacterGrounded = false;

    protected Vector3 CharacterDirect = Vector3.right;

    [SerializeField]
    protected BoxCollider2D CharacterBodyCehck = null;

    [SerializeField]
    protected SpriteRenderer CharacterPlayerSrpite = null;

    [SerializeField]
    protected HPManager HPManagerScript = null;

    void Start()
    {

    }

    public virtual void CharacterInjurd(int Damage)
    {
    }
}
