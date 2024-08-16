using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class ClikMove : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;

    public Transform spot;

    LineRenderer lr;
    Coroutine draw;

    public Transform nms;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material.color = Color.green;
        lr.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                agent.SetDestination(hit.point);
                anim.SetFloat("Speed", 2.0f);
                anim.SetFloat("MotionSpeed", 2.0f);

                spot.gameObject.SetActive(true);
                spot.position = hit.point;

                if(draw != null) StopCoroutine(draw);
                draw =  StartCoroutine(DrawPath());
            }
        }
        else if (agent.remainingDistance < 0.1f)
        {
            anim.SetFloat("Speed", 0f);
            anim.SetFloat("MotionSpeed", 0f);

            spot.gameObject.SetActive(false);

            lr.enabled = false;
            if (draw != null)
                StopCoroutine(draw);
        }
    }

    void CheckDistance()
    {
        if(Vector3.Distance(this.transform.position, nms.position) > 10f)
        {
            nms.transform.position = this.transform.position;
            nms.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }

    IEnumerator DrawPath()
    {
        lr.enabled = true;
        yield return null;
        while (true)
        {
            int cnt = agent.path.corners.Length;
            lr.positionCount= cnt;
            for (int i = 0; i < cnt; i++)
            {
                lr.SetPosition(i, agent.path.corners[i]);
            }
           yield return null;
        }
    }
}
