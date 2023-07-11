using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject follower;
    public TextMeshProUGUI timerText;
    public GameObject retryButton;
    public bool goal;

    GameObject[] objects;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        //timerText = timerText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goal == false)
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
        //timerText.text = "Time: " + currentTime.ToString();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameStart()
    {
        player.SetActive(true);
        follower.SetActive(true);
    }
}
