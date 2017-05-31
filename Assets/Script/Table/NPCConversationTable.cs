using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTable
{
    [System.Serializable]
    public struct NPCConversationType
    {
        public int ID;
        public string TalkContent;
        public int Next;
    }

    public class NPCConversationData
    {
        public NPCConversationType[] NPCConversation;
    }


    public class NPCConversationTable : Singleton<NPCConversationTable>
    {
        private NPCConversationData NPCConversationDataScript;
        public Dictionary<int, NPCConversationType> NPCConversationDataTable = new Dictionary<int, NPCConversationType>();

        public NPCConversationTable()
        {
            TextAsset Table = Resources.Load("GameTable/NPCConversation") as TextAsset;
            NPCConversationDataScript = JsonUtility.FromJson<NPCConversationData>(Table.ToString());
            AwakeTable();
        }

        private void AwakeTable()
        {
            for (int i = 0; i < NPCConversationDataScript.NPCConversation.Length; i++)
            {
                NPCConversationDataTable.Add(NPCConversationDataScript.NPCConversation[i].ID, NPCConversationDataScript.NPCConversation[i]);
            }
        }
    }
}
