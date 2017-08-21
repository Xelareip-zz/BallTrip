using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour, IPointerDownHandler
{
	public BUYABLE buyable;
	public string levelMethodSet;
	public string levelMethodGet;
	public int startLevel;
	public GameObject selectedUI;
	public GameObject notifUI;
	public GameObject lockUI;
	public Sprite untickedSprite;
	public Sprite tickedSprite;
	public MethodInfo levelMethodInfoSet;
	public MethodInfo levelMethodInfoGet;
	public Image[] ticks;
	public Selectable selectable;

	void Awake()
	{
		selectable = GetComponent<Selectable>();

		Type thisType = typeof(Player);
		levelMethodInfoGet = thisType.GetMethod(levelMethodGet);
		levelMethodInfoSet = thisType.GetMethod(levelMethodSet);
		FindTicks();
	}

	void Update()
	{
		if (lockUI != null)
		{
			lockUI.SetActive(Player.Instance.BuyableUnlocked(buyable) == false);
		}
		int level = Player.Instance.GetBuyableLevel(buyable);
		level -= startLevel;
		for (int tickIdx = 0; tickIdx < ticks.Length; ++tickIdx)
		{
			ticks[tickIdx].sprite = level <= tickIdx ? untickedSprite : tickedSprite;
		}
		notifUI.SetActive(CanBuy());
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		if (Player.Instance.BuyableUnlocked(buyable) == false)
		{
			return;
		}

		if (ShopManager.Instance.selectedButton == this)
		{
			ShopManager.Instance.SelectButton(null);
		}
		else
		{
			ShopManager.Instance.SelectButton(this);
		}
	}

	public void Select()
	{
		selectedUI.SetActive(true);
	}

	public void Unselect()
	{
		selectedUI.SetActive(false);
    }

	public bool CanBuy()
	{
		int price = ShopManager.Instance.GetBuyablePrice(buyable);
		return Player.Instance.CanBuy(price);
    }

	public bool BuyButtonClicked()
	{
		if (CanBuy())
		{
            Player.Instance.AddCoins(-ShopManager.Instance.GetBuyablePrice(buyable));
			Player.Instance.SetBuyableLevel(buyable, Player.Instance.GetBuyableLevel(buyable) + 1);
			return true;
		}
		return false;
	}

	public void FindTicks()
	{
		int maxLevel = 0;
		List<Image> ticksFound = new List<Image>();
		Image[] children = GetComponentsInChildren<Image>();
		for (int childIdx = 0; childIdx < children.Length; ++childIdx)
		{
			if (children[childIdx].name.StartsWith("LevelTick"))
			{
				ticksFound.Add(children[childIdx]);
				int level = int.Parse(children[childIdx].name.Replace("LevelTick", ""));
				maxLevel = Mathf.Max(level, maxLevel);
            }
		}
		ticks = new Image[maxLevel + 1];
		for (int childIdx = 0; childIdx < children.Length; ++childIdx)
		{
			if (children[childIdx].name.StartsWith("LevelTick"))
			{
				ticksFound.Add(children[childIdx]);
				int level = int.Parse(children[childIdx].name.Replace("LevelTick", ""));
				ticks[level] = children[childIdx];
            }
		}

	}
}
