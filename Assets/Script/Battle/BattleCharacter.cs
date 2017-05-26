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

    protected GameObject FollowTarget = null;

    void Start()
    {

    }

    public virtual void CharacterInjurd(int Damage)
    {
    }

    protected void SetFollowTarget()
    {
        FollowTarget = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void CheckGround()//Check is on ground
    {
        Vector2 LinecastEnd = new Vector2(CharacterTransform.position.x, CharacterTransform.position.y - 0.1f);
        LayerMask TargetGround = LayerMask.GetMask(LayerMask.LayerToName(CharacterGameObject.layer + 3));
        CharacterGrounded = Physics2D.Linecast(CharacterTransform.position, LinecastEnd, TargetGround);
    }
}
