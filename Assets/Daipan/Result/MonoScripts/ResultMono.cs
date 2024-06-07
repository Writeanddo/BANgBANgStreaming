using SceneName;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultMono : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneTransition.TransitioningScene(SceneName.SceneName.TitleScene);
        }
    }
}
