using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public GameObject startObject;
    public GameObject endObject;
    private Vector3 initialSize;

    void Start()
    {
        initialSize = transform.localScale;
        UpdateTransformForScale();
    }

    // Update is called once per frame
    void Update()
    {
        if(startObject.transform.hasChanged || endObject.transform.hasChanged)
        {
            UpdateTransformForScale();
        }
    }

    void UpdateTransformForScale()
    {
        float distance =
            Vector3.Distance(startObject.transform.position, endObject.transform.position);
        transform.localScale = new Vector3(transform.localScale.x, distance, initialSize.z);

        Vector3 middlePoint = (startObject.transform.position + endObject.transform.position) / 2f;
        middlePoint.y = transform.position.y;
        transform.position = middlePoint;
    }
}
