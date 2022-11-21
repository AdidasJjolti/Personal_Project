using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearMove : MonoBehaviour
{
    public bool isRising;
    public Vector3 startPosition;
    public Vector3 endPosition;


    public float speed;
    float currentY;      // 현재 위치
    int direction = 1;
    [SerializeField] float min;
    [SerializeField] float max;

    bool isCoolTime;     // 삼지창에 완전히 아래로 내려왔을 때 y 좌표 값을 증가(감소) 계산하는 것을 잠시 막기 위한 변수

    void Update()
    {
         if(!isCoolTime)                  // 삼지창이 멈춘 동안에는 좌표 변경하지 않음
        {
            currentY += Time.deltaTime * direction * speed;

            // 최대값보다 조금이라도 커지면 최대값으로 재조정
            if (currentY >= max)
            {
                currentY = max;
                direction *= -1;            // 오른쪽 끝에 닿으면 방향 전환, 22 line에서 현재 좌표값이 점차 감소 (좌측 이동 시작)
            }
            // 최소값보다 조금이라도 작아지면 최소값으로 재조정
            else if (currentY <= min)
            {
                currentY = min;
                direction *= -1;            // 왼쪽 끝에 닿으면 방향 전환, 22 line에서 현재 좌표값이 점차 증가 (우측 이동 시작)

                StartCoroutine("SpearCoolTime");
            }

            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);  // 플랫폼의 좌우값 변화
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
