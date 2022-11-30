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
    public TextMeshProUGUI gameoverText;
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
        gameScene = SceneManager.GetActiveScene().buildIndex;     // 게임 매니저 스크립트를 불러오는 씬 번호를 gameScene으로 저장

        SceneManager.sceneLoaded += OnSceneLoaded;
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
            LapsedTime = 0;
        }

        SetTimeUI();
        if(player)
        {
            SetHealthUI();
        }
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().buildIndex == gameScene)       // 현재 씬 번호와 게임 씬 번호를 비교
        {
            InitVariables();
        }
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

        if (!timeUI)
        {
            timeUI = GameObject.Find("Time UI").GetComponent<TextMeshProUGUI>();
        }

        if (!clearText)
        {
            clearText = GameObject.Find("Canvas").transform.Find("Clear_Text").gameObject.GetComponent<TextMeshProUGUI>();
        }

        if (!gameoverText)
        {
            gameoverText = GameObject.Find("Canvas").transform.Find("Gameover_Text").gameObject.GetComponent<TextMeshProUGUI>();
        }

        if (!RetryButton)
        {
            RetryButton = GameObject.Find("Canvas").transform.Find("Retry Button").gameObject;      // retry 버튼을 찾기, canvas 내 게임오브젝트로 찾기
            Button retryButton = RetryButton.GetComponent<Button>();
            // retry버튼 onClick 이벤트를 코드로 입력, 게임 재실행할 때 게임매니저 내 public 오브젝트가 날아가면서 onClick 이벤트가 제거하는 것을 복구
            retryButton.onClick.AddListener(OnClickRetryButton);
        }

        if (!QuitButton)
        {
            QuitButton = GameObject.Find("Canvas").transform.Find("Quit Button").gameObject;
            Button quitButton = QuitButton.GetComponent<Button>();
            quitButton.onClick.AddListener(OnClickQuitButton);
        }

        Button button = GameObject.Find("Pause Button").GetComponent<Button>();

        // pause버튼 내 이벤트는 효과음 이벤트, OnClickPauseButton 이벤트 총 2개 필요, 1개(버튼 효과음 이벤트)만 남은 경우 OnClickPauseButton이벤트 추가
        if (button.onClick.GetPersistentEventCount()<=1)
        {
            button.onClick.AddListener(OnClickPauseButton);
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

            GameObject.Find("Canvas").transform.Find("Retry Button").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("Quit Button").gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            buttonImg.sprite = PauseButton;
            Camera.main.GetComponent<AudioSource>().Play();

            GameObject.Find("Canvas").transform.Find("Retry Button").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("Quit Button").gameObject.SetActive(false);
        }
    }

    public void OnClickRetryButton()
    {
        isPaused = !isPaused;
        SceneManager.LoadScene(gameScene);
        Time.timeScale = 1;
        LapsedTime = 0;
    }

    public void OnClickQuitButton()
    {
        isPaused = !isPaused;
        SceneManager.LoadScene(gameScene - 1);
        Time.timeScale = 1;
        LapsedTime = 0;
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
        GameObject.Find("Canvas").transform.Find("Gameover_Text").gameObject.SetActive(true);

        LapsedTime += Time.deltaTime;
        float minutes = Mathf.Floor(LapsedTime / 60);
        float seconds = (int)(LapsedTime % 60);
        string minutesS = minutes.ToString();
        string secondsS = seconds.ToString();

        gameoverText.text = "Game Over\nRecord " + string.Format("{0}:{1}", minutesS, secondsS);

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
