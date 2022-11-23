using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public Transform player;
    public Transform SavePoint;
    int GameScene;

    private bool isSavePoint;

    public bool IsSavePoint
    {
        get { return isSavePoint; }
    }


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
        GameScene = SceneManager.GetActiveScene().buildIndex;     // ���� �Ŵ����� �ִ� �� ��ȣ(�÷����� ��)�� ����


    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown("R"))
        {
            SceneManager.LoadScene(GameScene);
        }
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
        Debug.Log("���� ����");
    }

    public void GameClear()
    {
        Debug.Log("���� Ŭ����");
    }
}
