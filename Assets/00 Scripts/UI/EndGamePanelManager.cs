using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanelManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    public GameObject backGround;
    public GameObject loseGO;
    public GameObject winGO;
    public Button SkipLevel;
    public Button TryAgain;

    public Button x2Coin;
    public Button NoThanks;

    private void OnEnable()
    {
        SkipLevel.onClick.AddListener(SkipLevelButtonOnclick);

        TryAgain.onClick.AddListener(TryAgainButtonOnclick);

        x2Coin.onClick.AddListener(x2CoinsButtonOnclick);

        NoThanks.onClick.AddListener(NoThanksButtonOnclick);
    }

    private void OnDisable()
    {
        SkipLevel.onClick.RemoveAllListeners();

        TryAgain.onClick.RemoveAllListeners();

        x2Coin.onClick.RemoveAllListeners();

        NoThanks.onClick.RemoveAllListeners();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowPopup(bool isWin)
    {

        backGround.SetActive(true);

        StartCoroutine(EnableButton(isWin));

        GameObject activeGO = isWin ? winGO : loseGO;
        foreach (Animation a in activeGO.transform.GetComponentsInChildren<Animation>())
        {
            Debug.LogWarning("Play anim: " + a.name);
            a.Play();
        }
    }

    IEnumerator EnableButton(bool isWin)
    {
        yield return new WaitForSeconds(.3f);
        winGO.SetActive(isWin);
        loseGO.SetActive(!isWin);
    }

    public void HidePopup()
    {
        backGround.SetActive(false);
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
                GameManager.instance.SetPlayerCoin();
                GameManager.instance.SetPlayerCoin();
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
        GameManager.instance.SetPlayerCoin();
        GameManager.instance.NextLevel();
    }

}
