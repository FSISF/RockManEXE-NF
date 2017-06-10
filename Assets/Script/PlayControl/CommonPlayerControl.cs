using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonPlayerState;

public class CommonPlayerControl : Character
{
    private CommonPlayerComponent CommonPlayerComponentScript = new CommonPlayerComponent();
    private CommonPlayerStateContext CommonPlayerStateContextScript = new CommonPlayerStateContext();

    private void Awake()
    {
        SetCommonPlayerComponent();
    }

    private void SetCommonPlayerComponent()
    {
        CommonPlayerComponentScript.ThisGameObject = CharacterGameObject;
        CommonPlayerComponentScript.ThisTransform = CharacterTransform;
        CommonPlayerComponentScript.ThisRigidbody2D = CharacterRigidbody2D;
        CommonPlayerComponentScript.ThisAnimator = CharacterAnimator;

        CommonPlayerComponentScript.MoveSpeed = MoveSpeed;
    }

    void Start()
    {
        CommonPlayerStateContextScript.AwakePlayerState(CommonPlayerComponentScript);
    }

    void Update()
    {
        Direct = CommonPlayerComponentScript.Direct;
        CommonPlayerStateContextScript.StateWork();
    }
}