using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    Button newGameBtn;
    Button continueBtn;
    Button quitBtn;

    PlayableDirector director;

    void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();

        newGameBtn.onClick.AddListener(PlayTiemline);

        if (PlayerPrefs.HasKey("level"))
        {
            continueBtn.onClick.AddListener(ContinueGame);
            continueBtn.enabled = true;
            continueBtn.GetComponent<Image>().enabled = true;
        }
        else
        {
            continueBtn.GetComponent<Transform>().GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.gray;
            continueBtn.enabled = false;
            continueBtn.GetComponent<Image>().enabled = false;
        }

        quitBtn.onClick.AddListener(QuitGame);

        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;
    }

    void PlayTiemline()
    {
        director.Play();
    }

    void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();
        // 转换场景
        SceneControler.Instance.TransitionToFirstLevel();
    }

    void ContinueGame()
    {
        // 转换场景，读取进度
        SceneControler.Instance.TransitionToLoadGame();
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
