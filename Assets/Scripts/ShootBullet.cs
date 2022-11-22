using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public float maxTime;         // 재장전 필요 시간
    public float curTime;         // 마지막 발사로부터 소요한 시간
    public float bulletPower;     // 총알 발사 파워
    public bool isUp;

    [SerializeField] GameObject UpBulletPrefab;
    [SerializeField] GameObject LeftBulletPrefab;
    List<GameObject> UpBulletPrefabList = new List<GameObject>();
    List<GameObject> LeftBulletPrefabList = new List<GameObject>();

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;

        if(curTime >= maxTime)
        {
            Create();
            Shoot(PoolOut());

            curTime = 0;
        }
    }

    void Create()
    {
        if (isUp && UpBulletPrefabList.Count == 0)
        {
            GameObject bullet = Instantiate(UpBulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().SetMyParent(transform);
            UpBulletPrefabList.Add(bullet);
            bullet.SetActive(false);
        }
        else if (!isUp && LeftBulletPrefabList.Count == 0)
        {
            GameObject bullet = Instantiate(LeftBulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().SetMyParent(transform);
            LeftBulletPrefabList.Add(bullet);
            bullet.SetActive(false);
        }
    }

    GameObject PoolOut()           // 마지막에 만들어진 프리팹의 GameObject 자료형을 반환
    {
        if(isUp)
        {
            int LastIndex = UpBulletPrefabList.Count - 1;
            GameObject bullet = UpBulletPrefabList[LastIndex];
            UpBulletPrefabList.Remove(bullet);
            bullet.SetActive(true);

            return bullet;
        }
        else
        {
            int LastIndex = LeftBulletPrefabList.Count - 1;
            GameObject bullet = LeftBulletPrefabList[LastIndex];
            LeftBulletPrefabList.Remove(bullet);
            bullet.SetActive(true);

            return bullet;
        }
    }

    void Shoot(GameObject bullet)
    {
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        if (isUp)                // 위로 쏘는 오브젝트면 총알을 위로 발사
        {
            rigid.AddForce(Vector2.up * bulletPower, ForceMode2D.Impulse);
        }
        else
        {
            rigid.AddForce(Vector2.left * bulletPower, ForceMode2D.Impulse);
            bullet.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
    }

    public void PoolIn(GameObject bullet, bool isLeft)
    {
        if(isLeft)
        {
            LeftBulletPrefabList.Add(bullet);
            bullet.SetActive(false);
        }
        else
        {
            UpBulletPrefabList.Add(bullet);
            bullet.SetActive(false);
        }

        bullet.transform.position = transform.position;           // 총알의 위치를 총알 쏘는 위치로 다시 초기화
    }
}
