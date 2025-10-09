#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class InputCreatorWindow : EditorWindow
{
    private string newInputName = "NewInput";

    [MenuItem("Tools/Input System/Input Creator")]
    public static void ShowWindow() => GetWindow<InputCreatorWindow>("Input Creator");

    private void OnGUI()
    {
        GUILayout.Label("Create Input Assets", EditorStyles.boldLabel);
        newInputName = EditorGUILayout.TextField("Input Name", newInputName);

        if (GUILayout.Button("Create External Input"))
        {
            CreateInputAsset<ExternalInputSO>(newInputName);
        }

        if (GUILayout.Button("Create Internal Input"))
        {
            CreateInputAsset<InternalInputSO>(newInputName);
        }
    }

    private void CreateInputAsset<T>(string name) where T : BaseInputSO
    {
        string path = $"Assets/Inputs/{name}.asset";
        var asset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
#endif
