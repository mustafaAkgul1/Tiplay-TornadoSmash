using System.Collections;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager _instance;

    [Header("General Variables")]
    public float confettiLoopRate;

    [Header("References")]
    public Transform centerTransform;

    [Header("VFX Prefabs")]
    public GameObject levelSuccessConfettiVFX;

    private void Awake()
    {
        _instance = this;
    }

    public void StartConfettiLoop()
    {
        StartCoroutine(ConfettiLoop(centerTransform.position));

    } // SpawnLevelSuccessConfettiVFX()

    IEnumerator ConfettiLoop(Vector3 _pos)
    {
        while (true)
        {
            yield return new WaitForSeconds(confettiLoopRate);

            Vector3 rndPos = new Vector3(Random.Range(_pos.x - 2f, _pos.x + 2f), _pos.y, Random.Range(_pos.z - 2f, _pos.z + 2f));
            Instantiate(levelSuccessConfettiVFX, rndPos, Quaternion.Euler(-90f, 0f, 0f));
        }

    } // ConfettiLoop()

} // class
