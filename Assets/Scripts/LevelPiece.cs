using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPiece : MonoBehaviour
{
	public static List<LevelPiece> levelPieces = new List<LevelPiece>();

	public string pieceName;

	protected virtual void VirtualAwake()
	{
		levelPieces.Add(this);
	}

	protected virtual void VirtualOnDestroy()
	{
		levelPieces.Add(this);
	}

	void Awake()
	{
		VirtualAwake();
	}

	void OnDestroy()
	{
		VirtualOnDestroy();
	}

	public virtual string GetPieceName()
	{
		return pieceName;
	}
}
