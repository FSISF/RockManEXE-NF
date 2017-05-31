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
        CommonPlayerStateContextScript.StateWork();
    }

    private NPC NPCScript = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("NPC") && collision.gameObject.name == "NPCTalkCheck")
        {
            NPCScript = collision.GetComponentInParent<NPC>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("NPC") && collision.gameObject.name == "NPCTalkCheck")
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                ConversationManager.Instance.OpenTalk(NPCScript.TalkID);
            }
        }
    }
}