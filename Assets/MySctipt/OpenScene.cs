using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenScene : MonoBehaviour
{
    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void OpenTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
