#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SpriteDatabaseAnimation
{
    [CustomEditor(typeof(SpriteDatabaseAnimator))]
    public class SpriteDatabaseAnimatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var anim = (SpriteDatabaseAnimator)target;

            if (anim == null)
                return;

            GUILayout.Space(10);
            GUILayout.Label("Test Category Buttons", EditorStyles.boldLabel);

            // Get categories from the Sprite Library
            if (anim.GetAvailableCategories() != null)
            {
                foreach (var cat in anim.GetAvailableCategories())
                {
                    if (GUILayout.Button(cat))
                        anim.SetCategory(cat);
                }
            }

            if (!Application.isPlaying)
                EditorUtility.SetDirty(anim);
        }
    }
#endif


}

