using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonobehaviour<UIManager>
{

    [SerializeField] private Text coinText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text scoreText;

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.UpdateCoin, UpdateCoinUI);
        EventDispatcher.Instance.RegisterListener(EventID.LoadLevel, UpdateLevelUI);
        EventDispatcher.Instance.RegisterListener(EventID.UpdateScore, UpdateScoreUI);
        EventDispatcher.Instance.RegisterListener(EventID.LoadLevel, ShowScoreUIWhenEndLevel);
        EventDispatcher.Instance.RegisterListener(EventID.StartPlay, HideScoreUIWhenStartPlay);

        SkipLevel.onClick.AddListener(SkipLevelButtonOnclick);
        TryAgain.onClick.AddListener(TryAgainButtonOnclick);
        x2Coin.onClick.AddListener(x2CoinsButtonOnclick);
        NoThanks.onClick.AddListener(NoThanksButtonOnclick);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.UpdateCoin, UpdateCoinUI);
        EventDispatcher.Instance.RemoveListener(EventID.LoadLevel, UpdateLevelUI);
        EventDispatcher.Instance.RemoveListener(EventID.UpdateScore, UpdateScoreUI);
        EventDispatcher.Instance.RemoveListener(EventID.LoadLevel, ShowScoreUIWhenEndLevel);
        EventDispatcher.Instance.RemoveListener(EventID.StartPlay, HideScoreUIWhenStartPlay);

        SkipLevel.onClick.RemoveAllListeners();
        TryAgain.onClick.RemoveAllListeners();
        x2Coin.onClick.RemoveAllListeners();
        NoThanks.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        UpdateCoinUI();
    }

    private void UpdateCoinUI(object param = null)
    {
        coinText.text = (UserData.CoinsNumber).ToString();
    }

    private void UpdateLevelUI(object param = null)
    {
        levelText.text = $"Level: {UserData.LevelNumber}";
    }

    private void UpdateScoreUI(object param = null)
    {
        Text scoreAmountUI = scoreText.transform.GetChild(0).gameObject.GetComponent<Text>();

        scoreAmountUI.text = UserData.ScoreNumber.ToString();
    }

    private void HideScoreUIWhenStartPlay(object param = null)
    {
        scoreText.gameObject.SetActive(false);
    }

    private void ShowScoreUIWhenEndLevel(object param = null)
    {
        scoreText.gameObject.SetActive(true);
    }

    public GameObject loseGO;
    public GameObject winGO;
    public Button SkipLevel;
    public Button TryAgain;

    public Button x2Coin;
    public Button NoThanks;

    protected override void Awake()
    {
        base.Awake();
    }

    public void ShowPopup(bool isWin)
    {

        EventDispatcher.Instance.PostEvent(EventID.ShowPopup);

        StartCoroutine(EnableButton(isWin));
    }

    private IEnumerator EnableButton(bool isWin)
    {
        yield return new WaitForSeconds(.3f);
        winGO.SetActive(isWin);
        loseGO.SetActive(!isWin);
    }

    public void HidePopup()
    {
        EventDispatcher.Instance.PostEvent(EventID.HidePopup);
        winGO.SetActive(false);
        loseGO.SetActive(false);
    }

    private void SkipLevelButtonOnclick()
    {
        AdsManager.instance.ShowRewardedAds(
            () =>
            {
                GameManager.instance.NextLevel();
            },
            () =>
            {
                Debug.LogWarning("Fail to load rewarded ads");
            },
            () =>
            {
                Debug.LogWarning("Rewarded ads not show completely");
            }
        );
    }

    private void TryAgainButtonOnclick()
    {
        GameManager.instance.LoadLevel();
    }

    private void x2CoinsButtonOnclick()
    {
        AdsManager.instance.ShowRewardedAds(
            () =>
            {
                PlayerBall.instance.CollectCoin(PlayerBall.instance.coinInLevel, false);
                GameManager.instance.NextLevel();
            },
            () =>
            {
                Debug.LogWarning("Fail to load rewarded ads");
            },
            () =>
            {
                NoThanksButtonOnclick();
                Debug.LogWarning("Rewarded ads not show completely");
            }
        );
    }

    private void NoThanksButtonOnclick()
    {
        GameManager.instance.NextLevel();
    }

}
