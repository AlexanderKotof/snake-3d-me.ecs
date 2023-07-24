using UnityEngine;

public class BillboardComponent : MonoBehaviour
{
    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
    }
}