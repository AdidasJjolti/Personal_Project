using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    float currentX;      // 현재 위치
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

        // 최대값보다 조금이라도 커지면 최대값으로 재조정
        if (currentX >= max)
        {
            currentX = max;
            direction *= -1;            // 오른쪽 끝에 닿으면 방향 전환, 22 line에서 현재 좌표값이 점차 감소 (좌측 이동 시작)
        }
        // 최소값보다 조금이라도 작아지면 최소값으로 재조정
        else if (currentX <= min)
        {
            currentX = min;
            direction *= -1;            // 왼쪽 끝에 닿으면 방향 전환, 22 line에서 현재 좌표값이 점차 증가 (우측 이동 시작)
        }

        transform.position = new Vector3(currentX, transform.position.y, transform.position.z);  // 플랫폼의 좌우값 변화

    }
}
