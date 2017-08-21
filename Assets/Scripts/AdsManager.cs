using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections;

public class AdsManager : MonoBehaviour
{
	private static AdsManager instance;
	public static AdsManager Instance
	{
		get
		{
			return instance;
		}
	}
#if UNITY_EDITOR
	string adUnitId = "unused";
	string rewardedAdUnitId = "unused";
#elif UNITY_ANDROID
     string adUnitId = "ca-app-pub-6394875470386076/3887432212";
    string rewardedAdUnitId = "ca-app-pub-6394875470386076/2728905973";
#elif UNITY_IPHONE
    string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
    string rewardedAdUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
#else
    string adUnitId = "unexpected_platform";
    string rewardedAdUnitId = "unexpected_platform";
#endif

	public float bannersHeight;
	public RewardBasedVideoAd rewardedVideo;
	public AdRequest request;

	public Action<bool> rewardedVideoCallback;

    void Awake()
	{
		instance = this;
		rewardedVideo = RewardBasedVideoAd.Instance;

		rewardedVideo.OnAdRewarded += RewardedVideo_OnAdRewarded;
		rewardedVideo.OnAdFailedToLoad += RewardedVideo_OnAdFailedToLoad;
		rewardedVideo.OnAdClosed += RewardedVideo_OnAdClosed;
	}

	private void RewardedVideo_OnAdClosed(object sender, System.EventArgs e)
	{
		if (rewardedVideoCallback != null)
		{
			rewardedVideoCallback(false);
		}
		rewardedVideo.LoadAd(request, rewardedAdUnitId);
	}

	private void RewardedVideo_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
	{
		if (rewardedVideoCallback != null)
		{
			rewardedVideoCallback(false);
		}
		rewardedVideo.LoadAd(request, rewardedAdUnitId);
	}

	private void RewardedVideo_OnAdRewarded(object sender, Reward e)
	{
		if (rewardedVideoCallback != null)
		{
			rewardedVideoCallback(true);
		}
		rewardedVideo.LoadAd(request, rewardedAdUnitId);
	}

	void Start()
	{

		// Create a 320x50 banner at the top of the screen.
		BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		// Create an empty ad request.
		request = new AdRequest.Builder()
			.AddTestDevice(AdRequest.TestDeviceSimulator)
			.AddTestDevice("F40B4C8F5DBC58BCA4BBFEAA0A43CEAE")
			.AddTestDevice("418E9BE656ADE8B73BEE91EA639E3AD2")
			.Build();
		// Load the banner with the request.
		bannerView.LoadAd(request);
		bannersHeight = Screen.dpi * AdSize.Banner.Height / (Screen.height * 160);

		StartCoroutine(LoadRewardedVideos());
	}

	public IEnumerator LoadRewardedVideos()
	{
		while (true)
		{
			if (rewardedVideo.IsLoaded())
			{
				yield return new WaitForSeconds(10.0f);
				continue;
			}
			else
			{
				rewardedVideo.LoadAd(request, rewardedAdUnitId);
				while (rewardedVideo.IsLoaded() == false)
				{
					yield return new WaitForSeconds(10.0f);
				}
			}
		}
	}
	
	public void ShowRewardedVideo()
	{
#if UNITY_EDITOR
		RewardedVideo_OnAdRewarded(null, null);
#else
		rewardedVideo.Show();
#endif
	}
}
