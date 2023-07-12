using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    [SerializeField] ARPlaneManager planeManager;
    [SerializeField] GameObject gameStartButton;
    [SerializeField] GameObject player;
    [SerializeField] GameObject follower;
    public TextMeshProUGUI timerText;
    public GameObject retryButton;
    public bool goal;
    private bool start = false;

    GameObject[] objects;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        timerText = timerText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (start && goal == false)
        {
            time += Time.deltaTime;
        }

        objects = GameObject.FindGameObjectsWithTag("Obj");

        if (objects.Length == 0)
        {
            goal = true;
            retryButton.SetActive(true);
        }

        int currentTime = Mathf.FloorToInt(time);
        timerText.text = "Time: " + currentTime.ToString();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameStart()
    {
        // planeを削除
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
        // planeManagerを非有効化
        planeManager.enabled = false;
        // スタートボタンを削除
        gameStartButton.SetActive(false);
        // プレイヤーとフォロワーを表示
        player.SetActive(true);
        follower.SetActive(true);
        // スタートをtrue
        start = true;
    }
}
