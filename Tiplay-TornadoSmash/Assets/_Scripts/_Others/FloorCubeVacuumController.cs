using DG.Tweening;
using UnityEngine;

public class FloorCubeVacuumController : MonoBehaviour
{
    [Header("General Variables")]
    public float shrinkTime;
    bool isRising = false;

    void TriggerRising()
    {
        float dist = Vector3.Distance(transform.localPosition, new Vector3(0f, transform.localPosition.y, 0f));
        if (dist <= 0.001f)
        {
            dist = 0.5f;
        }

        transform.DOLocalMove(new Vector3(0f, transform.localPosition.y + 2f, 0f), 5f / dist);

        transform.DOScale(0.03f, shrinkTime).OnComplete(() => {

            Destroy(gameObject);
        });

    } // TriggerRising()

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TornadoInnerTrigger"))
        {
            if (GameManager._instance.isGameFinished)
            {
                if (!isRising)
                {
                    isRising = true;
                    transform.parent = other.transform;
                    TriggerRising();

                    MoreMountains.NiceVibrations.MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
                }
            }
        }

    } // OnTriggerEnter()

} // class
