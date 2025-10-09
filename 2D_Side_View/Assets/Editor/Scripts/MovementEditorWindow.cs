#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class MovementForceCreatorWindow : EditorWindow
{
    private string newForceName = "NewMovementForce";
    private int selectedTypeIndex = 0;

    private string[] forceOptions = new string[]
    {
        "Continuous Rigidbody",
        "Triggered Rigidbody",
        "Charged Rigidbody",
        "Continuous Transform",
        "Triggered Transform",
        "Charged Transform"
    };

    [MenuItem("Tools/Movement System/Force Creator")]
    public static void ShowWindow() => GetWindow<MovementForceCreatorWindow>("Force Creator");

    private void OnGUI()
    {
        GUILayout.Label("Create MovementForce Assets", EditorStyles.boldLabel);
        newForceName = EditorGUILayout.TextField("Asset Name", newForceName);

        selectedTypeIndex = EditorGUILayout.Popup("Force Type", selectedTypeIndex, forceOptions);

        if (GUILayout.Button("Create MovementForce"))
        {
            CreateMovementForceAsset(forceOptions[selectedTypeIndex], newForceName);
        }
    }

    private void CreateMovementForceAsset(string type, string name)
    {
        MovementForceSO asset = null;
        string path = $"Assets/MovementForces/{name}.asset";

        switch (type)
        {
            case "Continuous Rigidbody":
                asset = ScriptableObject.CreateInstance<ContinuousRigidbodyMovement>();
                break;
            case "Triggered Rigidbody":
                asset = ScriptableObject.CreateInstance<TriggeredRigidbodyMovement>();
                break;
            case "Charged Rigidbody":
                asset = ScriptableObject.CreateInstance<ChargedRigidbodyMovement>();
                break;
            case "Continuous Transform":
                asset = ScriptableObject.CreateInstance<ContinuousTransformMovement>();
                break;
            case "Triggered Transform":
                asset = ScriptableObject.CreateInstance<TriggeredTransformMovement>();
                break;
            case "Charged Transform":
                asset = ScriptableObject.CreateInstance<ChargedTransformMovement>();
                break;
        }

        if (asset != null)
        {
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
        else
        {
            Debug.LogError("Failed to create MovementForce asset.");
        }
    }
}
#endif

