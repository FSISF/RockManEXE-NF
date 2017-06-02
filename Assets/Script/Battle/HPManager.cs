using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    public float MaxHp = 0;
    private float hp = 0;
    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            DoHPShow();
        }
    }

    private Slider SliderPlayerHP = null;
    private Text TextPlayerHP = null;

    [SerializeField]
    private GameObject HPBar;

    private GameObject CloneHPBar = null;
    private RectTransform RectTransformEnemyHP = null;
    private Slider SliderEnemyHP = null;

    private void Awake()
    {
        if (this.gameObject.tag.Contains("Player"))
        {
            SliderPlayerHP = GameObject.Find("SliderPlayerHP").GetComponent<Slider>();
            TextPlayerHP = GameObject.Find("TextPlayerHP").GetComponent<Text>();
        }
        else if (this.gameObject.tag.Contains("Enemy"))
        {
            DoCloneHPBar();
        }

        HP = MaxHp;
    }

    void Start()
    {
    }

    /// <summary>
    /// Clone HP Bar 
    /// </summary>
    private void DoCloneHPBar()
    {
        CloneHPBar = Instantiate(HPBar);
        RectTransformEnemyHP = CloneHPBar.GetComponent<RectTransform>();
        SliderEnemyHP = CloneHPBar.GetComponent<Slider>();

        RectTransform Parent = GameObject.Find("HpShowGroup").GetComponent<RectTransform>();
        RectTransformEnemyHP.SetParent(Parent, false);
    }

    private void DoHPShow()
    {
        float HPValue = hp / MaxHp;
        if (this.gameObject.tag.Contains("Player"))
        {
            SliderPlayerHP.value = HPValue;
            TextPlayerHP.text = string.Format("{0}/{1}", hp.ToString(), MaxHp.ToString());
        }
        else if (this.gameObject.tag.Contains("Enemy"))
        {
            if (SliderEnemyHP != null)
            {
                SliderEnemyHP.value = HPValue;
            }
            if (hp == 0 && CloneHPBar != null)
            {
                Destroy(CloneHPBar);
            }
        }
    }

    private void Update()
    {
        //Set HP Bar near Enemy
        if (this.gameObject.tag.Contains("Enemy") && RectTransformEnemyHP != null)
        {
            RectTransformEnemyHP.anchoredPosition = UIPosition.Instance.WorldToUI(this.transform.position - (Vector3.up * 1.5f));
        }
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