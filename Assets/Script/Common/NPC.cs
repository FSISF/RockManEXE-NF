using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCState;
using GameTable;

public enum StartDirect
{
    Up,
    Down,
    Left,
    Right,
}

public class NPC : Character
{
    [SerializeField]
    private string NPCName;

    [SerializeField]
    private StartDirect StartDirectEnum;

    [SerializeField]
    private MoveStyle ThisMoveStyle;

    [SerializeField]
    private List<Vector2> MoveLine = new List<Vector2>();

    [SerializeField]
    private int talkID = 0;
    public int TalkID
    {
        get
        {
            return talkID;
        }
    }

    private NPCStateContext NPCStateContextScript = new NPCStateContext();
    private NPCComponent NPCComponentScript = new NPCComponent();

    [SerializeField]
    private Transform PlayerTransform = null;
    [SerializeField]
    private CommonPlayerControl CommonPlayerControlScript = null;

    private void Awake()
    {
        AddFirstPoint();

        if (CharacterAnimator != null)
        {
            StartDirectPlay();
        }

        SetNPCCompent();

        NPCStateContextScript.AwakeSet(NPCComponentScript);
    }

    private void AddFirstPoint()
    {
        List<Vector2> TempMoveLine = new List<Vector2>();
        TempMoveLine.Add(CharacterTransform.position);
        TempMoveLine.AddRange(MoveLine);
        MoveLine = TempMoveLine;
    }

    private void StartDirectPlay()
    {
        switch (StartDirectEnum)
        {
            case StartDirect.Up:
                CharacterAnimator.Play(NPCName + "MoveUp");
                break;
            case StartDirect.Down:
                CharacterAnimator.Play(NPCName + "MoveDown");
                break;
            case StartDirect.Left:
                CharacterAnimator.Play(NPCName + "MoveLeft");
                break;
            case StartDirect.Right:
                CharacterAnimator.Play(NPCName + "MoveRight");
                break;
        }
    }

    private void SetNPCCompent()
    {
        NPCComponentScript.ThisGameObject = CharacterGameObject;
        NPCComponentScript.ThisTransform = CharacterTransform;
        NPCComponentScript.ThisRigidbody2D = CharacterRigidbody2D;
        NPCComponentScript.ThisAnimator = CharacterAnimator;

        NPCComponentScript.MoveSpeed = MoveSpeed;

        NPCComponentScript.ThisMoveStyle = ThisMoveStyle;
        NPCComponentScript.MoveLine = MoveLine;
        NPCComponentScript.NPCName = NPCName;
    }

    void Start()
    {
    }

    void Update()
    {
        MathPlayerAngle();
        NPCStateContextScript.StateWork();
    }


    public float ToPlayerDistance = 0;
    public Vector3 ToPlayerNormalize;
    public float ToPlayerAngle = 0;
    /// <summary>
    /// Check Player Face to NPC & Near NPC
    /// </summary>
    private void MathPlayerAngle()
    {
        ToPlayerDistance = Vector2.Distance(CharacterTransform.position, PlayerTransform.position);
        if (ToPlayerDistance <= 1.5f)
        {
            ToPlayerNormalize = Vector3.Normalize(CharacterTransform.position - PlayerTransform.position);
            ToPlayerAngle = Mathf.Acos(Vector3.Dot(ToPlayerNormalize, CommonPlayerControlScript.Direct));
            if (ToPlayerAngle <= 1&&Input.GetKeyDown(KeyCode.J))
            {
                ConversationManager.Instance.OpenTalk(TalkID);
            }
        }
        else
        {
            ToPlayerNormalize = Vector3.zero;
            ToPlayerAngle = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            NPCStateContextScript.SetState(new NPCWaitting(NPCStateContextScript, NPCComponentScript));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            NPCStateContextScript.SetState(new NPCIdle(NPCStateContextScript, NPCComponentScript));
        }
    }

    private void OnDrawGizmos()
    {
        if (MoveLine.Count == 0 || CharacterTransform == null || ThisMoveStyle == MoveStyle.None || ThisMoveStyle == MoveStyle.SwitchDirect)
        {
            return;
        }

        Gizmos.color = Color.red;

        if (Application.isPlaying)
        {
            for (int i = 0; i < MoveLine.Count - 1; i++)
            {
                Gizmos.DrawLine(MoveLine[i], MoveLine[i + 1]);
            }
            if (ThisMoveStyle == MoveStyle.Loop)
            {
                Gizmos.DrawLine(MoveLine[MoveLine.Count - 1], MoveLine[0]);
            }
        }
        else
        {
            Gizmos.DrawLine(CharacterTransform.position, MoveLine[0]);
            for (int i = 1; i < MoveLine.Count; i++)
            {
                Gizmos.DrawLine(MoveLine[i - 1], MoveLine[i]);
            }
            if (ThisMoveStyle == MoveStyle.Loop)
            {
                Gizmos.DrawLine(MoveLine[MoveLine.Count - 1], CharacterTransform.position);
            }
        }
    }
}