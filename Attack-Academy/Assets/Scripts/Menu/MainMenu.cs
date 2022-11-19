using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string playScene;

    public void Play()
    {
        SceneManager.LoadScene(playScene);
    }

    public void Quit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        if (Application.isEditor)
        {
            EditorApplication.isPlaying = false;
        }
        #endif
    }
}
