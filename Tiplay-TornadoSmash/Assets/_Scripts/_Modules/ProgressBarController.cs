using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ProgressBarController : MonoBehaviour
{
    float levelCubeCount;
    int destroyedCubeCount;

    [Header("References")]
    public Image filledBar;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    public Image nextLevelCircle;

    private void OnEnable()
    {
        EventManager.onCubeDestroyEvent.AddListener(ChangeFillAmount);
        EventManager.onLevelSuccessedEvent.AddListener(TriggerLevelSuccessed);

    } // OnEnable()

    private void Start()
    {
        InitProgressBar();

    } // Start()

    void InitProgressBar()
    {
        levelCubeCount = GameManager._instance.cubesList.Count;
        destroyedCubeCount = 0;
        filledBar.fillAmount = (destroyedCubeCount / levelCubeCount);

        currentLevelText.text = (PlayerPrefs.GetInt("CurrentLevel") + 1).ToString();
        nextLevelText.text = (PlayerPrefs.GetInt("CurrentLevel") + 2).ToString();

    } // InitProgressBar()

    void ChangeFillAmount()
    {
        if (DOTween.IsTweening("ProgressBarFillerTween"))
        {
            DOTween.Kill("ProgressBarFillerTween");
        }

        destroyedCubeCount++;
        filledBar.DOFillAmount((destroyedCubeCount / levelCubeCount), 0.1f).SetId("ProgressBarFillerTween");

    } // ChangeFillAmount()

    public void TriggerLevelSuccessed()
    {
        if (DOTween.IsTweening("ProgressBarCircleFillerTween"))
        {
            DOTween.Kill("ProgressBarCircleFillerTween");
        }

        nextLevelCircle.DOFillAmount(1f, 0.5f).SetId("ProgressBarCircleFillerTween");

    } // TriggerLevelSuccessed()

    private void OnDisable()
    {
        EventManager.onCubeDestroyEvent.RemoveListener(ChangeFillAmount);
        EventManager.onLevelSuccessedEvent.RemoveListener(TriggerLevelSuccessed);

    } // OnDisable()

} // class
