using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrabLayout : MonoBehaviour, IEndDragHandler, IDragHandler
{
    [SerializeField] float hardness = 10f;
    [SerializeField] Vector3 initialPosition;
    Vector3 targetPosition;

    float scaleFactor;

    public void OnDrag(PointerEventData eventData)
    {
        targetPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0) / scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        targetPosition = initialPosition;
    }
    
    void Start() {
        targetPosition = initialPosition;
    }

    private void Update()
    {
        scaleFactor = FindObjectOfType<Canvas>().scaleFactor;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, hardness * Time.deltaTime);
    }

}
