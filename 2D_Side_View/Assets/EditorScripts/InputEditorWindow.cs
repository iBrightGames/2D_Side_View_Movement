// using UnityEditor;
// using UnityEngine;
// using System;

// // Editor klasöründe olmalıdır.
// public class InputMovementBridgeEditor : EditorWindow
// {
//     private InputMovementBridgeSO targetSO;
//     private SerializedObject serializedObject;

//     // Serialized Property'ler
//     private SerializedProperty movementProp;
//     private SerializedProperty inputProp;

//     // Yaratma Modu Alanları
//     private bool isEditMode = false;
//     private string newSOName = "NewInputBridge";
//     private string savePath = "Assets/";

//     // UI
//     private Vector2 scrollPos;
//     private GUIStyle headerStyle;
//     private GUIStyle boxStyle;

//     #region Window Setup
//     [MenuItem("Tools/Movement/Input Bridge Editor")]
//     public static void ShowWindow()
//     {
//         var window = GetWindow<InputMovementBridgeEditor>("Input Bridge Editor");
//         window.minSize = new Vector2(400, 600);
//     }
//     #endregion

//     #region GUI Lifecycle
//     private void OnEnable()
//     {
//         InitializeStyles();
//     }
//     #region GUI Styles
//     private void InitializeStyles()
//     {
//         if (headerStyle == null)
//         {
//             headerStyle = new GUIStyle(EditorStyles.boldLabel)
//             {
//                 fontSize = 14,
//                 margin = new RectOffset(0, 0, 10, 5)
//             };
//         }

//         if (boxStyle == null)
//         {
//             boxStyle = new GUIStyle(EditorStyles.helpBox)
//             {
//                 padding = new RectOffset(10, 10, 10, 10),
//                 margin = new RectOffset(0, 0, 5, 5)
//             };
//         }
//     }
//     #endregion
//     private void OnGUI()
//     {
//         // 1. Serileştirilmiş objeyi ve stilini güncelle
//         if (serializedObject != null)
//         {
//             serializedObject.Update();
//         }

//         InitializeStyles();

//         scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

//         EditorGUILayout.Space(10);
//         DrawModeSelection();
//         EditorGUILayout.Space(10);

//         // Düzenleme veya Oluşturma Arayüzü
//         if (isEditMode)
//         {
//             if (targetSO != null)
//             {
//                 DrawSerializedDataConfiguration();
//             }
//             else
//             {
//                 EditorGUILayout.HelpBox("Lütfen düzenlemek istediğiniz bir ScriptableObject seçin.", MessageType.Info);
//             }
//         }
//         else // Oluşturma Modu
//         {
//             // Create modunda, geçici bir SO üzerinde çalışmak için bu yapıyı kullanırız.
//             DrawCreationDataConfiguration();
//         }

//         EditorGUILayout.Space(10);
//         DrawSaveSection();
//         EditorGUILayout.Space(10);

//         EditorGUILayout.EndScrollView();

//         // 2. Değişiklikleri kaydet (Edit modunda)
//         if (serializedObject != null && isEditMode)
//         {
//             serializedObject.ApplyModifiedProperties();
//             // Bu, SO'yu otomatik olarak kirli (dirty) işaretler ve Unity kaydeder.
//         }
//     }
//     #endregion

//     #region Mode/SO Management
//     private void DrawModeSelection()
//     {
//         // ... (Bu kısım temelde aynı kalır)
//         EditorGUILayout.BeginVertical(boxStyle);
//         EditorGUILayout.LabelField("Mode Selection", headerStyle);

//         EditorGUI.BeginChangeCheck();
//         bool previousEditMode = isEditMode;
//         isEditMode = EditorGUILayout.Toggle("Edit Existing SO", isEditMode);
//         bool modeChanged = EditorGUI.EndChangeCheck();

//         if (isEditMode)
//         {
//             EditorGUI.BeginChangeCheck();
//             targetSO = (InputMovementBridgeSO)EditorGUILayout.ObjectField(
//                 "Target SO", targetSO, typeof(InputMovementBridgeSO), false
//             );
//             bool soChanged = EditorGUI.EndChangeCheck();

