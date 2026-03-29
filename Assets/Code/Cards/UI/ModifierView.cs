using Game.Cards.Modifier;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Cards.UI
{
    public class ModifierView : MonoBehaviour
    {
        // [SerializeField] private TMP_Text nameText;
        // [SerializeField] private TMP_Text stackText;
        [SerializeField] private SpriteRenderer iconSprite;
        
        private ModifierData modifierData;
        
        public void Initialize(ModifierData data)
        {
            modifierData = data;
            UpdateView();
        }
        
        public void UpdateView()
        {
            // if (nameText != null) nameText.text = modifierData.Modifier.modifierName;
            // if (stackText != null) stackText.text = modifierData.Stack.ToString();
            if (iconSprite != null && modifierData.Modifier.icon != null) iconSprite.sprite = modifierData.Modifier.icon;
        }

        public void ClearAll()
        {
            if (iconSprite != null && modifierData.Modifier.icon != null) iconSprite.sprite = null;
        }
    }
}