using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<GameManager>();

                if(instance == null)
                {
                    Debug.LogError("���ӸŴ��� ã�� �� ����");
                    return null;
                }
            }
            return instance;
        }
    }

    [SerializeField] private Transform returnPoint;      //field
    public Transform ReturnPoint        //property
    {
        get { return returnPoint; }
        //set { returnPoint = value; }
    }

    private bool isSavePoint;

    public bool IsSavePoint
    {
        get { return isSavePoint; }
    }



    public Transform player;
    public Transform SavePoint;
    int gameScene;

    public TextMeshProUGUI healthUI;
    public TextMeshProUGUI timeUI;
    public TextMeshProUGUI clearText;
    public float LapsedTime;
    public GameObject RetryButton;
    public GameObject QuitButton;

    public Sprite PlayButton;
    public Sprite PauseButton;

    bool isPaused;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        gameScene = SceneManager.GetActiveScene().buildIndex;     // ���� �Ŵ����� �ִ� �� ��ȣ(�÷����� ��)�� ����
    }

    void Start()
    {
        LapsedTime = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(gameScene);
            InitVariables();
        }

        SetTimeUI();
        SetHealthUI();
    }

    void SetTimeUI()
    {
        LapsedTime += Time.deltaTime;
        float minutes = Mathf.Floor(LapsedTime / 60);
        float seconds = (int)(LapsedTime % 60);
        string minutesS = minutes.ToString();
        string secondsS = seconds.ToString();

        timeUI.text = "Time Lapsed " + string.Format("{0}:{1}", minutesS, secondsS);
    }

    void SetHealthUI()
    {
        int playerHP = FindObjectOfType<Player>().GetComponent<Player>().Health;
        healthUI.text = "HP: " + playerHP.ToString();
    }

    void InitVariables()
    {
        if(!player)
        {
            player = FindObjectOfType<Player>().transform;
        }

        if (!returnPoint)
        {
            returnPoint = GameObject.Find("Return Point").transform;
        }

        if (!SavePoint)
        {
            SavePoint = GameObject.Find("Checkpoint_Save").transform;
        }

        if (!healthUI)
        {
            healthUI = GameObject.Find("HP UI").GetComponent<TextMeshProUGUI>();
        }

        if (!RetryButton)
        {
            RetryButton = GameObject.Find("Retry Button");
        }

        if (!QuitButton)
        {
            QuitButton = GameObject.Find("Quit Button");
        }
    }


    public void OnClickPauseButton()
    {
        Image buttonImg = GameObject.Find("Pause Button").GetComponent<Image>();
        isPaused = !isPaused;
        

        if (isPaused)
        {
            Time.timeScale = 0;
            buttonImg.sprite = PlayButton;
            Camera.main.GetComponent<AudioSource>().Pause();
        }
        else
        {
            Time.timeScale = 1;
            buttonImg.sprite = PauseButton;
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }

    public void OnClickRetryButton()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void OnClickQuitButton()
    {
        SceneManager.LoadScene(gameScene - 1);
    }

    public void SetSavePoint()
    {
        isSavePoint = true;
    }

    public void SetPlayerPosition()
    {
        if(isSavePoint)
        {
            player.position = SavePoint.position;
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameObject.Find("Canvas").transform.Find("Retry Button").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("Quit Button").gameObject.SetActive(true);

    }

    public void GameClear()
    {
        Time.timeScale = 0;
        GameObject.Find("Canvas").transform.Find("Retry Button").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("Quit Button").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("Clear_Text").gameObject.SetActive(true);

        LapsedTime += Time.deltaTime;
        float minutes = Mathf.Floor(LapsedTime / 60);
        float seconds = (int)(LapsedTime % 60);
        string minutesS = minutes.ToString();
        string secondsS = seconds.ToString();

        clearText.text = "Congratulations!\nRecord " + string.Format("{0}:{1}", minutesS, secondsS);
    }
}