//             if (soChanged || (modeChanged && !previousEditMode))
//             {
//                 SetupSerializedObject(targetSO);
//             }
//         }
//         else // Create Mode
//         {
//             targetSO = null;
//             serializedObject = null;
//             newSOName = EditorGUILayout.TextField("New SO Name", newSOName);

//             EditorGUILayout.BeginHorizontal();
//             EditorGUILayout.LabelField("Save Path:", GUILayout.Width(70));
//             EditorGUILayout.LabelField(savePath, EditorStyles.textField);
//             if (GUILayout.Button("Browse", GUILayout.Width(60)))
//             {
//                 string path = EditorUtility.OpenFolderPanel("Select Save Location", "Assets", "");
//                 if (!string.IsNullOrEmpty(path) && path.StartsWith(Application.dataPath))
//                 {
//                     savePath = "Assets" + path.Substring(Application.dataPath.Length) + "/";
//                 }
//             }
//             EditorGUILayout.EndHorizontal();
//         }

//         EditorGUILayout.EndVertical();
//     }

//     private void SetupSerializedObject(InputMovementBridgeSO so)
//     {
//         if (so != null)
//         {
//             serializedObject = new SerializedObject(so);
//             movementProp = serializedObject.FindProperty("movement");
//             inputProp = serializedObject.FindProperty("baseInput");

//             // Alt sınıf örneği null ise varsayılan bir değer ata (opsiyonel ama önerilir)
//             CreateDefaultInstancesIfNull();
//             Repaint();
//         }
//     }

//     private void CreateDefaultInstancesIfNull()
//     {
//         // Yalnızca SO'yu düzenlerken null olmaması için
//         if (inputProp != null && inputProp.managedReferenceValue == null)
//         {
//             inputProp.managedReferenceValue = new ExternalInput();
//         }
//         if (movementProp != null && movementProp.managedReferenceValue == null)
//         {
//             movementProp.managedReferenceValue = new ContinuousRigidbodyMovement();
//         }
//         if (serializedObject != null)
//         {
//             serializedObject.ApplyModifiedProperties();
//         }
//     }
//     #endregion

//     #region Data Configuration (Basit ve Etkili)

//     // Edit Modu: PropertyDrawer'larla otomatik çizim
//     private void DrawSerializedDataConfiguration()
//     {
//         if (serializedObject == null) return;

//         EditorGUILayout.BeginVertical(boxStyle);
//         EditorGUILayout.LabelField("Input Configuration", headerStyle);
//         // Bu tek satır, BaseInputDrawer'ınızın (varsa) tüm alt sınıfları çizmesini sağlar
//         EditorGUILayout.PropertyField(inputProp, new GUIContent("Input Logic"), true);
//         EditorGUILayout.EndVertical();

//         EditorGUILayout.BeginVertical(boxStyle);
//         EditorGUILayout.LabelField("Movement Configuration", headerStyle);
//         // Bu tek satır, MovementDrawer'ınızın (varsa) tüm alt sınıfları çizmesini sağlar
//         EditorGUILayout.PropertyField(movementProp, new GUIContent("Movement Logic"), true);
//         EditorGUILayout.EndVertical();
//     }

//     // Create Modu: Geçici bir SO oluşturup onu SerializedProperty ile çizdiririz.
//     private void DrawCreationDataConfiguration()
//     {
//         // Geçici SO yoksa oluştur
//         if (targetSO == null && serializedObject == null)
//         {
//             // Edit modunda olmadığımız için "targetSO" kullanmak karmaşa yaratır.
//             // Bunun yerine, tüm veriyi tutmak için bellekte geçici bir SO örneği oluştururuz.
//             targetSO = ScriptableObject.CreateInstance<InputMovementBridgeSO>();
//             targetSO.hideFlags = HideFlags.HideAndDontSave; // Diskte kaydetme
//             SetupSerializedObject(targetSO);
//         }

//         DrawSerializedDataConfiguration();
//     }

