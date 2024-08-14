using UnityEngine;

namespace GrapllingHook
{
    public class MouseLook : MonoBehaviour
    {
        public float mouseSensitivity = 100f; // 마우스 감도

        private Transform playerBody;
        private float xRotation = 0f;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 잠금
            playerBody = transform.parent;            // 부모 오브젝트인 플레이어를 참조
        }

        void Update()
        {
            // 마우스 입력 받기
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // 카메라의 x축 회전 제한 (위아래 보기)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 카메라가 완전히 뒤집히지 않도록 제한

            // 카메라의 회전 적용
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // 플레이어의 y축 회전 (좌우 보기)
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

}
