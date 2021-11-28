using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 axis = default;
    [SerializeField] private float speed = default;

    private void Update()
    {
        Vector3 rot = transform.eulerAngles;
        rot += axis * Time.deltaTime * speed;
        transform.rotation = Quaternion.Euler(rot);
    }
}