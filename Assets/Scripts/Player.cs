using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    CapsuleCollider2D playerCollider;
    SpriteRenderer spriteRenderer;
    Animator anim;
    AudioSource audioSource;
    public AudioClip[] audioClips;

    
    public float maxSpeed;
    public float jumpPower;
    public bool isJumping;
    public bool isDamaged;
    public int KnockbackPower;

    enum CLIPNAME
    {
        JUMP = 0, 
        DAMAGED
    }

    [SerializeField]private int health = 3;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        anim.SetBool("isRun", false);
        anim.SetBool("isJump", false);

        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Update()
    {
        if(!isDamaged)           // 대미지를 입지 않은 동안에만 이동 관련 함수 실행
        {
            Move();
            Jump();
        }
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // 이동하는 중에만 뛰는 애니 재생
        if(Mathf.Abs(rigid.velocity.x) != 0)
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }

        // 최대 속도 제어
        if(maxSpeed < rigid.velocity.x)
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if(maxSpeed * -1 > rigid.velocity.x)
        {
            rigid.velocity = new Vector2(maxSpeed * -1, rigid.velocity.y);
        }


        // 진행 방향 따라 x축 회전
        if (h < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (h > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            if (!isJumping)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isJumping = true;

            anim.SetBool("isJump", true);                                        // 점프 애니 재생

            audioSource.clip = audioClips[(int)CLIPNAME.JUMP];
            audioSource.Play();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)                               // 이중 점프 방지
    {
        if(collision.transform.CompareTag("Platform"))
        {
            isJumping = false;
            anim.SetBool("isJump", false);
        }

        else if (collision.transform.CompareTag("Moving Platform"))            // 움직이는 발판 위에 있을 때 실행
        {
            isJumping = false;
            anim.SetBool("isJump", false);
            this.transform.parent = collision.transform;                       // 움직이는 발판을 플레이어의 부모 오브젝트로 설정
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Moving Platform"))
        {
            isJumping = true;
            anim.SetBool("isJump", true);
            this.transform.parent = null;                                    // 움직이는 발판을 플레이어의 부모 오브젝트에서 해제
        }
    }

    void OnTriggerEnter2D(Collider2D collision)                                  // 점프대 구현
    {
        if(collision.transform.CompareTag("Springboard") && rigid.velocity.y < 0)
        {
            rigid.AddForce(Vector2.up * 25, ForceMode2D.Impulse);
            isJumping = true;
            anim.SetBool("isJump", true);
        }
        else if(collision.transform.CompareTag("Item"))
        {
            collision.gameObject.SetActive(false);
            health++;
        }
        else if(collision.transform.CompareTag("Save Point"))
        {
            GameManager.Instance.SetSavePoint();
        }
        else if(collision.transform.CompareTag("Falling Border"))
        {
            if(!GameManager.Instance.IsSavePoint)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                if(health >1)
                {
                    health--;
                    GameManager.Instance.SetPlayerPosition();
                }
                else if(health <=1)
                {
                    GameManager.Instance.GameOver();
                }
            }
        }
        else if (collision.transform.CompareTag("Clear Point"))
        {
            GameManager.Instance.GameClear();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Enemy") && !isDamaged)             // 이중 피격 방지
        {
            OnDamaged();

            float dirVec = transform.position.x - collision.transform.position.x;

            if(dirVec <= 0)
            {
                rigid.AddForce((Vector2.left + Vector2.up) * KnockbackPower, ForceMode2D.Impulse);
            }
            else if(dirVec > 0)
            {
                rigid.AddForce((Vector2.right + Vector2.up) * KnockbackPower, ForceMode2D.Impulse);
            }

            Invoke("OffDamaged", 0.5f);
        }
    }

    void OnDamaged()
    {
        isDamaged = true;
        health--;

        audioSource.clip = audioClips[(int)CLIPNAME.DAMAGED];
        audioSource.Play();

        StartCoroutine(OnDamagedSprite());
    }

    IEnumerator OnDamagedSprite ()
    {
        float blinkTime = 0.5f;
        float curTime = 0f;

        while (blinkTime > curTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.125f);
            spriteRenderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.125f);

            curTime += 0.25f;
        }
    }

    void OffDamaged()          // 피격 상태 해제
    {
        isDamaged = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Trap Platform" && rigid.velocity.y == 0)
        {
            transform.position = FindObjectOfType<GameManager>().ReturnPoint.position;
        }
    }
}
