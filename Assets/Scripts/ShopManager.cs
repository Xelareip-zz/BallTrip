using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
	public Text coinsText;
	public Text launchesText;
	public Text viewText;
	public Text bouncesText;

	public Text launchesPriceText;
	public Text viewPriceText;
	public Text bouncesPriceText;

	public List<int> pricesLaunches;
	public List<int> pricesBounces;
	public List<int> pricesView;

	void Update()
	{
		coinsText.text = "" + Player.Instance.GetCoins();
		launchesText.text = "" + Player.Instance.GetLaunches();
		viewText.text = "" + Player.Instance.GetViewRange();
		bouncesText.text = "" + Player.Instance.GetBounces();

		
		launchesPriceText.text = "" + GetFriendlyPrice(GetLaunchPrice());
		viewPriceText.text = "" + GetFriendlyPrice(GetViewPrice());
		bouncesPriceText.text = "" + GetFriendlyPrice(GetBouncePrice());
	}

	public int GetLaunchPrice()
	{
		int nextLaunchCapacity = Player.Instance.GetLaunches() + 1;
		if (nextLaunchCapacity > pricesLaunches.Count)
		{
			return int.MaxValue;
		}
		return pricesLaunches[nextLaunchCapacity];
	}

	public string GetFriendlyPrice(int val)
	{
		if (val == int.MaxValue)
		{
			return "Max";
		}
		return val.ToString();
	}

	public int GetBouncePrice()
	{
		int nextBounceCapacity = Player.Instance.GetBounces() + 1;
		if (nextBounceCapacity > pricesBounces.Count)
		{
			return int.MaxValue;
		}
		return pricesBounces[nextBounceCapacity];
	}

	public int GetViewPrice()
	{
		int nextViewCapacity = Player.Instance.GetViewRange() + 1;
		if (nextViewCapacity > pricesView.Count)
		{
			return int.MaxValue;
		}
		return pricesView[nextViewCapacity];
	}

	public void BuyLaunch()
	{
		int launchPrice = GetLaunchPrice();
		if (Player.Instance.CanBuy(launchPrice))
		{
			Player.Instance.AddCoins(-launchPrice);
			Player.Instance.SetLaunchesAllowed(Player.Instance.GetLaunches() + 1);
        }
	}

	public void BuyView()
	{
		int viewPrice = GetViewPrice();
		if (Player.Instance.CanBuy(viewPrice))
		{
			Player.Instance.AddCoins(-viewPrice);
			Player.Instance.SetViewRange(Player.Instance.GetViewRange() + 1);
		}
	}

	public void BuyBounce()
	{
		int bouncePrice = GetBouncePrice();
		if (Player.Instance.CanBuy(bouncePrice))
		{
			Player.Instance.AddCoins(-bouncePrice);
			Player.Instance.SetBounces(Player.Instance.GetBounces() + 1);
		}
	}

	public void ResetProgress()
	{
		Player.Instance.SetLaunchesAllowed(1);
		Player.Instance.SetViewRange(1);
		Player.Instance.SetBounces(1);
		Player.Instance.AddCoins(-Player.Instance.GetCoins());
		Player.Instance.ResetLevels();
	}
}
