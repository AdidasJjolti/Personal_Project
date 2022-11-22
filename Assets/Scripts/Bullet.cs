using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Transform Parent;   

    public void SetMyParent(Transform parent)
    {
        Parent = parent;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Bullet Border"))
        {
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            bool isLeft = rigid.velocity.y > 0 ? false : true;

            gameObject.SetActive(false);
            Parent.GetComponent<ShootBullet>().PoolIn(gameObject,isLeft);
        }
    }
}
