using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneState : SingletonMono<SceneState>
{
    [SerializeField]
    private Image ImageFadeBlack = null;

    private SceneStateContext SceneStateContextScript = null;

    private void Awake()
    {
        SceneStateContextScript = new SceneStateContext(ImageFadeBlack);
        StartCoroutine(PlayState());
    }

    /// <summary>
    /// Do this Scene End & Change Scene
    /// </summary>
    public void SceneChange(string scenename)
    {
        SceneEnd.SceneName = scenename;
        SceneStateContextScript.SetState(new SceneEnd(SceneStateContextScript, ImageFadeBlack));
    }

    private IEnumerator PlayState()
    {
        while (true)
        {
            yield return SceneStateContextScript.StateWork();
        }
    }
}

public class SceneStateContext
{
    private ISceneState SceneStateScript = null;

    public SceneStateContext(Image imagefadeblack)
    {
        SceneStateScript = new SceneStart(this, imagefadeblack);
    }

    public void SetState(ISceneState iscenestate)
    {
        SceneStateScript = iscenestate;
    }

    public IEnumerator StateWork()
    {
        yield return SceneStateScript.StateWork();
    }
}

public abstract class ISceneState
{
    protected SceneStateContext SceneStateContextScript;
    protected Image ImageFadeBlack;

    public abstract IEnumerator StateWork();
}

/// <summary>
/// Scene Start State
/// </summary>
public class SceneStart : ISceneState
{
    public SceneStart(SceneStateContext scenestatecontext, Image imagefadeblack)
    {
        SceneStateContextScript = scenestatecontext;
        ImageFadeBlack = imagefadeblack;
        ImageFadeBlack.color = Color.black;
    }

    public override IEnumerator StateWork()
    {
        ImageFadeBlack.DOColor(Color.clear, 0.25f);
        yield return new WaitForSeconds(0.25f);

        ImageFadeBlack.enabled = false;
        SceneStateContextScript.SetState(new SceneUpdate());
    }
}

/// <summary>
/// Scene Update State
/// </summary>
public class SceneUpdate : ISceneState
{
    public SceneUpdate()
    {
    }

    public override IEnumerator StateWork()
    {
        yield return null;
    }
}

/// <summary>
/// Scene End State
/// </summary>
public class SceneEnd : ISceneState
{
    public static string SceneName;

    public SceneEnd(SceneStateContext scenestatecontext, Image imagefadeblack)
    {
        SceneStateContextScript = scenestatecontext;
        ImageFadeBlack = imagefadeblack;
        ImageFadeBlack.color = Color.clear;
        ImageFadeBlack.enabled = true;
    }

    public override IEnumerator StateWork()
    {
        ImageFadeBlack.DOColor(Color.black, 0.25f);
        yield return new WaitForSeconds(0.25f);

        SceneManager.LoadScene(SceneName);
    }
}