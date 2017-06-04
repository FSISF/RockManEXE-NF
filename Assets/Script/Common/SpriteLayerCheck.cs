using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerCheck : MonoBehaviour
{
    [SerializeField]
    private Transform ThisTransform = null;

    [SerializeField]
    private SpriteRenderer ThisSpriteRenderer = null;

    void Start()
    {
        ThisSpriteRenderer.sortingOrder = Mathf.RoundToInt(ThisTransform.position.y * 100) * -1;
    }

    void Update()
    {
        if (!this.gameObject.isStatic)
        {
            ThisSpriteRenderer.sortingOrder = Mathf.RoundToInt(ThisTransform.position.y * 100) * -1;
        }
    }
}