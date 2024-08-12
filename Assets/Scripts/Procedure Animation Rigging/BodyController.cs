using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    // 현재 속도 벡터
    Vector3 velocity;
    // 이전 프레임의 속도 벡터, 초기값은 (1,1,1)
    Vector3 lastVelocity = Vector3.one;
    // 이전 프레임에서 거미의 위치
    Vector3 lastSpiderPosition;
    // 각 다리의 현재 위치를 저장하는 배열
    Vector3[] legPositions;
    // 각 다리의 원래 위치를 저장하는 배열
    Vector3[] legOriginalPositions;
    // 다음에 움직여야 할 다리의 인덱스를 저장하는 리스트
    List<int> nextIndexToMove = new List<int>();
    // 현재 움직이고 있는 다리의 인덱스를 저장하는 리스트
    List<int> IndexMoving = new List<int>();
    // 이전 프레임의 몸체의 위쪽 방향 벡터
    Vector3 lastBodyUp;
    // 각 다리의 반대편 다리의 인덱스를 저장하는 리스트
    List<int> oppositeLeg = new List<int>();
    // 현재 처리 중인 다리의 상태를 나타내는 불리언 값
    bool currentLeg = true;
    // 리셋 타이머 (사용되지 않음)
    float resetTimer = 0.5f;

    [Space(10)]
    [Header("GameObject Assignment")]
    [Space(10)]

    // 거미 몸체의 GameObject
    public GameObject spider;
    // 다리의 목표 위치를 나타내는 GameObject 배열
    public GameObject[] legTargets;
    // 다리의 기준 위치를 나타내는 GameObject 배열
    public GameObject[] legCubes;

    [Space(10)]
    [Header("Rotation of Body and Movement of leg")]
    [Space(10)]

    // 몸체의 회전 활성화 여부
    public bool enableBodyRotation = false;
    // 다리 움직임에 따른 회전 활성화 여부
    public bool enableMovementRotation = false;
    // Rigidbody 컨트롤러 사용 여부
    public bool rigidBodyController;

    [Space(10)]
    [Header("Values for leg Movement")]
    [Space(10)]

    // 다리가 이동해야 할 최소 거리
    public float moveDistance = 0.7f;
    // 다리 이동 시 들어올리는 높이
    public float stepHeight = .15f;
    // 거미의 떨림을 방지하는 임계값
    public float spiderJitterCutOff = 0f;
    // 각 스텝 사이의 대기 시간
    public int waitTimeBetweenEveryStep = 0;
    // 다리 움직임의 부드러움 정도
    public float LegSmoothness = 8;
    // 몸체 움직임의 부드러움 정도
    public float BodySmoothness = 8;
    // 과도한 스텝 시 곱해지는 배수
    public float OverStepMultiplier = 4;

    void Start()
    {
        // 초기 몸체의 위쪽 방향을 현재의 up 벡터로 설정
        lastBodyUp = transform.up;

        // 배열 초기화
        legPositions = new Vector3[legTargets.Length];
        legOriginalPositions = new Vector3[legTargets.Length];

        // 각 다리의 초기 위치를 설정하고, 반대편 다리의 인덱스를 설정
        for (int i = 0; i < legTargets.Length; i++)
        {
            legPositions[i] = legTargets[i].transform.position;
            legOriginalPositions[i] = legTargets[i].transform.position;

            // currentLeg 값에 따라 oppositeLeg 리스트에 반대편 다리의 인덱스를 추가
            if (currentLeg)
            {
                oppositeLeg.Add(i + 1);
                currentLeg = false;
            }
            else
            {
                oppositeLeg.Add(i - 1);
                currentLeg = true;
            }
        }

        // 이전 거미 위치를 현재 위치로 설정
        lastSpiderPosition = spider.transform.position;

        // 몸체 회전 초기화
        rotateBody();
    }

    void FixedUpdate()
    {
        // 현재 프레임의 속도를 계산
        velocity = spider.transform.position - lastSpiderPosition;
        // 이전 프레임의 속도와 현재 속도를 평균하여 부드러운 속도 계산
        velocity = (velocity + BodySmoothness * lastVelocity) / (BodySmoothness + 1f);

        // 다리 움직임 및 몸체 회전 함수 호출
        moveLegs();
        rotateBody();

        // 이전 거미 위치와 속도를 현재 값으로 업데이트
        lastSpiderPosition = spider.transform.position;
        lastVelocity = velocity;
    }

    // 다리의 움직임을 제어하는 함수
    void moveLegs()
    {
        // 움직임 회전이 비활성화되어 있으면 함수 종료
        if (!enableMovementRotation) return;

        for (int i = 0; i < legTargets.Length; i++)
        {
            // 다리의 현재 위치와 기준 위치 사이의 거리가 moveDistance 이상이면 움직여야 함
            if (Vector3.Distance(legTargets[i].transform.position, legCubes[i].transform.position) >= moveDistance)
            {
                // 이미 움직일 예정이 아니고, 현재 움직이고 있지 않으면 움직일 리스트에 추가
                if (!nextIndexToMove.Contains(i) && !IndexMoving.Contains(i))
                    nextIndexToMove.Add(i);
            }
            else if (!IndexMoving.Contains(i))
            {
                // 움직일 필요가 없으면 다리를 원래 위치로 되돌림
                legTargets[i].transform.position = legOriginalPositions[i];
            }
        }

        // 움직일 다리가 없거나, 이미 움직임이 진행 중이면 함수 종료
        if (nextIndexToMove.Count == 0 || IndexMoving.Count != 0) return;

        // 다음으로 움직일 다리의 목표 위치를 계산
        Vector3 targetPosition = legCubes[nextIndexToMove[0]].transform.position
                               + Mathf.Clamp(velocity.magnitude * OverStepMultiplier, 0.0f, 1.5f)
                               * (legCubes[nextIndexToMove[0]].transform.position - legTargets[nextIndexToMove[0]].transform.position)
                               + velocity * OverStepMultiplier;

        // 다리 움직임을 처리하는 코루틴 시작
        StartCoroutine(step(nextIndexToMove[0], targetPosition, false));
    }

    // 다리를 부드럽게 움직이기 위한 코루틴 함수
    IEnumerator step(int index, Vector3 moveTo, bool isOpposite)
    {
        // 만약 반대편 다리가 아니면 해당 다리의 반대편 다리를 움직임
        if (!isOpposite)
            moveOppisteLeg(oppositeLeg[index]);

        // 움직일 다리 리스트에서 해당 인덱스 제거
        if (nextIndexToMove.Contains(index))
            nextIndexToMove.Remove(index);

        // 현재 움직이고 있는 다리 리스트에 추가
        if (!IndexMoving.Contains(index))
            IndexMoving.Add(index);

        // 시작 위치 저장
        Vector3 startPos = legOriginalPositions[index];

        // 다리를 부드럽게 들어올리며 이동
        for (int i = 1; i <= LegSmoothness; ++i)
        {
            // Lerp를 사용하여 다리의 위치를 보간하고, Sin 함수를 사용하여 다리를 들어올림
            legTargets[index].transform.position = Vector3.Lerp(
                startPos,
                moveTo + new Vector3(0, Mathf.Sin(i / (float)(LegSmoothness + spiderJitterCutOff) * Mathf.PI) * stepHeight, 0),
                (i / LegSmoothness + spiderJitterCutOff)
            );
            // FixedUpdate의 다음 프레임까지 대기
            yield return new WaitForFixedUpdate();
        }

        // 다리의 원래 위치를 새로운 위치로 업데이트
        legOriginalPositions[index] = moveTo;

        // 각 스텝 사이의 대기 시간 처리
        for (int i = 1; i <= waitTimeBetweenEveryStep; ++i)
            yield return new WaitForFixedUpdate();

        // 움직임이 완료되었으므로 리스트에서 제거
        if (IndexMoving.Contains(index))
            IndexMoving.Remove(index);
    }

    // 반대편 다리를 움직이는 함수
    void moveOppisteLeg(int index)
    {
        // 반대편 다리의 목표 위치를 계산
        Vector3 targetPosition = legCubes[index].transform.position
                               + Mathf.Clamp(velocity.magnitude * OverStepMultiplier, 0.0f, 1.5f)
                               * (legCubes[index].transform.position - legTargets[index].transform.position)
                               + velocity * OverStepMultiplier;

        // 반대편 다리의 움직임을 처리하는 코루틴 시작
        StartCoroutine(step(index, targetPosition, true));
    }

    // 몸체의 회전을 제어하는 함수
    void rotateBody()
    {
        // 몸체 회전이 비활성화되어 있으면 함수 종료
        if (!enableBodyRotation) return;

        // 첫 번째와 두 번째 다리 사이의 벡터 계산
        Vector3 v1 = legTargets[0].transform.position - legTargets[1].transform.position;
        // 세 번째와 네 번째 다리 사이의 벡터 계산
        Vector3 v2 = legTargets[2].transform.position - legTargets[3].transform.position;
        // 두 벡터의 외적을 통해 몸체의 새로운 위쪽 방향 벡터 계산
        Vector3 normal = Vector3.Cross(v1, v2).normalized;
        // 이전 몸체의 위쪽 방향과 새로운 방향을 보간하여 부드럽게 회전
        Vector3 up = Vector3.Lerp(lastBodyUp, normal, 1f / (float)(BodySmoothness));
        // 몸체의 up 벡터를 업데이트
        transform.up = up;

        // Rigidbody 컨트롤러를 사용하지 않으면 회전 업데이트
        if (!rigidBodyController)
            transform.rotation = Quaternion.LookRotation(transform.parent.forward, up);

        // 이전 몸체의 위쪽 방향을 현재 값으로 업데이트
        lastBodyUp = transform.up;
    }
}
