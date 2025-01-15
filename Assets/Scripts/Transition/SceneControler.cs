using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneControler : Singleton<SceneControler>, IEndGameObserver
{
    public GameObject playerPrefeb;
    public SceneFade sceneFadePrefab;
    bool fadeFinished;
    GameObject player;
    NavMeshAgent playerAgent;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GameManager.Instance.AddObserver(this);
        fadeFinished = true;
    }

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Trasnsition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScenen:
                StartCoroutine(Trasnsition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }

    IEnumerator Trasnsition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        // 保存数据
        SaveManager.Instance.SavePlayerData();

        if (SceneManager.GetActiveScene().name != sceneName)
        {
            // FIXME: 可以加入fader;
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefeb, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            // 读取数据
            SaveManager.Instance.LoadPlayerData();
            yield break;
        }
        else
        {
            player = GameManager.Instance.playerStats.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            playerAgent.enabled = true;
            yield return null;
        }
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();

        foreach (var entrance in entrances)
        {
            if (entrance.destinationTag == destinationTag)
                return entrance;
        }
        return null;
    }

    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
    }

    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("GameSceneStart"));
    }

    IEnumerator LoadLevel(string scene)
    {
        SceneFade fade = Instantiate(sceneFadePrefab);
        if (scene != "")
        {
            yield return StartCoroutine(fade.FadeOut(2f));
            yield return SceneManager.LoadSceneAsync(scene);
            yield return Instantiate(playerPrefeb, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);

            // 保存数据
            SaveManager.Instance.SavePlayerData();
            yield return StartCoroutine(fade.FadeIn(2f));
            yield break;
        }
    }

    IEnumerator LoadMain()
    {
        SceneFade fade = Instantiate(sceneFadePrefab);
        yield return StartCoroutine(fade.FadeOut(2f));
        yield return SceneManager.LoadSceneAsync("GameSceneMain");
        yield return StartCoroutine(fade.FadeIn(2f));
        yield break;
    }

    public void EndNotify()
    {
        if (fadeFinished)
        {
            fadeFinished = false;
            StartCoroutine(LoadMain());
        }
    }
}
