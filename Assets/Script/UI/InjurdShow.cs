using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InjurdShow : MonoBehaviour
{
    [SerializeField]
    private RectTransform ThisRectTransform;

    [SerializeField]
    private Text TextInjurdShow = null;

    private int injurdnum = 0;
    private int InjurdNum
    {
        set
        {
            injurdnum = value;
            TextInjurdShow.text = "-" + injurdnum.ToString();
        }
    }

    void Start()
    {

    }

    public void CloneToPlay(int injurd, Vector3 clonepos)
    {
        Transform Parent = GameObject.Find("InjurdShowGroup").transform;
        ThisRectTransform.SetParent(Parent, false);
        ThisRectTransform.anchoredPosition = UIPosition.Instance.WorldToUI(clonepos);
        InjurdNum = injurd;
        StartCoroutine(PlayInjurdUp());
    }

    IEnumerator PlayInjurdUp()
    {
        ThisRectTransform.DOMoveY(1, 0.5f);
        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }
}