using UnityEngine;
using DG.Tweening;

public class LevelCubeVacuumController : MonoBehaviour
{
    [Header("General Variables")]
    public float shrinkSpeed; 
    public float risingSpeed; 
    public float risingRotationSpeed; 
    public float vacuumSpeed; 
    public LevelCubeStates currentState;

    [Header("References")]
    Rigidbody rbCube;

    public enum LevelCubeStates
    {
        OnHold,
        InVacuum,
        OnRise
    }

    private void Start()
    {
        GameManager._instance.cubesList.Add(transform);
        rbCube = GetComponent<Rigidbody>();

    } // Start()

    private void Update()
    {
        switch (currentState)
        {
            case LevelCubeStates.OnHold:
                break;
            case LevelCubeStates.InVacuum:
                HandleInVacuum();
                break;
            case LevelCubeStates.OnRise:
                HandleRising();
                break;
            default:
                break;
        }

    } // Update()

    void HandleInVacuum()
    {
        transform.Rotate(new Vector3(0f, (risingRotationSpeed / 2f) * Time.deltaTime, (risingRotationSpeed / 2f ) * Time.deltaTime), Space.Self);

    } // HandleInVacuum()

    void HandleRising()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (risingSpeed * Time.deltaTime), transform.position.z);
        transform.Rotate(new Vector3(0f, risingRotationSpeed * Time.deltaTime, risingRotationSpeed * Time.deltaTime), Space.Self);

    } // HandleRising()

    void TriggerShrink()
    {
        transform.DOScale(0.03f, shrinkSpeed).SetDelay(0.1f).OnComplete(() =>
        {
            UpdateCubeListFromGameManager();
            Destroy(gameObject);
        });

    } // TriggerShrink()

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TornadoInnerTrigger"))
        {
            if (currentState != LevelCubeStates.OnRise)
            {
                currentState = LevelCubeStates.OnRise;
                transform.parent = other.transform;
                rbCube.isKinematic = true;
                TriggerShrink();
            }
        }

    } // OnTriggerEnter()

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("TornadoVacuumCollider"))
        {
            if (currentState != LevelCubeStates.OnRise)
            {
                currentState = LevelCubeStates.InVacuum;

                Vector3 dir = (other.GetComponent<TornadoVacuumCollController>().centerTornadoTransform.position - transform.position).normalized;
                rbCube.AddForce(dir * vacuumSpeed * Time.deltaTime);
            }
        }

    } // OnTriggerStay()

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TornadoVacuumCollider"))
        {
            if (currentState != LevelCubeStates.OnRise)
            {
                currentState = LevelCubeStates.OnHold;
            }
        }

    } // OnTriggerExit()

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Killzone"))
        {
            UpdateCubeListFromGameManager();
            Destroy(gameObject);
        }

    } // OnCollisionEnter()

    void UpdateCubeListFromGameManager()
    {
        GameManager._instance.cubesList.RemoveAt(GameManager._instance.cubesList.IndexOf(transform));
        GameManager._instance.CheckGameWinning();

    } // UpdateCubeListFromGameManager()

} // class
