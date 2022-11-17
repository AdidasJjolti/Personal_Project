using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    float currentX;      // ���� ��ġ
    int direction = 1;
    [SerializeField] float min;
    [SerializeField] float max;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentX += Time.deltaTime * direction * speed;

        // �ִ밪���� �����̶� Ŀ���� �ִ밪���� ������
        if (currentX >= max)
        {
            currentX = max;
            direction *= -1;            // ������ ���� ������ ���� ��ȯ, 22 line���� ���� ��ǥ���� ���� ���� (���� �̵� ����)
        }
        // �ּҰ����� �����̶� �۾����� �ּҰ����� ������
        else if (currentX <= min)
        {
            currentX = min;
            direction *= -1;            // ���� ���� ������ ���� ��ȯ, 22 line���� ���� ��ǥ���� ���� ���� (���� �̵� ����)
        }

        transform.position = new Vector3(currentX, transform.position.y, transform.position.z);  // �÷����� �¿찪 ��ȭ

    }
}
