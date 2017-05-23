using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    public int MaxHp = 0;
    private int HP = 0;

    private void Awake()
    {
        HP = MaxHp;
    }

    void Start ()
    {
	}

    /// <summary>
    /// Recover Target HP
    /// </summary>
    /// <param name="Recover"></param>
    public void RecoverHP(int Recover)
    {
        int TotalRecover = Recover;
        HP += TotalRecover;
        if (HP > MaxHp)
        {
            HP = MaxHp;
        }
    }

    /// <summary>
    /// Decrease Target HP
    /// </summary>
    /// <param name="Damage"></param>
    public void DecreaseHP(int Damage)
    {
        int TotalDamage = Damage;
        HP -= TotalDamage;

        if (HP <= 0)
        {
            Destroy(this.transform.parent.parent.gameObject);
        }
    }
}