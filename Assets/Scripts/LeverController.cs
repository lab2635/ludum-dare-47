using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    public Vector3 axis = Vector3.right;
    public GameObject armPivot;
    public float angle = 0;
    public float minAngle = -30;
    public float maxAngle = 30;
    public float speed = 2f;

    private float currentAngle = 0;

    void Start()
    {
        currentAngle = angle;
    }

    void Update()
    {
        currentAngle = Mathf.Lerp(currentAngle, angle, Time.deltaTime * speed);
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        armPivot.transform.localRotation = Quaternion.AngleAxis(currentAngle, axis);
    }
}
