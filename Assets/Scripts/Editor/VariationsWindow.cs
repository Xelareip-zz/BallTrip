using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Collections;



public class VariationsWindow : EditorWindow
{
	private GUISkin skin;

	private GameObject visualizerPrefab;

	private List<VariationsVisualizer> visualizers = new List<VariationsVisualizer>();
	private Dictionary<string, Vector2> scrolls = new Dictionary<string, Vector2>();
	private List<GameObject> possibleLevels = new List<GameObject>();
	private double lastUpdate;
	private GameObject currentLevelObject;
	private InfiniteLevel currentLevel;
	private LevelVariationsContainer currentData;
	private Texture2D tex;
	private Vector2 selectedVariation;
	private bool lazyVisUpdate;
	private bool lazyUpdate;

	private bool active;

	private float width;
	private float height;
	private float maxHeight;
	private float variationsHeight;

	[MenuItem("BallTrip/Variations %#v")]
	static void Init()
	{
		VariationsWindow window = (VariationsWindow)GetWindow(typeof(VariationsWindow));
		window.Show();
	}

	void OnEnable()
	{
		lastUpdate = 0;
		UpdateVisualizers();
		EditorApplication.update += Update;
	}

	private Vector2 GetScroll(string key)
	{
		if (scrolls.ContainsKey(key) == false)
		{
			scrolls.Add(key, Vector2.zero);
		}
		return scrolls[key];
	}

	private void SetScroll(string key, Vector2 val)
	{
		if (scrolls.ContainsKey(key) == false)
		{
			scrolls.Add(key, val);
		}
		scrolls[key] = val;
	}

	void Update()
	{
		if (active == false || EditorApplication.timeSinceStartup - lastUpdate < 0.5f) 
		{
			return;
		}
		lastUpdate = EditorApplication.timeSinceStartup;
		foreach (var vis in visualizers)
		{
			if (vis != null)
			{
				vis.ManualUpdate();
			}
		}
		FindLevels();
		Repaint();
	}

	void LoadData()
	{
		if (visualizerPrefab == null)
		{
			visualizerPrefab = Resources.Load<GameObject>("Visualizer");
		}
		if (tex == null)
		{
			tex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/VariationSelected.png");
		}
	}

	private void InitSkin()
	{
		if (skin == null)
		{
			skin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/EditorSkin/XelareipSkin.guiskin");
		}
		GUI.skin = skin;
	}

