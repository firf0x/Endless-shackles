using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.GameSystem;
using Game.Cards.Modifier;
using System.Collections.Generic;
using Game.Lib;

namespace Game.Cards.UI
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private CardViewModel viewModel;
        
        // Текстовые поля для отображения информации
        [Header("Ссылки на текстовые поля")]
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private TMP_Text stepText;
        
        // Контейнеры для включения/отключения в зависимости от наличия декораторов
        [Header("Ссылки на контейнеры"), Space(20f)]
        [SerializeField] private GameObject healthContainer;
        [SerializeField] private GameObject damageContainer;
        [SerializeField] private GameObject stepContainer;
        [SerializeField] private List<GameObject> modifierPrefab;

        private Dictionary<ModifierData, ModifierView> modifierViews = new Dictionary<ModifierData, ModifierView>();
        
        private void Start()
        {
            viewModel = new CardViewModel(GetComponent<CardData>());

            InitializeView();
            UpdateView();

            if (viewModel.isModifierDecorator)
            {
                InitializeModifiers();
                viewModel.ModifierAdded += OnModifierAdded;
                viewModel.ModifierRemoved += OnModifierRemoved;
            }

            StepCombatSystem.Instance.EventUpdate += UpdateView;
            viewModel.UpdateUI += UpdateView;
        }

        private void Update()
        {
            //TODO: Сделать наведение из statemachine
        }

        private void OnDisable()
        {
            if(StepCombatSystem.Instance != null) StepCombatSystem.Instance.EventUpdate -= UpdateView;
            viewModel.UpdateUI -= UpdateView;

            if (viewModel != null)
            {
                viewModel.ModifierAdded -= OnModifierAdded;
                viewModel.ModifierRemoved -= OnModifierRemoved;
            }
        }
        
        private void InitializeView()
        {
            if (healthContainer != null) healthContainer.SetActive(viewModel.isHealthDecorator);
            if (damageContainer != null) damageContainer.SetActive(viewModel.isAttackDecorator);
            if (stepContainer != null) stepContainer.SetActive(viewModel.isStepDecorator);
        }
        
        #region Modifier
        
        private void InitializeModifiers()
        {
            if (modifierPrefab == null) return;
            
            foreach (var modifier in viewModel.Modifiers)
            {
                CreateModifierView(modifier);
            }
        }

        private void OnModifierAdded(ModifierData modifier)
        {
            CreateModifierView(modifier);
        }
        
        private void OnModifierRemoved(ModifierData modifier)
        {
            if (modifierViews.TryGetValue(modifier, out var view))
            {
                view.gameObject.SetActive(false);
                modifierViews.Remove(modifier);
                view.ClearAll();
            }
        }

        private void CreateModifierView(ModifierData data)
        {
            //  data.Modifier.icon;
            var view = GetModifierViewFromContainer();
            if(view == null) return;

            view.gameObject.SetActive(true);
            view.Initialize(data);

            modifierViews[data] = view;
        }

        // Util
        private ModifierView GetModifierViewFromContainer()
        {
            foreach (var prefab in modifierPrefab)
            {
                if(prefab.activeSelf == false && prefab.TryGetComponent<ModifierView>(out var component)) return component;
            }

            return null;
        }

        #endregion

        private void UpdateView()
        {
            if (healthText != null && viewModel.isHealthDecorator) healthText.text = viewModel.Health;
            if (damageText != null && viewModel.isAttackDecorator) damageText.text = viewModel.Damage;
            if (stepText != null && viewModel.isStepDecorator) stepText.text = viewModel.Step;
            
            // Debug.Log($"Обновление данных => {viewModel.Name}");
        }

        private void OnDestroy()
        {
            if (viewModel != null)
            {
                StepCombatSystem.Instance.EventUpdate -= UpdateView;
            }
            modifierViews.Clear();
            viewModel.UpdateUI -= UpdateView;
            viewModel.Dispose();
        }
    }
}