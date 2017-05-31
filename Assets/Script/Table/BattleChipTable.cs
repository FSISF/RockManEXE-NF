using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTable
{
    public enum BattleType
    {
        None = 0,
        Fire,
        Water,
        Thunder,
        Wood,
    }

    [System.Serializable]
    public struct BattleChipType
    {
        public ushort ID;
        public string Name;
        public string Info;
        public short Damage;
        public BattleType Type;
        public byte Features;
    }

    public class BattleChipData
    {
        public BattleChipType[] BattleChip;
    }

    public class BattleChipTable : Singleton<BattleChipTable>
    {
        private BattleChipData BattleChipDataScript;
        private Dictionary<ushort, BattleChipType> BattleChipDatabase = new Dictionary<ushort, BattleChipType>();

        public BattleChipTable()
        {
            TextAsset Table = Resources.Load("GameTable/BattleChip") as TextAsset;
            BattleChipDataScript = JsonUtility.FromJson<BattleChipData>(Table.ToString());
            AwakeTable();
        }

        private void AwakeTable()
        {
            for (int i = 0; i < BattleChipDataScript.BattleChip.Length; i++)
            {
                BattleChipDatabase.Add(BattleChipDataScript.BattleChip[i].ID, BattleChipDataScript.BattleChip[i]);
            }
        }
    }
}