using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTable;
using UnityEngine.UI;

public class ConversationManager : SingletonMono<ConversationManager>
{
    [SerializeField]
    private GameObject TalkGroup = null;

    [SerializeField]
    private Text TextContent = null;

    private bool IsTalking = false;

    private int id = 0;
    private Dictionary<int, NPCConversationType> NPCConversationDataTable = new Dictionary<int, NPCConversationType>();
    private string TempText = string.Empty;

    private void Awake()
    {
        NPCConversationDataTable = NPCConversationTable.Instance.NPCConversationDataTable;
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
        //NPCStateContextScript.SetState(new NPCTalking(NPCStateContextScript, NPCComponentScript));
        StartCoroutine(PlayTalk(talkid, 0.1f));
    }

    private IEnumerator PlayTalk(int startid, float nextwordswait)
    {
        id = startid;
        TempText = NPCConversationDataTable[id].TalkContent;
        TextContent.text = string.Empty;

        while (true)
        {
            if (TextContent.text != TempText)
            {
                for (int i = 0; i < TempText.Length; i++)
                {
                    TextContent.text += TempText[i];
                    yield return new WaitForSeconds(nextwordswait);
                }
            }
            else if (TextContent.text == TempText && Input.GetKeyDown(KeyCode.J))
            {
                id = NPCConversationDataTable[id].Next;
                if (id == 0)
                {
                    break;
                }
                TempText = NPCConversationDataTable[id].TalkContent;
                TextContent.text = string.Empty;
            }
            yield return null;
        }

        IsTalking = false;
        TalkGroup.SetActive(false);
    }
}