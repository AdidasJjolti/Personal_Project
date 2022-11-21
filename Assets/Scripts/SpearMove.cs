using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearMove : MonoBehaviour
{
    public bool isRising;
    public Vector3 startPosition;
    public Vector3 endPosition;


    public float speed;
    float currentY;      // ���� ��ġ
    int direction = 1;
    [SerializeField] float min;
    [SerializeField] float max;

    bool isCoolTime;     // ����â�� ������ �Ʒ��� �������� �� y ��ǥ ���� ����(����) ����ϴ� ���� ��� ���� ���� ����

    void Update()
    {
         if(!isCoolTime)                  // ����â�� ���� ���ȿ��� ��ǥ �������� ����
        {
            currentY += Time.deltaTime * direction * speed;

            // �ִ밪���� �����̶� Ŀ���� �ִ밪���� ������
            if (currentY >= max)
            {
                currentY = max;
                direction *= -1;            // ������ ���� ������ ���� ��ȯ, 22 line���� ���� ��ǥ���� ���� ���� (���� �̵� ����)
            }
            // �ּҰ����� �����̶� �۾����� �ּҰ����� ������
            else if (currentY <= min)
            {
                currentY = min;
                direction *= -1;            // ���� ���� ������ ���� ��ȯ, 22 line���� ���� ��ǥ���� ���� ���� (���� �̵� ����)

                StartCoroutine("SpearCoolTime");
            }

            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);  // �÷����� �¿찪 ��ȭ
        }
    }

    IEnumerator SpearCoolTime()
    {
        isCoolTime = true;

        float curTime = 0;
        float maxTime = 2;

        while (maxTime > curTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            curTime += Time.deltaTime;
        }

        isCoolTime = false;
    }
}
