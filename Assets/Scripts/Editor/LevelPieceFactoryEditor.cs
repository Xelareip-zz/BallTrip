#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(LevelPieceFactory))]
public class LevelPieceFactoryEditor : Editor
{

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		LevelPieceFactory myTarget = target as LevelPieceFactory;

		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("Level pieces (" + myTarget.levelPiecesDict.Count + "):");
		EditorRenderers.RenderDictionary(myTarget.levelPiecesDict);
		EditorGUILayout.EndVertical();
	}
}
#endif