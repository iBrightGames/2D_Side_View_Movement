#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;

#region InputMovementBridge Custom Drawer

[CustomPropertyDrawer(typeof(InputMovementBridge))]
public class InputMovementBridgeDrawer : PropertyDrawer
{
    private const float LINE_HEIGHT = 20f;
    private const float SPACING = 4f;
    private const float INDENT = 15f;
    
    private bool showConditions = true;
    private bool showModifiers = true;
    private bool showFeedback = true;
    private bool showDebug = false;
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = LINE_HEIGHT; // Bridge name
        
        SerializedProperty enabled = property.FindPropertyRelative("enabled");
        if (!enabled.boolValue)
            return height + SPACING;
        
        height += LINE_HEIGHT * 3; // Input, Movement, enabled toggle
        height += SPACING * 3;
        
        // Conditions section
        SerializedProperty conditions = property.FindPropertyRelative("conditions");
        if (conditions != null)
        {
            height += LINE_HEIGHT; // Header
            if (showConditions)
            {
                height += EditorGUI.GetPropertyHeight(conditions, true);
                height += LINE_HEIGHT; // Status display
            }
        }
        
        // Modifiers section
        SerializedProperty modifiers = property.FindPropertyRelative("modifiers");
        if (modifiers != null)
        {
            height += LINE_HEIGHT; // Header
            if (showModifiers)
                height += EditorGUI.GetPropertyHeight(modifiers, true);
        }
        
        // Feedback section
        height += LINE_HEIGHT; // Header
        if (showFeedback)
        {
            SerializedProperty feedback = property.FindPropertyRelative("feedback");
            if (feedback != null)
                height += EditorGUI.GetPropertyHeight(feedback, true);
        }
        
        // Debug section
        SerializedProperty showDebugInfo = property.FindPropertyRelative("showDebugInfo");
        if (showDebugInfo != null && showDebugInfo.boolValue)
        {
            height += LINE_HEIGHT * 3; // Debug stats
        }
        
