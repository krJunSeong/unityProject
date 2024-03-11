using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    [SerializeField] string sceneName = "";
    [SerializeField] GameObject img_blackWindow;

    private void OnEnable()
    {
        LoadingSceneManager.LoadScene(sceneName);
        img_blackWindow.SetActive(false);
    }
}
