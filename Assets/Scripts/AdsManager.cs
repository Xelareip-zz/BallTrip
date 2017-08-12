using UnityEngine;
using GoogleMobileAds.Api;

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
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-6394875470386076/3887432212";
#elif UNITY_IPHONE
        string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
#else
        string adUnitId = "unexpected_platform";
#endif

	public float adsHeight;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		// Create a 320x50 banner at the top of the screen.
		BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder()
			.AddTestDevice(AdRequest.TestDeviceSimulator)
			.AddTestDevice("F40B4C8F5DBC58BCA4BBFEAA0A43CEAE")
			.Build();
		// Load the banner with the request.
		bannerView.LoadAd(request);

		adsHeight = AdSize.Banner.Height / (float)Screen.height * 2;
	}
}
