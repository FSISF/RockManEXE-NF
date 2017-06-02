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

        /// <summary>
        /// Get This ID Battle Chip Name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string BattleChipName(ushort id)
        {
            if (!BattleChipDatabase.ContainsKey(id))
            {
                return string.Empty;
            }
            return BattleChipDatabase[id].Name;
        }

        /// <summary>
        /// Get This ID Battle Chip Info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string BattleChipInfo(ushort id)
        {
            if (!BattleChipDatabase.ContainsKey(id))
            {
                return string.Empty;
            }
            return BattleChipDatabase[id].Info;
        }

        /// <summary>
        /// Get This ID Battle Chip Damage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public short BattleChipDamage(ushort id)
        {
            if (!BattleChipDatabase.ContainsKey(id))
            {
                return 0;
            }
            return BattleChipDatabase[id].Damage;
        }

        /// <summary>
        /// Get This ID Battle Chip Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BattleType BattleChipType(ushort id)
        {
            if (!BattleChipDatabase.ContainsKey(id))
            {
                return BattleType.None;
            }
            return BattleChipDatabase[id].Type;
        }

        /// <summary>
        /// Get This ID Battle Chip Features
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public byte BattleChipFeatures(ushort id)
        {
            if (!BattleChipDatabase.ContainsKey(id))
            {
                return 0;
            }
            return BattleChipDatabase[id].Features;
        }
    }
}