//     #endregion

//     #region Save Section
//     private void DrawSaveSection()
//     {
//         EditorGUILayout.BeginVertical(boxStyle);

//         bool dataValid = ValidateCreationData();

//         GUI.enabled = dataValid;

//         if (isEditMode)
//         {
//             // Edit modunda kaydetme, OnGUI'deki ApplyModifiedProperties() ile yapılır
//             EditorGUILayout.HelpBox($"SO: {targetSO.name} güncel. Değişiklikler otomatik kaydedilir.", MessageType.Info);
//             // İsterseniz buraya bir 'Force Save' butonu ekleyebilirsiniz.
//         }
//         else
//         {
//             if (GUILayout.Button("Create ScriptableObject", GUILayout.Height(35)))
//             {
//                 FinalizeCreation();
//             }
//         }

//         GUI.enabled = true;

//         if (!dataValid)
//         {
//             EditorGUILayout.HelpBox("Lütfen tüm zorunlu alanları (özellikle Input Action) doldurun.", MessageType.Error);
//         }

//         EditorGUILayout.EndVertical();
//     }

//     private bool ValidateCreationData()
//     {
//         // Geçici SO veya Property'ler oluşmadıysa kaydetmeye izin verme
//         if (inputProp == null || movementProp == null) return false;

//         // External Input seçiliyse, InputActionReference null olmamalı
//         if (inputProp.managedReferenceValue is ExternalInput ext && ext.inputAction == null)
//         {
//             return false;
//         }

//         // Şarj süreleri kontrolü (Önceki manuel mantığınızdan)
//         if (movementProp.managedReferenceValue is ChargedRigidbodyMovement chrgR)
//         {
//             if (chrgR.MinChargeTime > chrgR.MaxChargeTime) return false;
//         }
//         if (movementProp.managedReferenceValue is ChargedTransformMovement chrgT)
//         {
//             if (chrgT.MinChargeTime > chrgT.MaxChargeTime) return false;
//         }

//         if (!isEditMode && string.IsNullOrEmpty(newSOName)) return false;

//         return true;
//     }
//     #endregion

//     #region Finalization
//     private void FinalizeCreation()
//     {
//         if (targetSO == null || !ValidateCreationData()) return;

//         string fullPath = $"{savePath}{newSOName}.asset";

//         // Asset'i Project Penceresine kaydet
//         AssetDatabase.CreateAsset(targetSO, fullPath);

//         // HideFlags'i kaldırıp görünür hale getir
//         targetSO.hideFlags = HideFlags.None;

//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();

//         // Pencereyi Edit moda geçir ve yeni SO'yu seç
//         isEditMode = true;
//         targetSO = AssetDatabase.LoadAssetAtPath<InputMovementBridgeSO>(fullPath);
//         SetupSerializedObject(targetSO);

//         EditorUtility.FocusProjectWindow();
//         Selection.activeObject = targetSO;

//         EditorUtility.DisplayDialog("Success",
//             $"Created InputMovementBridge at:\n{fullPath}", "OK");
//     }

//     private void OnDestroy()
//     {
//         // Pencere kapanırken gizli ve kaydedilmemiş SO'yu temizle
//         if (targetSO != null && targetSO.hideFlags.HasFlag(HideFlags.HideAndDontSave))
//         {
//             DestroyImmediate(targetSO);
//         }
//     }
//     #endregion
// }

