using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotation : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject character;
    float speed = 5f;
    Vector3 rot;
    Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        rot = character.transform.localEulerAngles;
        origin = rot;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rot.y += Input.GetAxis("Mouse X") * speed;
        character.transform.localEulerAngles = -rot;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 시작");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        character.transform.localEulerAngles = origin;
    }
}
