using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField]
    string sceneName = "";

    private void OnEnable()
    {
        LoadingSceneManager.LoadScene(sceneName);
    }
}