        return height + SPACING * 4;
    }
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        Rect rect = new Rect(position.x, position.y, position.width, LINE_HEIGHT);
        
        // Get properties
        SerializedProperty bridgeName = property.FindPropertyRelative("bridgeName");
        SerializedProperty enabled = property.FindPropertyRelative("enabled");
        SerializedProperty inputSource = property.FindPropertyRelative("inputSource");
        SerializedProperty movement = property.FindPropertyRelative("movement");
        SerializedProperty conditions = property.FindPropertyRelative("conditions");
        SerializedProperty modifiers = property.FindPropertyRelative("modifiers");
        SerializedProperty feedback = property.FindPropertyRelative("feedback");
        SerializedProperty showDebugInfo = property.FindPropertyRelative("showDebugInfo");
        
        // Bridge name with toggle
        Color originalBG = GUI.backgroundColor;
        GUI.backgroundColor = enabled.boolValue ? Color.green : Color.gray;
        
        EditorGUI.BeginChangeCheck();
        Rect nameRect = new Rect(rect.x, rect.y, rect.width - 50, rect.height);
        Rect toggleRect = new Rect(rect.x + rect.width - 45, rect.y, 45, rect.height);
        
        bridgeName.stringValue = EditorGUI.TextField(nameRect, bridgeName.stringValue, EditorStyles.boldLabel);
        enabled.boolValue = EditorGUI.Toggle(toggleRect, enabled.boolValue);
        
        GUI.backgroundColor = originalBG;
        
        if (!enabled.boolValue)
        {
            EditorGUI.EndProperty();
            return;
        }
        
        rect.y += LINE_HEIGHT + SPACING;
        
        // Draw box background
        Rect boxRect = new Rect(position.x, rect.y, position.width, position.height - (rect.y - position.y));
        GUI.Box(boxRect, "", EditorStyles.helpBox);
        
        rect.x += INDENT;
        rect.width -= INDENT * 2;
        
        // Input Source
        EditorGUI.PropertyField(rect, inputSource, new GUIContent("Input Source"));
        rect.y += LINE_HEIGHT + SPACING;
        
        // Movement
        EditorGUI.PropertyField(rect, movement, new GUIContent("Movement"));
        rect.y += LINE_HEIGHT + SPACING * 2;
        
        // Conditions Section
        DrawSectionHeader(ref rect, "Conditions", ref showConditions);
        if (showConditions && conditions != null)
        {
            EditorGUI.PropertyField(rect, conditions, new GUIContent("Required Conditions"), true);
            float conditionsHeight = EditorGUI.GetPropertyHeight(conditions, true);
            rect.y += conditionsHeight;
            
            // Show condition status in play mode
            if (Application.isPlaying)
            {
                DrawConditionStatus(rect, property);
                rect.y += LINE_HEIGHT + SPACING;
            }
        }
        rect.y += SPACING;
        
        // Modifiers Section
        DrawSectionHeader(ref rect, "Modifiers", ref showModifiers);
        if (showModifiers && modifiers != null)
        {
            EditorGUI.PropertyField(rect, modifiers, new GUIContent("Force Modifiers"), true);
            float modifiersHeight = EditorGUI.GetPropertyHeight(modifiers, true);
            rect.y += modifiersHeight + SPACING;
        }
        rect.y += SPACING;
        
        // Feedback Section
        DrawSectionHeader(ref rect, "Feedback", ref showFeedback);
        if (showFeedback && feedback != null)
        {
            EditorGUI.PropertyField(rect, feedback, true);
            float feedbackHeight = EditorGUI.GetPropertyHeight(feedback, true);
            rect.y += feedbackHeight + SPACING;
        }
        rect.y += SPACING;
        
        // Debug Info
        EditorGUI.PropertyField(rect, showDebugInfo, new GUIContent("Show Debug Info"));
        rect.y += LINE_HEIGHT;
        
        if (showDebugInfo.boolValue && Application.isPlaying)
        {
            DrawDebugInfo(rect, property);
        }
        
        EditorGUI.EndProperty();
    }
    
    private void DrawSectionHeader(ref Rect rect, string title, ref bool foldout)
    {
        Rect headerRect = new Rect(rect.x - INDENT, rect.y, rect.width + INDENT * 2, LINE_HEIGHT);
        
        Color originalBG = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
        GUI.Box(headerRect, "", EditorStyles.toolbar);
        GUI.backgroundColor = originalBG;
        
        foldout = EditorGUI.Foldout(headerRect, foldout, title, true, EditorStyles.foldoutHeader);
        rect.y += LINE_HEIGHT;
    }
    
    private void DrawConditionStatus(Rect rect, SerializedProperty property)
    {
        // Get the actual bridge object to check condition status
        object targetObject = GetTargetObjectOfProperty(property);
        if (targetObject is InputMovementBridge bridge)
        {
            // Find MovementController in scene
            MovementController controller = GameObject.FindFirstObjectByType<MovementController>();
            if (controller != null && bridge.conditions != null)
            {
                EditorGUI.LabelField(rect, "Condition Status:", EditorStyles.boldLabel);
                rect.y += LINE_HEIGHT;
                
                foreach (var condition in bridge.conditions)
                {
                    if (condition == null) continue;
                    
                    bool passed = condition.CanExecute(controller);
                    Color color = passed ? Color.green : Color.red;
                    string status = passed ? "✓ PASS" : "✗ FAIL";
                    
                    GUI.color = color;
                    EditorGUI.LabelField(rect, $"  {condition.conditionName}: {status}");
                    GUI.color = Color.white;
                    
                    if (!passed)
                    {
                        rect.y += LINE_HEIGHT;
                        EditorGUI.LabelField(rect, $"    Reason: {condition.GetFailureReason()}", EditorStyles.miniLabel);
                    }
                    
                    rect.y += LINE_HEIGHT;
                }
            }
        }
    }
    
    private void DrawDebugInfo(Rect rect, SerializedProperty property)
    {
        object targetObject = GetTargetObjectOfProperty(property);
        if (targetObject is InputMovementBridge bridge)
        {
            EditorGUI.LabelField(rect, "═══ Runtime Debug ═══", EditorStyles.boldLabel);
            rect.y += LINE_HEIGHT;
            
            // Execution stats
            EditorGUI.LabelField(rect, $"Executing: {(bridge.isExecuting ? "YES" : "NO")}");
            rect.y += LINE_HEIGHT;
            
            if (bridge.lastExecutionTime > 0)
            {
                float timeSince = Time.time - bridge.lastExecutionTime;
                EditorGUI.LabelField(rect, $"Last Executed: {timeSince:F2}s ago");
            }
            else
            {
                EditorGUI.LabelField(rect, "Last Executed: Never");
            }
            rect.y += LINE_HEIGHT;
            
            EditorGUI.LabelField(rect, $"Execution Count: {bridge.executionCount}");
        }
    }
    
    private object GetTargetObjectOfProperty(SerializedProperty prop)
    {
        var path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        var elements = path.Split('.');
        
        foreach (var element in elements)
        {
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue_Imp(obj, elementName, index);
            }
            else
            {
                obj = GetValue_Imp(obj, element);
            }
        }
        return obj;
    }
    
    private object GetValue_Imp(object source, string name)
    {
        if (source == null) return null;
        var type = source.GetType();
        
        while (type != null)
        {
            var f = type.GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (f != null) return f.GetValue(source);
            
            var p = type.GetProperty(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
            if (p != null) return p.GetValue(source, null);
            
            type = type.BaseType;
        }
        return null;
    }
    
    private object GetValue_Imp(object source, string name, int index)
    {
        var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
        if (enumerable == null) return null;
        var enm = enumerable.GetEnumerator();
        
        for (int i = 0; i <= index; i++)
        {
            if (!enm.MoveNext()) return null;
        }
        return enm.Current;
    }
}

