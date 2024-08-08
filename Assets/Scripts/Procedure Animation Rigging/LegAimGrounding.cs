using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAimGrounding : MonoBehaviour
{
    int layerMask;
    GameObject raycastOrigin;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
        raycastOrigin = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(raycastOrigin.transform.position , -transform.up, out hit, layerMask))
        {
            transform.position = hit.point + new Vector3(0f, 0.2f, 0f);
        }
    }
}
