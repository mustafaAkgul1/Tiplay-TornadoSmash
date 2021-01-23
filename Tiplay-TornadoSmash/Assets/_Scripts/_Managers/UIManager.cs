using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;

    [Header("UI Element References")]
    public GameObject tryAgainButton;
    public GameObject nextLevelButton;
    public GameObject slideTutorialObject;
    public GameObject completeTextObject;

    private void Awake()
    {
        _instance = this;
    }

    public void TriggerCompleteText()
    {
        completeTextObject.SetActive(true);

        TextMeshProUGUI completeText = completeTextObject.GetComponent<TextMeshProUGUI>();

        DOTween.To(() => completeText.fontSize, x => completeText.fontSize = x, 75f, 1f).SetEase(Ease.OutBounce).OnComplete(() => {

            DOTween.To(() => completeText.fontSize, x => completeText.fontSize = x, 1f, 0.75f).SetDelay(1f).OnComplete(() =>
            {
                completeTextObject.SetActive(false);
            });
        });

    } // TriggerCompleteText()

    public void TryAgainButtonPressed()
    {
        GameManager._instance.SceneLoadTrigger();

    } // TryAgainButtonPressed()

    public void NextLevelButtonPressed()
    {
        GameManager._instance.SceneLoadTrigger();

    } // NextLevelButtonPressed()

    public void TriggerLevelFailed()
    {
        tryAgainButton.SetActive(true);

    } // TriggerLevelFailed()

    public void TriggerLevelSuccesed()
    {
        nextLevelButton.SetActive(true);

    } // TriggerLevelSuccesed()

    public void TriggerSlideStarted()
    {
        slideTutorialObject.SetActive(false);

    } // TriggerSlideStarted()


} // class
