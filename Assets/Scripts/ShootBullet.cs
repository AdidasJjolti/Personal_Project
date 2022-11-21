using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public float maxTime;         // 재장전 필요 시간
    public float curTime;         // 마지막 발사로부터 소요한 시간
    public float bulletPower;     // 총알 발사 파워

    public GameObject[] BulletPrefab;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;

        if(curTime >= maxTime)
        {
            GameObject bullet = Instantiate(BulletPrefab[0], transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(Vector2.up * bulletPower, ForceMode2D.Impulse);

            curTime = 0;
        }
    }
}
