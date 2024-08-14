using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrapllingHook
{
    public class RopeAction : MonoBehaviour
    {
        // 1. 레이캐스트
        // 2. 라인랜더러
        // 3. 스프링조인트

        public Transform player;
        public Camera cam;
        RaycastHit hit;
        public LayerMask GrapplingObj;
        LineRenderer lr;
        bool OnGrappling = false;
        public Transform gunTip;
        Vector3 spot;
        Quaternion gunRot;

        SpringJoint sj;

        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main;
            lr = GetComponent<LineRenderer>();
            gunRot = gunTip.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                RpoeShoot();
            else if (Input.GetMouseButtonUp(0))
            {
                EndShoot();
            }

            DrawRope();
        }

        void RpoeShoot()
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f, GrapplingObj))
            {
                OnGrappling = true;

                spot = hit.point;
                lr.positionCount = 2;
                lr.SetPosition(0, this.transform.position);
                lr.SetPosition(1, hit.point);

                sj = player.gameObject.AddComponent<SpringJoint>();
                sj.autoConfigureConnectedAnchor = false;
                sj.connectedAnchor = spot;

                float dis = Vector3.Distance(this.transform.position, spot);

                sj.maxDistance = dis * 0.8f; // maxDistance를 조금 더 줄여봅니다.
                sj.minDistance = dis * 0.25f; // minDistance도 적당히 설정
                sj.spring = 100f; // 스프링 강도를 더 높여봅니다.
                sj.damper = 7f; // 감쇠를 조금 더 줄입니다.
                sj.massScale = 5f;
            }
        }

        void EndShoot()
        {
            OnGrappling = false;
            lr.positionCount = 0;
            //this.transform.rotation = gunRot;
            Destroy(sj);
        }

        void DrawRope()
        {
            if (OnGrappling)
            {
                lr.SetPosition(0, gunTip.position);
                this.transform.LookAt(spot);
            }
        }
    }

}
