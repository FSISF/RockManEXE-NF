using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTable;
using UnityEngine.UI;
using System;

public class ConversationManager : SingletonMono<ConversationManager>
{
    [SerializeField]
    private GameObject TalkGroup = null;

    [SerializeField]
    private Text TextContent = null;

    private bool IsTalking = false;

    private void Awake()
    {
        TalkContext.TextContent = TextContent;
    }

    private void Start()
    {

    }

    public void OpenTalk(int talkid)
    {
        TalkGroup.SetActive(true);

        if (IsTalking)
        {
            return;
        }
        IsTalking = true;
        StartCoroutine(PlayTalk(talkid, 0.1f));
    }

    private IEnumerator PlayTalk(int startid, float nextwordswait)
    {
        TalkContext TalkContextScript = new TalkContext(startid);
        while (true)
        {
            yield return TalkContextScript.StateWork();
            if (TalkContext.BreakWhile)
            {
                break;
            }
        }
        TalkContext.BreakWhile = false;
        IsTalking = false;
        TalkGroup.SetActive(false);
    }
}

public class TalkContext
{
    public static Text TextContent = null;
    public static bool BreakWhile = false;
    private static ITalkState TalkStateScript = null;

    public TalkContext(int id)
    {
        TalkStateScript = new TalkShow(id);
    }

    public IEnumerator StateWork()
    {
        yield return TalkStateScript.StateWork();
    }

    public static void SetState(ITalkState talkstate)
    {
        TalkStateScript = talkstate;
    }
}

public abstract class ITalkState
{
    public abstract IEnumerator StateWork();
}

/// <summary>
/// NPC Talk Words Show State
/// </summary>
public class TalkShow : ITalkState
{
    private string TempText;
    private int ID;

    public TalkShow(int id)
    {
        TempText = NPCConversationTable.Instance.NPCConversationDataTable[id].TalkContent;
        ID = id;
        TalkContext.TextContent.text = string.Empty;
    }

    public override IEnumerator StateWork()
    {
        for (int i = 0; i < TempText.Length; i++)
        {
            TalkContext.TextContent.text += TempText[i];
            yield return new WaitForSeconds(0.1f);
        }
        TalkContext.SetState(new TalkClickNext(ID));
    }
}

/// <summary>
/// Ready Input OK Click To Next Talk or Close
/// </summary>
public class TalkClickNext : ITalkState
{
    private int NextID = 0;
    public TalkClickNext(int id)
    {
        NextID = NPCConversationTable.Instance.NPCConversationDataTable[id].Next;
    }

    public override IEnumerator StateWork()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            switch (NextID)
            {
                case 0:
                    TalkContext.BreakWhile = true;
                    break;
                default:
                    TalkContext.SetState(new TalkShow(NextID));
                    break;
            }
        }
        yield return null;
    }
}