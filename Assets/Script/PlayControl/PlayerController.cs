using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllPlayerState;

public class PlayerController : SingletonMono<PlayerController>
{
    [SerializeField]
    private GameObject PlayerGameObject = null;

    [SerializeField]
    private Transform PlayerTransform = null;

    [SerializeField]
    private Rigidbody2D PlayerRigidbody2D = null;

    [SerializeField]
    private Animator PlayerAnimator = null;

    [SerializeField]
    private float Speed = 0;

    [SerializeField]
    private float JumpForce = 0;

    [SerializeField]
    private bool Grounded = false;

    private Vector3 Direct = Vector3.right;

    private PlayerStateContext PlayerStateContextScript;
    private StateComponent PlayerComponent = new StateComponent();

    [SerializeField]
    private HPManager HPManagerScript = null;
    [SerializeField]
    private BoxCollider2D PlayerBody = null;
    [SerializeField]
    private SpriteRenderer PlayerSrpite = null;

    private void Awake()
    {
        SetPlayerComponent();
        PlayerStateContextScript = new PlayerStateContext();
        PlayerStateContextScript.AwakePlayerState(PlayerComponent);
    }

    private void SetPlayerComponent()//Set player component to use in state machine
    {
        PlayerComponent.ThisGameObject = PlayerGameObject;
        PlayerComponent.ThisTransform = PlayerTransform;
        PlayerComponent.ThisRigidbody2D = PlayerRigidbody2D;
        PlayerComponent.ThisAnimator = PlayerAnimator;

        PlayerComponent.MoveSpeed = Speed;
        PlayerComponent.JumpForce = JumpForce;
        PlayerComponent.Direct = Direct;
    }

    void Start()
    {
    }

    private void FixedUpdate()
    {
        PlayerStateContextScript.StateWork();
        CheckGround();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Ground"))
        {
            if (PlayerStateContextScript.NowState == PlayerNowState.Jump || PlayerStateContextScript.NowState == PlayerNowState.JumpAttack)
            {
                PlayerStateContextScript.SetPlayerState(new PlayerIdle(PlayerStateContextScript, PlayerComponent));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Enemy") && collision.gameObject.layer == PlayerGameObject.layer)
        {
            PlayerInjurd(20);
        }
    }

    private void CheckGround()//Check player is on ground
    {
        Vector2 LinecastEnd = new Vector2(PlayerTransform.position.x, PlayerTransform.position.y - 1.5f);
        LayerMask TargetGround = LayerMask.GetMask(LayerMask.LayerToName(PlayerGameObject.layer + 3));
        Grounded = Physics2D.Linecast(PlayerTransform.position, LinecastEnd, TargetGround);
        PlayerComponent.Grounded = Grounded;
    }

    public void PlayerInjurd(int Damage)
    {
        if (!PlayerBody.enabled)
        {
            return;
        }
        HPManagerScript.DecreaseHP(Damage);
        PlayerStateContextScript.SetPlayerState(new PlayerInjurd(PlayerStateContextScript, PlayerComponent));
        StartCoroutine(PlayInvincible(15, 0.1f));
    }

    private IEnumerator PlayInvincible(int playtimes,float playonce)
    {
        PlayerBody.enabled = false;
        for (int i = 0; i < playtimes; i++)
        {
            if (i % 2 == 1)
            {
                PlayerSrpite.color = Color.clear;
            }
            else
            {
                PlayerSrpite.color = Color.white;
            }
            yield return new WaitForSeconds(playonce);
        }
        PlayerSrpite.color = Color.white;
        PlayerBody.enabled = true;
    }
}