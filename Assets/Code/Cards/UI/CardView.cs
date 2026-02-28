using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.GameSystem;

namespace Game.Cards.UI
{
    public class CardView : MonoBehaviour
    {
        private CardViewModel viewModel;
        
        // Текстовые поля для отображения информации
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private TMP_Text stepText;
        
        // Контейнеры для включения/отключения в зависимости от наличия декораторов
        [SerializeField] private GameObject healthContainer;
        [SerializeField] private GameObject damageContainer;
        [SerializeField] private GameObject stepContainer;
        
        private void Start()
        {
            viewModel = new CardViewModel(GetComponent<CardData>());

            InitializeView();
            UpdateView();

            StepCombatSystem.Instance.EventUpdate += UpdateView;
            viewModel.UpdateUI += UpdateView;
            

        }
        
        // private void OnEnable()
        // {
        //     StepCombatSystem.Instance.EventUpdate += UpdateView;
        //     viewModel.UpdateUI += UpdateView;
        // }
        
        private void OnDisable()
        {
            StepCombatSystem.Instance.EventUpdate -= UpdateView;
            viewModel.UpdateUI -= UpdateView;
        }
        
        private void InitializeView()
        {
            if (healthContainer != null) healthContainer.SetActive(viewModel.isHealthDecorator);
            if (damageContainer != null) damageContainer.SetActive(viewModel.isAttackDecorator);
            if (stepContainer != null) stepContainer.SetActive(viewModel.isStepDecorator);
        }
        
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
            viewModel.UpdateUI -= UpdateView;
            viewModel.Dispose();
        }
    }
}