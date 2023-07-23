using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BillboardComponent : MonoBehaviour
{
    private Transform _mainCameraTransform;
    private void Start()
    {
        _mainCameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _mainCameraTransform.position);
    }
}
