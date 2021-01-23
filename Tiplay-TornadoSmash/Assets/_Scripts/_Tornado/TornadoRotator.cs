using UnityEngine;

public class TornadoRotator : MonoBehaviour
{
    [Header("General Variables")]
    public float rotateSpeed;

    [Header("References")]
    public Transform centerTransformToRotate;

    void Update()
    {
        centerTransformToRotate.Rotate(new Vector3(0f, rotateSpeed * Time.deltaTime, 0f));

    } // Update()

} // class
