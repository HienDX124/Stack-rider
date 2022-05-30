using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdsManager : SingletonMonobehaviour<AdsManager>
{
    [SerializeField] private RewardedAdsButton rewardedAds;
    [SerializeField] private InterstitialAdsButton interstitialAds;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ShowRewardedAds(
                () =>
                {
                    Debug.LogWarning("Rewarded ads show complete");
                },
                () =>
                {
                    Debug.LogWarning("Rewarded ads fail to load");
                },
                () =>
                {
                    Debug.LogWarning("Rewarded ads show failure");
                });
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowInterstitialAds(
                () =>
                {
                    Debug.LogWarning("Interstitial ads show complete");
                },
                () =>
                {
                    Debug.LogWarning("Interstitial ads fail to load");
                },
                () =>
                {
                    Debug.LogWarning("Interstitial ads show failure");
                });
        }
    }

    public void ShowRewardedAds(UnityAction OnShowComplete = null, UnityAction OnFailToLoad = null, UnityAction OnShowFailure = null)
    {
        rewardedAds.ShowAd();

        rewardedAds.OnCompleteAction = null;
        rewardedAds.OnFailedToLoadAction = null;
        rewardedAds.OnShowFailureAction = null;

        rewardedAds.OnCompleteAction += OnShowComplete;
        rewardedAds.OnFailedToLoadAction += OnFailToLoad;
        rewardedAds.OnShowFailureAction += OnShowFailure;
    }

    public void ShowInterstitialAds(UnityAction OnShowComplete = null, UnityAction OnFailToLoad = null, UnityAction OnShowFailure = null)
    {
        interstitialAds.ShowAd();

        interstitialAds.OnCompleteAction = null;
        interstitialAds.OnFailedToLoadAction = null;
        interstitialAds.OnShowFailureAction = null;

        interstitialAds.OnCompleteAction += OnShowComplete;
        interstitialAds.OnFailedToLoadAction += OnFailToLoad;
        interstitialAds.OnShowFailureAction += OnShowFailure;
    }

}