	private void FindLevels()
	{
		possibleLevels.Clear();
		string[] assetsPaths = AssetDatabase.FindAssets("t:GameObject");

		for (int assetIdx = 0; assetIdx < assetsPaths.Length; ++assetIdx)
		{
			GameObject newObject = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assetsPaths[assetIdx]));
			InfiniteLevel level = newObject.GetComponentInChildren<InfiniteLevel>();
			if (level != null)
			{
				possibleLevels.Add(newObject);
			}
		}
	}

	private void DestroyVisualizers()
	{
		visualizers.Clear();

		VariationsVisualizer[] visInScene = FindObjectsOfType<VariationsVisualizer>();
		for (int visIdx = 0; visIdx < visInScene.Length; ++visIdx)
		{
			DestroyImmediate(visInScene[visIdx].gameObject);
		}
	}

	private VariationsVisualizer GetVisualizer(int layer, int variation)
	{
		int currentCount = 0;
		for (int layerIdx = 0; layerIdx < currentData.variationsLevels.Count; ++layerIdx)
		{
			for (int varIdx = 0; varIdx < currentData.variationsLevels[layerIdx].Count; ++varIdx)
			{
				if (layerIdx == layer && varIdx == variation)
				{
					return visualizers[currentCount];
				}
				++currentCount;

			}
		}
		return visualizers[0];
	}

	private void UpdateVisualizers()
	{
		if (currentLevel == null)
		{
			return;
		}

		int currentCount = 0;
		for (int layerIdx = 0; layerIdx < currentData.variationsLevels.Count; ++layerIdx)
		{
			for (int varIdx = 0; varIdx < currentData.variationsLevels[layerIdx].Count; ++varIdx)
			{
				++currentCount;
			}
		}


		while (visualizers.Contains(null))
		{
			visualizers.Remove(null);
        }
		while (visualizers.Count > currentCount)
		{
			visualizers.RemoveAt(visualizers.Count - 1);
		}
		while (visualizers.Count < currentCount)
		{
			visualizers.Add(InstantiateVisualizer());
		}

		currentCount = 0;
		for (int layerIdx = 0; layerIdx < currentData.variationsLevels.Count; ++layerIdx)
		{
			for (int varIdx = 0; varIdx < currentData.variationsLevels[layerIdx].Count; ++varIdx)
			{
				visualizers[currentCount].variationPosition = new Vector2(layerIdx, varIdx);
				++currentCount;
			}
		}
	}

	private Rect MakeRect(float rectWidth, float rectHeight, bool newLine = false)
	{
		Rect res = new Rect(width, height, rectWidth, rectHeight);
		width += rectWidth + 3;
		maxHeight = Mathf.Max(height + rectHeight + 5, maxHeight);
		if (newLine)
		{
			width = 0;
			height = maxHeight;
		}
        return res;
	}

	public void SetActive(bool newActive)
	{
		if (newActive == false)
		{
			DestroyVisualizers();
		}
		else
		{
			UpdateVisualizers();
		}
		active = newActive;
	}
	
	void OnGUI()
	{
		LoadData();
		if (lazyUpdate)
		{
			Update();
			lazyUpdate = false;
		}
		height = 2;
		maxHeight = 0;
		width = 0;

		EditorGUI.BeginChangeCheck();
		bool tempActive = EditorGUI.Toggle(MakeRect(20, 20), active);
		EditorGUI.LabelField(MakeRect(150, 20), "Activate page");

		if (EditorGUI.EndChangeCheck())
		{
			SetActive(tempActive);
		}
		if (tempActive == false)
		{
			return;
		}
		UpdateVisualizers();
		MakeRect(0, 0, true);

		int idx = possibleLevels.IndexOf(currentLevelObject);
		List<string> names = new List<string>();
		names.Add("None");
		foreach (var level in possibleLevels)
		{
			names.Add(level.name);
		}
		EditorGUI.BeginChangeCheck();
		int newIndex = EditorGUI.Popup(MakeRect(300, 20), "Current level", idx + 1, names.ToArray());

		if (EditorGUI.EndChangeCheck())
		{
			if (newIndex > 0)
			{
				currentLevelObject = possibleLevels[newIndex - 1];
				currentLevel = currentLevelObject.GetComponent<InfiniteLevel>();
				currentData = currentLevel.variationsData;
				DestroyVisualizers();
				UpdateVisualizers();
			}
			else
			{
				currentLevel = null;
				currentLevelObject = null;
				currentData = null;
				UpdateVisualizers();
			}
		}
		if (currentLevel == null)
		{
			return;
		}
		EditorGUI.BeginChangeCheck();
		if (GUI.Button(MakeRect(200, 17, true), "Add Layer"))
		{
			currentData.variationsLevels.Add(new ListListString());
			EditorUtility.SetDirty(currentLevelObject);
			return;
		}
		if (GUI.Button(MakeRect(200, 50, true), "Force Update"))
		{
			UpdateVisualizers();
			Update();
			return;
		}
		SetScroll("Main", GUI.BeginScrollView(new Rect(0, height, position.width, position.height - height - 17), GetScroll("Main"), new Rect(0, 0, position.width - 17, variationsHeight)));
		int count = 0;
		height = 0;
		maxHeight = 0;
		for (int layerIdx = 0; layerIdx < currentData.variationsLevels.Count; ++layerIdx)
		{
			width = 0;
			if (GUI.Button(MakeRect(100, 20), "Delete Layer"))
			{
				currentData.variationsLevels.RemoveAt(layerIdx);
				GUI.EndScrollView();
				return;
			}
			if (layerIdx != 0 && GUI.Button(MakeRect(100, 20), "Move up"))
			{
				var temp = currentData.variationsLevels[layerIdx];
				currentData.variationsLevels[layerIdx] = currentData.variationsLevels[layerIdx - 1];
				currentData.variationsLevels[layerIdx - 1] = temp;

				var tempVis = visualizers[layerIdx];
				visualizers[layerIdx] = visualizers[layerIdx - 1];
				visualizers[layerIdx - 1] = tempVis;
				GUI.EndScrollView();
				return;
			}
			if (layerIdx != currentData.variationsLevels.Count - 1 && GUI.Button(MakeRect(100, 20), "Move down"))
			{
				var temp = currentData.variationsLevels[layerIdx];
				currentData.variationsLevels[layerIdx] = currentData.variationsLevels[layerIdx + 1];
				currentData.variationsLevels[layerIdx + 1] = temp;

				var tempVis = visualizers[layerIdx];
				visualizers[layerIdx] = visualizers[layerIdx + 1];
				visualizers[layerIdx + 1] = tempVis;
				GUI.EndScrollView();
				return;
			}
			currentData.AdaptLayerLevelsSize();
			currentData.layerLevels[layerIdx].mode = (LAYER_LEVEL_MODE) EditorGUI.EnumPopup(MakeRect(150, 20), currentData.layerLevels[layerIdx].mode);
			int potentialSecond = EditorGUI.IntField(MakeRect(150, 20), currentData.layerLevels[layerIdx].value);
			if (currentData.layerLevels[layerIdx].value != potentialSecond)
			{
				if (layerIdx == 0 ||
					currentData.layerLevels[layerIdx].mode == LAYER_LEVEL_MODE.RELATIVE ||
					potentialSecond >= currentData.GetLevelForLayer(layerIdx - 1))
				{
					currentData.layerLevels[layerIdx].value = potentialSecond;
                }
            }
			EditorGUI.LabelField(MakeRect(150, 17, true), currentData.GetLevelForLayer(layerIdx).ToString());

			EditorGUI.LabelField(MakeRect(150, 17, true), "Layer " + layerIdx.ToString());
			DrawLayer(layerIdx);
			++count;
		}
		variationsHeight = height;
		GUI.EndScrollView();
		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(currentData);
		}
	}

	private void DrawLayer(int layerIdx)
	{
		ListListString variations = currentData.variationsLevels[layerIdx];

		float variationWidth = 132;
		float variationHeight = 260;
		float textureBorder = 2;
		float editionWidth = 100;
		bool selectedLayer = selectedVariation.x == layerIdx;

		float currentOffset = 0;
		float contentWidth = variations.Count * variationWidth + 10 + (selectedLayer ? editionWidth : 0);
        SetScroll("layer" + layerIdx, GUI.BeginScrollView(MakeRect(Mathf.Min(position.width - 50, contentWidth), variationHeight + 17), GetScroll("layer" + layerIdx), new Rect(0, 0, contentWidth + 22, variationHeight)));
		for (int variationIdx = 0; variationIdx < variations.Count; ++variationIdx)
		{
			if (Event.current != null && Event.current.type == EventType.MouseDown)
			{
				if (Event.current.mousePosition.x > currentOffset + GetScroll("layer" + layerIdx).x &&
					Event.current.mousePosition.x < (currentOffset + GetScroll("layer" + layerIdx).x + variationWidth + (selectedVariation == new Vector2(layerIdx, variationIdx) ? editionWidth : 0)) &&
					Event.current.mousePosition.y > 0 &&
					Event.current.mousePosition.y < variationHeight)
				{
					Debug.Log("Selected " + variationIdx);
					selectedVariation = new Vector2(layerIdx, variationIdx);
                }
			}

			VariationsVisualizer visualizer = GetVisualizer(layerIdx, variationIdx);

			if (selectedVariation == new Vector2(layerIdx, variationIdx))
			{
				EditorGUI.DrawPreviewTexture(new Rect(currentOffset, 0, variationWidth, variationHeight), tex);
			}
			EditorGUI.DrawPreviewTexture(new Rect(currentOffset + textureBorder, 2, variationWidth - 4, variationHeight), visualizer.renderTexture);
			
			currentOffset += variationWidth;
			
			if (selectedVariation == new Vector2(layerIdx, variationIdx))
			{
				if (GUI.Button(new Rect(currentOffset, 2, 20, 20), "-"))
				{
					variations.RemoveAt(variationIdx);
					lazyVisUpdate = true;
					return;
				}
				IGUIDIdentified[] guids = currentLevelObject.GetComponentsInChildren<IGUIDIdentified>(true);
				SetScroll("selected", GUI.BeginScrollView(new Rect(currentOffset, 20, editionWidth, variationHeight - 22), GetScroll("selected"), new Rect(0, 0, editionWidth - 17, guids.Length * 17)));
				float guidHeight = 2;
				for (int guidPos = 0; guidPos < guids.Length; ++guidPos)
				{
					IGUIDIdentified guidHolder = guids[guidPos];
					if (Event.current != null && Event.current.type == EventType.MouseDown)
					{
						if (Event.current.mousePosition.x > 0 &&
							Event.current.mousePosition.x < variationWidth - 4 &&
							Event.current.mousePosition.y > guidHeight &&
							Event.current.mousePosition.y < guidHeight + variationHeight)
						{
							VariationsVisualizer.selectedGUID = guidHolder.GetGUID();
						}
					}

					if (VariationsVisualizer.selectedGUID == guidHolder.GetGUID())
					{
						if (Event.current != null && Event.current.keyCode == KeyCode.DownArrow && Event.current.type == EventType.KeyDown)
						{
							VariationsVisualizer.selectedGUID = guids[(guids.Length + guidPos + 1) % guids.Length].GetGUID();
							GUI.EndScrollView();
							GUI.EndScrollView();
							return;
                        }
						else if (Event.current != null && Event.current.keyCode == KeyCode.UpArrow && Event.current.type == EventType.KeyDown)
						{
							VariationsVisualizer.selectedGUID = guids[(guids.Length + guidPos - 1) % guids.Length].GetGUID();
							GUI.EndScrollView();
							GUI.EndScrollView();
							return;
						}
					}
					if (VariationsVisualizer.selectedGUID == guidHolder.GetGUID())
					{
						EditorGUI.DrawPreviewTexture(new Rect(0, guidHeight, variationWidth - 4, 15), tex);
					}

					EditorGUI.BeginChangeCheck();
					bool obstacleVisible = variations[variationIdx].Contains(guidHolder.GetGUID());
					bool newVisibility = EditorGUI.Toggle(new Rect(0, guidHeight, 17f, 17f), obstacleVisible);
					if (EditorGUI.EndChangeCheck())
					{
						if (newVisibility)
						{
							variations[variationIdx].Add(guidHolder.GetGUID());
                        }
						else
						{
							variations[variationIdx].Remove(guidHolder.GetGUID());
						}
						lazyUpdate = true;
					}
					EditorGUI.LabelField(new Rect(20, guidHeight, editionWidth - 20, 20), (guidHolder as MonoBehaviour).gameObject.name);
					guidHeight += 17;
				}
				GUI.EndScrollView();
			}

			if (selectedVariation == new Vector2(layerIdx, variationIdx))
			{
				currentOffset += editionWidth;
			}
		}
		GUI.EndScrollView();
		if (GUI.Button(MakeRect(30, variationHeight), "+"))
		{
			variations.Add(new ListString());
			selectedVariation = new Vector2(layerIdx, variations.Count - 1);
			lazyUpdate = true;
            return;
		}
		MakeRect(0, 0, true);
	}

	private VariationsVisualizer InstantiateVisualizer()
	{
		GameObject go = Instantiate<GameObject>(visualizerPrefab);
		VariationsVisualizer vis = go.GetComponent<VariationsVisualizer>();
		vis.SetLevel(currentLevelObject);
        return vis;
	}
}