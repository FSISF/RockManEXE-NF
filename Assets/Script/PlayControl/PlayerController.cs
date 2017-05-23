using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllPlayerState;

public class PlayerController : BattleCharacter
{
    private PlayerStateContext PlayerStateContextScript;
    private StateComponent PlayerComponent = new StateComponent();

    private void Awake()
    {
        SetPlayerComponent();
        PlayerStateContextScript = new PlayerStateContext();
        PlayerStateContextScript.AwakePlayerState(PlayerComponent);
    }

    private void SetPlayerComponent()//Set player component to use in state machine
    {
        PlayerComponent.ThisGameObject = CharacterGameObject;
        PlayerComponent.ThisTransform = CharacterTransform;
        PlayerComponent.ThisRigidbody2D = CharacterRigidbody2D;
        PlayerComponent.ThisAnimator = CharacterAnimator;

        PlayerComponent.MoveSpeed = CharacterSpeed;
        PlayerComponent.JumpForce = CharacterJumpForce;
        PlayerComponent.Direct = CharacterDirect;
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
        if (collision.gameObject.tag.Contains("Enemy") && collision.gameObject.layer == CharacterGameObject.layer)
        {
            CharacterInjurd(20);
        }
    }

    private void CheckGround()//Check player is on ground
    {
        Vector2 LinecastEnd = new Vector2(CharacterTransform.position.x, CharacterTransform.position.y - 0.1f);
        LayerMask TargetGround = LayerMask.GetMask(LayerMask.LayerToName(CharacterGameObject.layer + 3));
        CharacterGrounded = Physics2D.Linecast(CharacterTransform.position, LinecastEnd, TargetGround);
        PlayerComponent.Grounded = CharacterGrounded;
    }

    public override void CharacterInjurd(int Damage)
    {
        if (!CharacterBodyCehck.enabled)
        {
            return;
        }
        HPManagerScript.DecreaseHP(Damage);
        PlayerStateContextScript.SetPlayerState(new PlayerInjurd(PlayerStateContextScript, PlayerComponent));
        StartCoroutine(PlayInvincible(15, 0.1f));
    }

    private IEnumerator PlayInvincible(int playtimes,float playonce)
    {
        CharacterBodyCehck.enabled = false;
        for (int i = 0; i < playtimes; i++)
        {
            if (i % 2 == 1)
            {
                CharacterPlayerSrpite.color = Color.clear;
            }
            else
            {
                CharacterPlayerSrpite.color = Color.white;
            }
            yield return new WaitForSeconds(playonce);
        }
        CharacterPlayerSrpite.color = Color.white;
        CharacterBodyCehck.enabled = true;
    }
}