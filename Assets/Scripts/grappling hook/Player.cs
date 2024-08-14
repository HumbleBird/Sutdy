using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrapllingHook
{
    public class Player : MonoBehaviour
    {
        public float moveSpeed = 5f;        // 이동 속도
        public float jumpForce = 5f;        // 점프의 힘
        public Transform cameraTransform;   // 카메라의 Transform

        private Rigidbody rb;
        private bool isGrounded;

        void Start()
        {
            rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트를 가져옵니다.
        }

        void Update()
        {
            // 지면 체크 - 플레이어의 발 아래에서 레이캐스트를 쏴서 지면을 감지
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

            // 플레이어의 이동 입력 받기 (WASD 또는 화살표 키)
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            // 카메라의 시점에 따른 방향 계산
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // 카메라의 방향에서 수직(y) 성분 제거
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            // 카메라의 방향을 기준으로 이동 벡터 계산
            Vector3 move = forward * moveZ + right * moveX;
            move *= moveSpeed;

            // Rigidbody의 속도를 변경하여 이동을 처리
            Vector3 velocity = rb.velocity;
            velocity.x = move.x;
            velocity.z = move.z;

            // 점프 입력 받기
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = jumpForce; // 점프의 힘을 위로 가하는 방식으로 적용
            }

            // Rigidbody의 속도를 업데이트
            rb.velocity = velocity;
        }
    }
}
