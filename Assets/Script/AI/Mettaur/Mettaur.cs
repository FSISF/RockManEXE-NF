using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MettaurState;

public class Mettaur : BattleCharacter
{
    private MettaurComponent MettaurComponentScript = new MettaurComponent();
    private MettaurStateContext MettaurStateContextScript = new MettaurStateContext();

    private void Awake()
    {
        SetMettaurComponent();
        MettaurStateContextScript.AwakeStateSet(MettaurComponentScript);
    }

    private void SetMettaurComponent()
    {
        MettaurComponentScript.ThisGameObject = CharacterGameObject;
        MettaurComponentScript.ThisTransform = CharacterTransform;
        MettaurComponentScript.ThisRigidbody2D = CharacterRigidbody2D;
        MettaurComponentScript.ThisAnimator = CharacterAnimator;

        MettaurComponentScript.MoveSpeed = CharacterSpeed;
        MettaurComponentScript.Direct = CharacterDirect;
        MettaurComponentScript.ThisBodyCheck = CharacterBodyCehck.gameObject;
    }

    void Start()
    {
        SetFollowTarget();
    }

    void Update()
    {
        MettaurComponentScript.FollowTarget = FollowTarget;
        MettaurStateContextScript.StateWork();
        CheckGround();
    }

    protected override void CheckGround()
    {
        base.CheckGround();
        MettaurComponentScript.Grounded = CharacterGrounded;
    }

    public override void CharacterInjurd(int Damage)
    {
        HPManagerScript.DecreaseHP(Damage);
        CloneInjurdShow(Damage);
    }
}