using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class InputMovementBridgeEditor : EditorWindow
{
    #region Window Setup
    [MenuItem("Tools/Movement/Input Bridge Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow<InputMovementBridgeEditor>("Input Bridge Editor");
        window.minSize = new Vector2(400, 600);
    }
    #endregion

    #region Serialized Data
    // Mode Selection
    private bool isEditMode = false;
    private InputMovementBridgeSO targetSO;
    private InputMovementBridgeSO lastLoadedSO; // Track last loaded SO
    private string newSOName = "NewInputBridge";
    private string savePath = "Assets/";

    // Input Type Selection
    private enum InputTypeSelection { External, Internal }
    private InputTypeSelection inputType = InputTypeSelection.External;

    // External Input Data
    private InputActionReference inputActionRef;

    // Internal Input Data
    private bool internalIsActive = false;
    private bool internalTriggerOnce = false;

    // Movement Type Selection
    private enum MovementTypeSelection
    {
        ContinuousRigidbody,
        TriggeredRigidbody,
        ChargedRigidbody,
        ContinuousTransform,
        TriggeredTransform,
        ChargedTransform
    }
    private MovementTypeSelection movementType = MovementTypeSelection.ContinuousRigidbody;

    // Common Movement Data
    private ForceType forceType = ForceType.Linear;
    private Vector3 direction = Vector3.forward;
    private bool useInputDirection = false;

    // Rigidbody Specific
    private ForceMode2D forceMode2D = ForceMode2D.Force;

    // Continuous Specific
    private float maxDuration = 0f;

    // Triggered Specific
    private CompletionType completionType = CompletionType.Time;
    private float completionValue = 1f;
    private bool blockUntilComplete = true;

    // Charged Specific
    private float minChargeTime = 0.2f;
    private float maxChargeTime = 2f;
    private float chargeMultiplier = 2f;

    // UI
    private Vector2 scrollPos;
    private GUIStyle headerStyle;
    private GUIStyle boxStyle;
    #endregion

    #region GUI Styles
    private void InitializeStyles()
    {
        if (headerStyle == null)
        {
            headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14,
                margin = new RectOffset(0, 0, 10, 5)
            };
        }

        if (boxStyle == null)
        {
            boxStyle = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(0, 0, 5, 5)
            };
        }
    }
    #endregion

    #region Main GUI
    private void OnGUI()
    {
        InitializeStyles();

        // Check if we need to reload SO data
        if (isEditMode && targetSO != null && targetSO != lastLoadedSO)
        {
            LoadFromSO();
            lastLoadedSO = targetSO;
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.Space(10);
        DrawModeSelection();
        EditorGUILayout.Space(10);

        if (isEditMode && targetSO == null)
        {
            EditorGUILayout.HelpBox("Please select a ScriptableObject to edit.", MessageType.Info);
            EditorGUILayout.EndScrollView();
            return;
        }

        DrawInputSection();
        EditorGUILayout.Space(10);

        DrawMovementSection();
        EditorGUILayout.Space(10);

        DrawSaveSection();
        EditorGUILayout.Space(10);

        EditorGUILayout.EndScrollView();
    }
    #endregion

    #region Mode Selection
    private void DrawModeSelection()
    {
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.LabelField("Mode Selection", headerStyle);

        EditorGUI.BeginChangeCheck();
        bool previousEditMode = isEditMode;
        isEditMode = EditorGUILayout.Toggle("Edit Existing SO", isEditMode);
        bool modeChanged = EditorGUI.EndChangeCheck();

        if (isEditMode)
        {
            EditorGUI.BeginChangeCheck();
            targetSO = (InputMovementBridgeSO)EditorGUILayout.ObjectField(
                "Target SO",
                targetSO,
                typeof(InputMovementBridgeSO),
                false
            );

            bool soChanged = EditorGUI.EndChangeCheck();

            // Load when SO changes or when switching to edit mode with SO already selected
            if ((soChanged && targetSO != null) ||
                (modeChanged && !previousEditMode && targetSO != null))
            {
                LoadFromSO();
                lastLoadedSO = targetSO;
            }
        }
        else
        {
            lastLoadedSO = null; // Reset tracking when switching to create mode

            newSOName = EditorGUILayout.TextField("New SO Name", newSOName);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Save Path:", GUILayout.Width(70));
            EditorGUILayout.LabelField(savePath, EditorStyles.textField);
            if (GUILayout.Button("Browse", GUILayout.Width(60)))
            {
                string path = EditorUtility.OpenFolderPanel("Select Save Location", "Assets", "");
                if (!string.IsNullOrEmpty(path))
                {
                    if (path.StartsWith(Application.dataPath))
                    {
                        savePath = "Assets" + path.Substring(Application.dataPath.Length) + "/";
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }
    #endregion

    #region Input Section
    private void DrawInputSection()
    {
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.LabelField("Input Configuration", headerStyle);

        inputType = (InputTypeSelection)EditorGUILayout.EnumPopup("Input Type", inputType);

        EditorGUILayout.Space(5);

        switch (inputType)
        {
            case InputTypeSelection.External:
                DrawExternalInputFields();
                break;
            case InputTypeSelection.Internal:
                DrawInternalInputFields();
                break;
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawExternalInputFields()
    {
        inputActionRef = (InputActionReference)EditorGUILayout.ObjectField(
            "Input Action",
            inputActionRef,
            typeof(InputActionReference),
            false
        );

        if (inputActionRef == null)
        {
            EditorGUILayout.HelpBox("Assign an Input Action Reference", MessageType.Warning);
        }
    }

    private void DrawInternalInputFields()
    {
        internalIsActive = EditorGUILayout.Toggle("Is Active", internalIsActive);
        internalTriggerOnce = EditorGUILayout.Toggle("Trigger Once", internalTriggerOnce);

        EditorGUILayout.HelpBox(
            "Internal Input can be controlled via scripts:\n" +
            "- IsActive: Toggle for continuous control\n" +
            "- TriggerOnce: Fire once then auto-reset",
            MessageType.Info
        );
    }
    #endregion

    #region Movement Section
    private void DrawMovementSection()
    {
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.LabelField("Movement Configuration", headerStyle);

        movementType = (MovementTypeSelection)EditorGUILayout.EnumPopup("Movement Type", movementType);

        EditorGUILayout.Space(5);

        // Common Fields
        DrawCommonMovementFields();

        EditorGUILayout.Space(5);

        // Type-Specific Fields
        switch (movementType)
        {
            case MovementTypeSelection.ContinuousRigidbody:
                DrawContinuousRigidbodyFields();
                break;
            case MovementTypeSelection.TriggeredRigidbody:
                DrawTriggeredRigidbodyFields();
                break;
            case MovementTypeSelection.ChargedRigidbody:
                DrawChargedRigidbodyFields();
                break;
            case MovementTypeSelection.ContinuousTransform:
                DrawContinuousTransformFields();
                break;
            case MovementTypeSelection.TriggeredTransform:
                DrawTriggeredTransformFields();
                break;
            case MovementTypeSelection.ChargedTransform:
                DrawChargedTransformFields();
                break;
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawCommonMovementFields()
    {
        forceType = (ForceType)EditorGUILayout.EnumPopup("Force Type", forceType);
        direction = EditorGUILayout.Vector3Field("Direction", direction);
        useInputDirection = EditorGUILayout.Toggle("Use Input Direction", useInputDirection);

        if (useInputDirection)
        {
            EditorGUILayout.HelpBox("Direction will be overridden by input", MessageType.Info);
        }
    }

    private void DrawContinuousRigidbodyFields()
    {
        forceMode2D = (ForceMode2D)EditorGUILayout.EnumPopup("Force Mode", forceMode2D);
        maxDuration = EditorGUILayout.Slider("Max Duration (0=unlimited)", maxDuration, 0f, 10f);
    }

    private void DrawTriggeredRigidbodyFields()
    {
        forceMode2D = (ForceMode2D)EditorGUILayout.EnumPopup("Force Mode", forceMode2D);
        completionType = (CompletionType)EditorGUILayout.EnumPopup("Completion Type", completionType);

        string label = completionType switch
        {
            CompletionType.Time => "Duration (seconds)",
            CompletionType.Rotation => "Rotation (degrees)",
            CompletionType.Distance => "Distance (units)",
            _ => "Completion Value"
        };

        completionValue = EditorGUILayout.FloatField(label, completionValue);
        blockUntilComplete = EditorGUILayout.Toggle("Block Until Complete", blockUntilComplete);
    }

    private void DrawChargedRigidbodyFields()
    {
        forceMode2D = (ForceMode2D)EditorGUILayout.EnumPopup("Force Mode", forceMode2D);
        minChargeTime = EditorGUILayout.FloatField("Min Charge Time", minChargeTime);
        maxChargeTime = EditorGUILayout.FloatField("Max Charge Time", maxChargeTime);
        chargeMultiplier = EditorGUILayout.FloatField("Charge Multiplier", chargeMultiplier);

        if (minChargeTime > maxChargeTime)
        {
            EditorGUILayout.HelpBox("Min Charge Time cannot exceed Max Charge Time", MessageType.Error);
        }
    }

    private void DrawContinuousTransformFields()
    {
        maxDuration = EditorGUILayout.Slider("Max Duration (0=unlimited)", maxDuration, 0f, 10f);
    }

    private void DrawTriggeredTransformFields()
    {
        completionType = (CompletionType)EditorGUILayout.EnumPopup("Completion Type", completionType);

        string label = completionType switch
        {
            CompletionType.Time => "Duration (seconds)",
            CompletionType.Rotation => "Rotation (degrees)",
            CompletionType.Distance => "Distance (units)",
            _ => "Completion Value"
        };

        completionValue = EditorGUILayout.FloatField(label, completionValue);
        blockUntilComplete = EditorGUILayout.Toggle("Block Until Complete", blockUntilComplete);
    }

    private void DrawChargedTransformFields()
    {
        minChargeTime = EditorGUILayout.FloatField("Min Charge Time", minChargeTime);
        maxChargeTime = EditorGUILayout.FloatField("Max Charge Time", maxChargeTime);
        chargeMultiplier = EditorGUILayout.FloatField("Charge Multiplier", chargeMultiplier);

        if (minChargeTime > maxChargeTime)
        {
            EditorGUILayout.HelpBox("Min Charge Time cannot exceed Max Charge Time", MessageType.Error);
        }
    }
    #endregion

    #region Save Section
    private void DrawSaveSection()
    {
        EditorGUILayout.BeginVertical(boxStyle);

        if (!ValidateData())
        {
            EditorGUILayout.HelpBox("Please fill all required fields", MessageType.Error);
        }

        GUI.enabled = ValidateData();

        if (isEditMode)
        {
            if (GUILayout.Button("Update ScriptableObject", GUILayout.Height(35)))
            {
                UpdateSO();
            }
        }
        else
        {
            if (GUILayout.Button("Create ScriptableObject", GUILayout.Height(35)))
            {
                CreateSO();
            }
        }

        GUI.enabled = true;

        EditorGUILayout.EndVertical();
    }

    private bool ValidateData()
    {
        if (inputType == InputTypeSelection.External && inputActionRef == null)
            return false;

        if (string.IsNullOrEmpty(newSOName) && !isEditMode)
            return false;

        if (movementType == MovementTypeSelection.ChargedRigidbody ||
            movementType == MovementTypeSelection.ChargedTransform)
        {
            if (minChargeTime > maxChargeTime)
                return false;
        }

        return true;
    }
    #endregion

    #region SO Creation/Update
    private void CreateSO()
    {
        if (!ValidateData())
        {
            EditorUtility.DisplayDialog("Validation Error",
                "Please check Console for validation errors.", "OK");
            return;
        }

        string fullPath = $"{savePath}{newSOName}.asset";
        if (AssetDatabase.LoadAssetAtPath<InputMovementBridgeSO>(fullPath) != null)
        {
            bool overwrite = EditorUtility.DisplayDialog("File Exists",
                $"A file named '{newSOName}.asset' already exists at this location. Overwrite?",
                "Overwrite", "Cancel");

            if (!overwrite) return;

            AssetDatabase.DeleteAsset(fullPath);
        }

        InputMovementBridgeSO newSO = CreateInstance<InputMovementBridgeSO>();

        newSO.baseInput = CreateInputInstance();
        if (newSO.baseInput == null)
        {
            Debug.LogError("Failed to create Input instance!");
            DestroyImmediate(newSO);
            return;
        }

        newSO.movement = CreateMovementInstance();
        if (newSO.movement == null)
        {
            Debug.LogError("Failed to create Movement instance!");
            DestroyImmediate(newSO);
            return;
        }

        string directory = System.IO.Path.GetDirectoryName(fullPath);
        if (!System.IO.Directory.Exists(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
        }

        AssetDatabase.CreateAsset(newSO, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newSO;

        EditorUtility.DisplayDialog("Success",
            $"Created InputMovementBridge at:\n{fullPath}", "OK");

        Debug.Log($"✓ Created InputMovementBridge at: {fullPath}");
    }

    private void UpdateSO()
    {
        if (targetSO == null)
        {
            Debug.LogError("Cannot update: Target SO is null");
            return;
        }

        if (!ValidateData())
        {
            EditorUtility.DisplayDialog("Validation Error",
                "Please check Console for validation errors.", "OK");
            return;
        }

        Undo.RecordObject(targetSO, "Update InputMovementBridge");

        BaseInput newInput = CreateInputInstance();
        Movement newMovement = CreateMovementInstance();

        if (newInput == null)
        {
            Debug.LogError("Failed to create Input instance!");
            return;
        }

        if (newMovement == null)
        {
            Debug.LogError("Failed to create Movement instance!");
            return;
        }

        targetSO.baseInput = newInput;
        targetSO.movement = newMovement;

        EditorUtility.SetDirty(targetSO);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Success",
            $"Updated {targetSO.name} successfully!", "OK");

        Debug.Log($"✓ Updated {targetSO.name}");
    }

    private BaseInput CreateInputInstance()
    {
        BaseInput input = null;

        try
        {
            switch (inputType)
            {
                case InputTypeSelection.External:
                    input = new ExternalInput { inputAction = inputActionRef };
                    break;

                case InputTypeSelection.Internal:
                    input = new InternalInput
                    {
                        IsActive = internalIsActive,
                        TriggerOnce = internalTriggerOnce
                    };
                    break;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error creating Input instance: {e.Message}");
            return null;
        }

        return input;
    }

    private Movement CreateMovementInstance()
    {
        Movement movement = null;

        try
        {
            switch (movementType)
            {
                case MovementTypeSelection.ContinuousRigidbody:
                    movement = new ContinuousRigidbodyMovement
                    {
                        ForceMode2D = forceMode2D,
                        MaxDuration = maxDuration
                    };
                    break;

                case MovementTypeSelection.TriggeredRigidbody:
                    movement = new TriggeredRigidbodyMovement
                    {
                        ForceMode2D = forceMode2D,
                        CompletionType = completionType,
                        CompletionValue = completionValue,
                        BlockUntilComplete = blockUntilComplete
                    };
                    break;

                case MovementTypeSelection.ChargedRigidbody:
                    movement = new ChargedRigidbodyMovement
                    {
                        ForceMode2D = forceMode2D,
                        MinChargeTime = minChargeTime,
                        MaxChargeTime = maxChargeTime,
                        ChargeMultiplier = chargeMultiplier
                    };
                    break;

                case MovementTypeSelection.ContinuousTransform:
                    movement = new ContinuousTransformMovement
                    {
                        MaxDuration = maxDuration
                    };
                    break;

                case MovementTypeSelection.TriggeredTransform:
                    movement = new TriggeredTransformMovement
                    {
                        CompletionType = completionType,
                        CompletionValue = completionValue,
                        BlockUntilComplete = blockUntilComplete
                    };
                    break;

                case MovementTypeSelection.ChargedTransform:
                    movement = new ChargedTransformMovement
                    {
                        MinChargeTime = minChargeTime,
                        MaxChargeTime = maxChargeTime,
                        ChargeMultiplier = chargeMultiplier
                    };
                    break;
            }

            if (movement != null)
            {
                movement.ForceType = forceType;
                movement.Direction = direction;
                movement.UseInputDirection = useInputDirection;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error creating Movement instance: {e.Message}\n{e.StackTrace}");
            return null;
        }

        return movement;
    }
    #endregion

    #region Load from SO
    private void LoadFromSO()
    {
        if (targetSO == null)
        {
            Debug.LogWarning("Target SO is null, cannot load data.");
            return;
        }

        Debug.Log($"📂 Loading data from SO: {targetSO.name}");

        ResetFields();

        // Load Input
        if (targetSO.baseInput != null)
        {
            if (targetSO.baseInput is ExternalInput ext)
            {
                inputType = InputTypeSelection.External;
                inputActionRef = ext.inputAction;
                Debug.Log($"  ✓ External Input - Action: {(ext.inputAction != null ? ext.inputAction.name : "null")}");
            }
            else if (targetSO.baseInput is InternalInput intern)
            {
                inputType = InputTypeSelection.Internal;
                internalIsActive = intern.IsActive;
                internalTriggerOnce = intern.TriggerOnce;
                Debug.Log($"  ✓ Internal Input - IsActive: {intern.IsActive}, TriggerOnce: {intern.TriggerOnce}");
            }
        }
        else
        {
            Debug.LogWarning("  ⚠ SO has null baseInput!");
        }

        // Load Movement
        var mov = targetSO.movement;
        if (mov != null)
        {
            forceType = mov.ForceType;
            direction = mov.Direction;
            useInputDirection = mov.UseInputDirection;

            Debug.Log($"  ✓ Movement Type: {mov.GetType().Name}");
            Debug.Log($"    ForceType: {mov.ForceType}, Direction: {mov.Direction}, UseInputDirection: {mov.UseInputDirection}");

            switch (mov)
            {
                case ContinuousRigidbodyMovement crm:
                    movementType = MovementTypeSelection.ContinuousRigidbody;
                    forceMode2D = crm.ForceMode2D;
                    maxDuration = crm.MaxDuration;
                    break;

                case TriggeredRigidbodyMovement trm:
                    movementType = MovementTypeSelection.TriggeredRigidbody;
                    forceMode2D = trm.ForceMode2D;
                    completionType = trm.CompletionType;
                    completionValue = trm.CompletionValue;
                    blockUntilComplete = trm.BlockUntilComplete;
                    break;

                case ChargedRigidbodyMovement chrm:
                    movementType = MovementTypeSelection.ChargedRigidbody;
                    forceMode2D = chrm.ForceMode2D;
                    minChargeTime = chrm.MinChargeTime;
                    maxChargeTime = chrm.MaxChargeTime;
                    chargeMultiplier = chrm.ChargeMultiplier;
                    break;

                case ContinuousTransformMovement ctm:
                    movementType = MovementTypeSelection.ContinuousTransform;
                    maxDuration = ctm.MaxDuration;
                    break;

                case TriggeredTransformMovement ttm:
                    movementType = MovementTypeSelection.TriggeredTransform;
                    completionType = ttm.CompletionType;
                    completionValue = ttm.CompletionValue;
                    blockUntilComplete = ttm.BlockUntilComplete;
                    break;

                case ChargedTransformMovement chtm:
                    movementType = MovementTypeSelection.ChargedTransform;
                    minChargeTime = chtm.MinChargeTime;
                    maxChargeTime = chtm.MaxChargeTime;
                    chargeMultiplier = chtm.ChargeMultiplier;
                    break;

                default:
                    Debug.LogError($"  ❌ Unknown movement type: {mov.GetType().Name}");
                    break;
            }
        }
        else
        {
            Debug.LogWarning("  ⚠ SO has null movement!");
        }

        Repaint();
        Debug.Log("✓ LoadFromSO completed");
    }

    private void ResetFields()
    {
        inputActionRef = null;
        internalIsActive = false;
        internalTriggerOnce = false;

        forceType = ForceType.Linear;
        direction = Vector3.forward;
        useInputDirection = false;

        forceMode2D = ForceMode2D.Force;
        maxDuration = 0f;

        completionType = CompletionType.Time;
        completionValue = 1f;
        blockUntilComplete = true;

        minChargeTime = 0.2f;
        maxChargeTime = 2f;
        chargeMultiplier = 2f;
    }
    #endregion
}


