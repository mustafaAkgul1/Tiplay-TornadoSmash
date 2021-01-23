using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    [Header("General Variables")]
    public bool isGameFinished = false;
    public bool isLevelCleared = false;
    public float levelEndSweepTime;
    public float levelEndSweepCollRadius;
    bool isTutorialActive = true;
    public List<Transform> cubesList = new List<Transform>();

    private void Awake()
    {
        _instance = this;

    } // Awake()

    void Start()
    {
        DOTween.Clear(true);
        DOTween.SetTweensCapacity(350, 5);

    } // Start()

    void Update()
    {
        HandleWinning();
        HandleSlideTutorialUI();

    } // Update()

    void HandleWinning()
    {
        if (!isGameFinished && isLevelCleared)
        {
            isGameFinished = true;
            TornadoMovementController._instance.TriggerLevelFinished();
            SetPlayerPrefSettings();
            SweepLevelFloorCubes();
            StartCoroutine(LevelEndAnimEnded());
        }

    } // HandleWinning()

    void HandleSlideTutorialUI()
    {
        if (isTutorialActive && Input.GetMouseButtonDown(0))
        {
            isTutorialActive = false;
            UIManager._instance.TriggerSlideStarted();
        }

    } // HandleSlideTutorialUI()

    void SetPlayerPrefSettings()
    {
        int tmpCurr = PlayerPrefs.GetInt("CurrentLevel");
        tmpCurr++;
        PlayerPrefs.SetInt("CurrentLevel", tmpCurr);

    } // SetPlayerPrefSettings()

    public void CheckGameWinning()
    {
        EventManager.onCubeDestroyEvent.Invoke();

        if (cubesList.Count <= 0 && !isLevelCleared)
        {
            isLevelCleared = true;
            UIManager._instance.TriggerCompleteText();
            VFXManager._instance.StartConfettiLoop();
            EventManager.onLevelSuccessedEvent.Invoke();
        }

    } // CheckGameWinning()

    void SweepLevelFloorCubes()
    {
        DOTween.To(() => TornadoMovementController._instance.centerCollider.radius, x => TornadoMovementController._instance.centerCollider.radius = x, levelEndSweepCollRadius, levelEndSweepTime);

    } // SweepLevelFloorCubes()

    public void SceneLoadTrigger()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    } // SceneLoadTrigger()


    IEnumerator LevelEndAnimEnded()
    {
        yield return new WaitForSeconds(levelEndSweepTime);

        UIManager._instance.TriggerLevelSuccesed();
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);

    } // LevelEndAnimEnded()

} // class
