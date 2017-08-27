using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LayersLevel
{
	public LayersLevel(LAYER_LEVEL_MODE _mode, int _value)
	{
		mode = _mode;
		value = _value;
	}
	
	public LAYER_LEVEL_MODE mode;
	public int value;
};

public enum LAYER_LEVEL_MODE
{
	FIXED,
	RELATIVE
}

[CreateAssetMenu]
[System.Serializable]
public class LevelVariationsContainer : ScriptableObject
{
	public GameObject linkedPrefab;

	[SerializeField]
	public List<LayersLevel> layerLevels = new List<LayersLevel>();

	[SerializeField]
	public List<ListListString> variationsLevels = new List<ListListString>();
	

	public void AdaptLayerLevelsSize()
	{
		while (layerLevels.Count > variationsLevels.Count)
		{
			layerLevels.RemoveAt(layerLevels.Count - 1);
		}
		while (layerLevels.Count < variationsLevels.Count)
		{
			layerLevels.Add(new LayersLevel(LAYER_LEVEL_MODE.RELATIVE, 1));
		}
	}

	public int GetLayerForLevel(int level)
	{
		int levelIdx = 0;
		int layerIdx = 1;

		for (; layerIdx < layerLevels.Count; ++layerIdx)
		{
			if (layerLevels[layerIdx].mode == LAYER_LEVEL_MODE.FIXED)
			{
				levelIdx = layerLevels[layerIdx].value;
			}
			else
			{
				levelIdx += layerLevels[layerIdx].value;
			}
			if (levelIdx > level)
			{
				break;
			}
        }

		return layerIdx - 1;
	}

	public int GetLevelForLayer(int layer)
	{
		int levelIdx = 0;
		int layerIdx = 0;

		for (; layerIdx < layerLevels.Count && layerIdx <= layer; ++layerIdx)
		{
			if (layerLevels[layerIdx].mode == LAYER_LEVEL_MODE.FIXED)
			{
				levelIdx = layerLevels[layerIdx].value;
			}
			else
			{
				levelIdx += layerLevels[layerIdx].value;
			}
		}

		return levelIdx;
	}
}