#endregion

#region InternalInputSO Custom Inspector

[CustomEditor(typeof(InternalInputSO))]
public class InternalInputSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        InternalInputSO input = (InternalInputSO)target;
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Runtime Controls", EditorStyles.boldLabel);
        
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Enter Play Mode to use runtime controls", MessageType.Info);
            return;
        }
        
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        
        // Continuous control
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Continuous Mode:", GUILayout.Width(120));
        
        if (input.isActive)
        {
            GUI.color = Color.red;
            if (GUILayout.Button("Deactivate", GUILayout.Height(30)))
            {
                input.Deactivate();
            }
        }
        else
        {
            GUI.color = Color.green;
            if (GUILayout.Button("Activate", GUILayout.Height(30)))
            {
                input.Activate();
            }
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // One-shot trigger
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("One-Shot Trigger:", GUILayout.Width(120));
        GUI.color = Color.cyan;
        if (GUILayout.Button("Trigger Once", GUILayout.Height(30)))
        {
            input.Trigger();
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // Toggle
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Quick Toggle:", GUILayout.Width(120));
        GUI.color = Color.yellow;
        if (GUILayout.Button("Toggle Active", GUILayout.Height(30)))
        {
            input.Toggle();
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
        
        // Status display
        EditorGUILayout.Space(10);
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Current Status", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"Is Active: {input.isActive}");
        EditorGUILayout.LabelField($"Trigger Once: {input.triggerOnce}");
        EditorGUILayout.LabelField($"Direction: {input.scriptedDirection}");
        EditorGUILayout.EndVertical();
        
        // Repaint to show live updates
        if (Application.isPlaying)
        {
            Repaint();
        }
    }
}

#endregion

#region CooldownCondition Custom Inspector

[CustomEditor(typeof(CooldownCondition))]
public class CooldownConditionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        CooldownCondition condition = (CooldownCondition)target;
        
        if (!Application.isPlaying) return;
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Runtime Status", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        
        float remaining = condition.GetRemainingCooldown();
        float percent = condition.GetCooldownPercent();
        
        if (remaining > 0)
        {
            EditorGUILayout.LabelField($"⏱ On Cooldown: {remaining:F2}s remaining");
            
            Rect rect = EditorGUILayout.GetControlRect(false, 20);
            EditorGUI.ProgressBar(rect, percent, $"{percent * 100:F0}%");
        }
        else
        {
            GUI.color = Color.green;
            EditorGUILayout.LabelField("✓ Ready to use!");
            GUI.color = Color.white;
        }
        
        EditorGUILayout.Space(5);
        
        if (GUILayout.Button("Reset Cooldown", GUILayout.Height(25)))
        {
            // Force reset by setting last execution time far in past
            System.Reflection.FieldInfo field = typeof(CooldownCondition).GetField("lastExecutionTime", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(condition, -999f);
        }
        
        EditorGUILayout.EndVertical();
        
        Repaint();
    }
}

