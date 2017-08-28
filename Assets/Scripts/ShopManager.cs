using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
	private static ShopManager instance;
	public static ShopManager Instance
	{
		get
		{
			return instance;
		}
	}

	public Text coinsText;
	public Text launchesText;
	public Text viewText;
	public Text bouncesText;
	public Text priceText;

	public Text launchesPriceText;
	public Text viewPriceText;
	public Text bouncesPriceText;

	public List<int> pricesLaunches;
	public List<int> pricesBounces;
	public List<int> pricesView;

	public ShopButton defaultButton;
	public ShopButton selectedButton;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		TutoManager.Instance.StartTuto("TutoFirstBuy");
		ShopButton[] shopButtons = FindObjectsOfType<ShopButton>();
        foreach (var button in shopButtons)
		{
			if (button.CanBuy())
			{
				SelectButton(button);
			}
		}
		if (selectedButton == null)
		{
			SelectButton(shopButtons[0]);
		}
	}

	void Update()
	{
		if (selectedButton != null)
		{
			priceText.text = GetBuyablePrice(selectedButton.buyable).ToString();
        }
		else
		{
			priceText.text = "-";
		}

		coinsText.text = "" + Player.Instance.GetCoins();
		launchesText.text = "" + Player.Instance.GetBuyableLevel(BUYABLE.LAUNCH);
		viewText.text = "" + Player.Instance.GetViewRange();
		bouncesText.text = "" + Player.Instance.GetHearts();

		
		launchesPriceText.text = "" + GetFriendlyPrice(GetLaunchPrice());
		viewPriceText.text = "" + GetFriendlyPrice(GetViewPrice());
		bouncesPriceText.text = "" + GetFriendlyPrice(GetBouncePrice());
	}

	public void SelectButton(ShopButton button)
	{
		if (selectedButton != null)
		{
			selectedButton.Unselect();
		}
		if (button != null)
		{
			button.Select();
		}
		selectedButton = button;
	}

	public int GetLaunchPrice()
	{
		int nextLaunchCapacity = Player.Instance.GetBuyableLevel(BUYABLE.LAUNCH) + 1;
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
		int nextBounceCapacity = Player.Instance.GetBuyableLevel(BUYABLE.HEARTS) + 1;
		if (nextBounceCapacity > pricesBounces.Count)
		{
			return int.MaxValue;
		}
		return pricesBounces[nextBounceCapacity];
	}

	public int GetViewPrice()
	{
		int nextViewCapacity = Player.Instance.GetBuyableLevel(BUYABLE.VIEW) + 1;
		if (nextViewCapacity > pricesView.Count)
		{
			return int.MaxValue;
		}
		return pricesView[nextViewCapacity];
	}

	public int GetBuyablePrice(BUYABLE buyable)
	{
		switch(buyable)
		{
			case BUYABLE.HEARTS:
				return GetBouncePrice();
			case BUYABLE.ENERGY:
				return GetLaunchPrice();
			case BUYABLE.VIEW:
				return GetViewPrice();
		}
		return int.MaxValue;
	}

	public void BuySelected()
	{
		if (selectedButton != null)
		{
			selectedButton.BuyButtonClicked();
		}
	}

	public void ResetProgress()
	{
		Player.Instance.Reinit();
		Player.Instance.Save();
    }

	public void StartGame()
	{
		if (TutoManager.Instance.GetCanStartGame())
		{
			SceneManager.LoadScene("InfiniteLevels");
		}
	}
}
