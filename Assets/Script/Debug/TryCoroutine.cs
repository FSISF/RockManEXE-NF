using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryCoroutine : SingletonMono<TryCoroutine>
{
    public delegate IEnumerator CoroutineDoVoid();

    public void DoCoroutine(CoroutineDoVoid mycoroutine)
    {
        StartCoroutine(DoTryCoroutine(mycoroutine));
    }

    private IEnumerator DoTryCoroutine(CoroutineDoVoid mycoroutine)
    {
        mycoroutine();
        yield return null;
    }
}