#endregion

#region Movement Debug Window

public class MovementDebugWindow : EditorWindow
{
    private MovementController selectedController;
    private Vector2 scrollPosition;
    
    [MenuItem("Tools/Movement System/Debug Window")]
    public static void ShowWindow()
    {
        GetWindow<MovementDebugWindow>("Movement Debug");
    }
    
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Movement System Debugger", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        selectedController = (MovementController)EditorGUILayout.ObjectField(
            "Controller", selectedController, typeof(MovementController), true);
        
        if (selectedController == null)
        {
            EditorGUILayout.HelpBox("Select a MovementController to debug", MessageType.Info);
            return;
        }
        
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Enter Play Mode to see debug info", MessageType.Warning);
            return;
        }
        
        EditorGUILayout.Space();
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        // Controller state
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Controller State", EditorStyles.boldLabel);
        // EditorGUILayout.LabelField($"Current Movement: {selectedController.GetCurrentMovement()?.name ?? "None"}");
        EditorGUILayout.LabelField($"Velocity: {selectedController.GetVelocity()}");
        // EditorGUILayout.LabelField($"Is Executing: {selectedController.IsExecutingMovement()}");
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space();
        
        // Find all bridges
        InputMovementHandler handler = selectedController.GetComponent<InputMovementHandler>();
        if (handler != null)
        {
            EditorGUILayout.LabelField("Active Bridges", EditorStyles.boldLabel);
            
            System.Reflection.FieldInfo field = typeof(InputMovementHandler).GetField("inputMovementBridges", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (field != null)
            {
                InputMovementBridge[] bridges = (InputMovementBridge[])field.GetValue(handler);
                
                foreach (var bridge in bridges)
                {
                    if (bridge == null) continue;
                    
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    
                    Color statusColor = bridge.enabled ? (bridge.isExecuting ? Color.green : Color.white) : Color.gray;
                    GUI.color = statusColor;
                    EditorGUILayout.LabelField(bridge.bridgeName, EditorStyles.boldLabel);
                    GUI.color = Color.white;
                    
                    EditorGUILayout.LabelField($"Enabled: {bridge.enabled}");
                    EditorGUILayout.LabelField($"Executing: {bridge.isExecuting}");
                    EditorGUILayout.LabelField($"Executions: {bridge.executionCount}");
                    
                    if (bridge.lastExecutionTime > 0)
                    {
                        float timeSince = Time.time - bridge.lastExecutionTime;
                        EditorGUILayout.LabelField($"Last Used: {timeSince:F2}s ago");
                    }
                    
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space(5);
                }
            }
        }
        
        EditorGUILayout.EndScrollView();
        
        Repaint();
    }
}

#endregion


#endif