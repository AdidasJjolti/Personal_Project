using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public float maxTime;         // ������ �ʿ� �ð�
    public float curTime;         // ������ �߻�κ��� �ҿ��� �ð�
    public float bulletPower;     // �Ѿ� �߻� �Ŀ�

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
