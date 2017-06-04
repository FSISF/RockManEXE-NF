using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    void Start()
    {

    }

    public void ClickChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
