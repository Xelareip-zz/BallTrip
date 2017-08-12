using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class LevelPieceFactory : MonoBehaviour
{
	private static LevelPieceFactory instance;
	public static LevelPieceFactory Instance
	{
		get
		{
			return instance;
		}
	}

	public List<GameObject> levelPiecesList = new List<GameObject>();
	public Dictionary<string, GameObject> levelPiecesDict = new Dictionary<string, GameObject>();

	public void EditorInit()
	{
		instance = this;
	}

	void Awake()
	{
		instance = this;

		foreach (GameObject levelPiece in levelPiecesList)
		{
			levelPiecesDict.Add(levelPiece.GetComponentInChildren<LevelPiece>().GetPieceName(), levelPiece);
		}
	}

#if UNITY_EDITOR
	public void ReloadLevelPiecesList()
	{
		levelPiecesDict = new Dictionary<string, GameObject>();
		string[] assetsPaths = AssetDatabase.FindAssets("t:GameObject");

		for (int assetIdx = 0; assetIdx < assetsPaths.Length; ++assetIdx)
		{
			GameObject newObject = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assetsPaths[assetIdx]));
			LevelPiece levelPiece = newObject.GetComponentInChildren<LevelPiece>();
            if (levelPiece != null)
			{
				levelPiecesList.Add(newObject);
				levelPiecesDict.Add(levelPiece.GetPieceName(), newObject);
			}
		}
	}
#endif

}
