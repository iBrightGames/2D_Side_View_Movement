using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(BaseInput), true)]
public class BaseInputDrawer : PropertyDrawer
{
    private const float LabelWidth = 100f; // Etiket genişliği
    private const float VerticalSpacing = 2f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Temel sınıfın (BaseInput) alanı zaten gizli, sadece alt sınıfların property'leri için yükseklik hesapla
        SerializedProperty iterator = property.Copy();
        bool enterChildren = true;
        float totalHeight = EditorGUIUtility.singleLineHeight + VerticalSpacing; // Başlık için yükseklik

        while (iterator.NextVisible(enterChildren))
        {
            // Script property'sini ve diğer gereksiz alanları atla
            if (iterator.propertyPath.Contains("m_Script"))
            {
                enterChildren = false;
                continue;
            }

            // "BaseInput" soyut sınıfının kendi alanlarını atla (şu anda yok ama gelecekte eklenebilir)
            if (iterator.propertyPath.StartsWith(property.propertyPath + ".m_Script"))
            {
                continue;
            }

            totalHeight += EditorGUI.GetPropertyHeight(iterator, true) + VerticalSpacing;
            enterChildren = false; // Sadece birinci seviyedeki çocukları say
        }

        return totalHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Başlık (Sınıf adını göster)
        string className = property.managedReferenceValue != null ?
                           property.managedReferenceValue.GetType().Name :
                           "BaseInput (Null)";

        Rect titleRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(titleRect, new GUIContent(label.text + " (" + className + ")", label.tooltip), EditorStyles.boldLabel);

        // Alanları çizmeye başla
        Rect currentRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + VerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);

        SerializedProperty iterator = property.Copy();
        bool enterChildren = true;

        while (iterator.NextVisible(enterChildren))
        {
            // Script property'sini atla
            if (iterator.propertyPath.Contains("m_Script"))
            {
                enterChildren = false;
                continue;
            }

            // Geçerli Property için yüksekliği al
            float propHeight = EditorGUI.GetPropertyHeight(iterator, true);
            currentRect.height = propHeight;

            // Alanı çiz
            EditorGUI.PropertyField(currentRect, iterator, true);

            // Sonraki alanın konumunu ayarla
            currentRect.y += propHeight + VerticalSpacing;
            enterChildren = false;
        }

        EditorGUI.EndProperty();
    }
}

// using UnityEditor;
// using UnityEngine;

[CustomPropertyDrawer(typeof(Movement), true)]
public class MovementDrawer : PropertyDrawer
{
    private const float VerticalSpacing = 2f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Temel sınıfın ve alt sınıfların tüm property'leri için toplam yükseklik hesapla
        SerializedProperty iterator = property.Copy();
        bool enterChildren = true;
        float totalHeight = EditorGUIUtility.singleLineHeight + VerticalSpacing; // Başlık için yükseklik

        while (iterator.NextVisible(enterChildren))
        {
            // Script property'sini atla
            if (iterator.propertyPath.Contains("m_Script"))
            {
                enterChildren = false;
                continue;
            }

            totalHeight += EditorGUI.GetPropertyHeight(iterator, true) + VerticalSpacing;
            enterChildren = false; // Sadece birinci seviyedeki çocukları say
        }

        return totalHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Başlık (Sınıf adını göster)
        string className = property.managedReferenceValue != null ?
                           property.managedReferenceValue.GetType().Name :
                           "Movement (Null)";

        Rect titleRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(titleRect, new GUIContent(label.text + " (" + className + ")", label.tooltip), EditorStyles.boldLabel);

        // Alanları çizmeye başla
        Rect currentRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + VerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);

        SerializedProperty iterator = property.Copy();
        bool enterChildren = true;

        while (iterator.NextVisible(enterChildren))
        {
            // Script property'sini atla
            if (iterator.propertyPath.Contains("m_Script"))
            {
                enterChildren = false;
                continue;
            }

            // Geçerli Property için yüksekliği al
            float propHeight = EditorGUI.GetPropertyHeight(iterator, true);
            currentRect.height = propHeight;

            // Alanı çiz
            EditorGUI.PropertyField(currentRect, iterator, true);

            // Sonraki alanın konumunu ayarla
            currentRect.y += propHeight + VerticalSpacing;
            enterChildren = false;
        }

        EditorGUI.EndProperty();
    }
}


