

using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Collections.Generic;

namespace SpriteDatabaseAnimation
{

    [RequireComponent(typeof(SpriteLibrary))]
    [RequireComponent(typeof(SpriteResolver))]
    public class SpriteDatabaseAnimator : MonoBehaviour
    {
        [SerializeField] SpriteResolver resolver;
        [SerializeField] SpriteLibrary spriteLibrary; // Reference to get available labels
        [SerializeField] float frameRate = 10f;
        [SerializeField] string currentCategory;

        int frame;
        float timer;
        List<string> currentLabels = new List<string>();
        void Awake()
        {
            if (resolver == null) { resolver = GetComponent<SpriteResolver>(); }
            if(spriteLibrary==null){ spriteLibrary = GetComponent<SpriteLibrary>(); }
        }

        void Start()
        {
            // If no sprite library assigned, try to get it from resolver
            if (spriteLibrary == null && resolver != null)
            {
                spriteLibrary = resolver.GetComponent<SpriteLibrary>();
            }
        }

        public void SetCategory(string category)
        {
            currentCategory = category;
            frame = 0;
            timer = 0;
            UpdateLabelsForCategory();
        }

        void UpdateLabelsForCategory()
        {
            currentLabels.Clear();

            if (spriteLibrary == null || string.IsNullOrEmpty(currentCategory))
                return;

            var labels = spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(currentCategory);
            if (labels != null)
            {
                foreach (var label in labels)
                {
                    currentLabels.Add(label);
                }
            }
        }

        void Update()
        {
            if (string.IsNullOrEmpty(currentCategory) || currentLabels.Count == 0)
                return;

            timer += Time.deltaTime;
            if (timer >= 1f / frameRate)
            {
                timer = 0;
                frame = (frame + 1) % currentLabels.Count;
                resolver.SetCategoryAndLabel(currentCategory, currentLabels[frame]);
            }
        }

        public string[] GetAvailableCategories()
        {
            if (spriteLibrary == null || spriteLibrary.spriteLibraryAsset == null)
                return null;

            var categories = new List<string>();
            foreach (var category in spriteLibrary.spriteLibraryAsset.GetCategoryNames())
            {
                categories.Add(category);
            }
            return categories.ToArray();
        }
    
    }

}

