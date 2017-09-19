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

	public ShopData data;

	public Text coinsText;
	public Text priceText;
	public Text descriptionText;
	

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
			if (Player.Instance.BuyableUnlocked(button.buyable) && button.CanBuy())
			{
				SelectButton(button);
			}
		}
		if (selectedButton == null)
		{
			SelectButton(null);
		}
	}

	void Update()
	{
		if (selectedButton != null)
		{
			priceText.text = GetPrice(selectedButton.buyable).ToString();
        }
		else
		{
			priceText.text = "-";
		}

		coinsText.text = "" + Player.Instance.GetCoins();
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
			descriptionText.text = button.description;
		}
		else
		{
			descriptionText.text = "";
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

	public int GetPrice(BUYABLE buyable)
	{
		int buyableLevel = Player.Instance.GetBuyableLevel(buyable);
		if (buyableLevel >= data.prices[buyable].Count - 1)
		{
			return int.MaxValue;
		}
		return data.prices[buyable][buyableLevel + 1];
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
	/*
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
	*/
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
