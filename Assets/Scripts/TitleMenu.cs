using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Doozy.Engine.UI;

public class TitleMenu : MonoBehaviour
{
    public List<UIView> TitleMenuViews;
    public UIButton StartButton;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ClickStart()
    {
        SceneManager.LoadScene("Main");
        foreach(UIView view in this.TitleMenuViews)
        {
            view.Hide();
        }
        this.StartButton.gameObject.SetActive(false);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "TitleScene")
        {
            foreach(UIView view in this.TitleMenuViews)
            {
                view.Show();
            }
            this.StartButton.gameObject.SetActive(true);
        }
    }
}
