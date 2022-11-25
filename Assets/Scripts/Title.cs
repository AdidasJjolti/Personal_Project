using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            OnClickStartButton();
        }
    }

    public void OnClickStartButton()
    {
         Invoke("MoveNextScene", 5f);
    }

    public void MoveNextScene()
    {
        SceneManager.LoadScene(1);
    }
}
