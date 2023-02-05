using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GGJRuntime
{
    public class ConditionalStatement : MonoBehaviour
    {
        [SerializeField]
        private SetModifierValueLogic modifierLogic = null;
        [SerializeField]
        private SetObjectValueLogic conditionalLogic = null;

        [field: SerializeField]
        public SoilType SelectedSoilType { get; private set; } = SoilType.None;
        [field: SerializeField]
        public ModifierType SelectedModifierType { get; private set; } = ModifierType.None;

        [SerializeField]
        private TMP_Dropdown _objectDropdown = null;
        [SerializeField]
        private TMP_Dropdown _modifierDropdown = null;

        private void OnObjectDropdownValueChanged(int i)
        {
            switch(_objectDropdown.options[i].text)
            {
                case "Dirt":
                    SelectedSoilType = SoilType.Dirt;
                    break;
                case "Rock":
                    SelectedSoilType = SoilType.Rock;
                    break;
                case "Magma":
                    SelectedSoilType = SoilType.Magma;
                    break;
                case "Water":
                    SelectedSoilType = SoilType.Water;
                    break;
                default:
                    SelectedSoilType = SoilType.None;
                    break;
            }
        }

        private void OnModifierDropdownValueChanged(int i)
        {
            switch (_modifierDropdown.options[i].text)
            {
                case "Life":
                    SelectedModifierType = ModifierType.VeryGood;
                    break;
                case "Happiness":
                    SelectedModifierType = ModifierType.Good;
                    break;
                case "Sorrow":
                    SelectedModifierType = ModifierType.Bad;
                    break;
                case "Death":
                    SelectedModifierType = ModifierType.VeryBad;
                    break;
                default:
                    SelectedModifierType = ModifierType.None;
                    break;
            }
        }


        private void OnModifierUpdated(ModifierType type)
        {
            SelectedModifierType = type;
        }


        private void OnConditionalUpdated(SoilType type)
        {
            SelectedSoilType = type;
        }


        private void OnDestroy()
        {
            modifierLogic.onUpdate -= OnModifierUpdated;
            conditionalLogic.onUpdate -= OnConditionalUpdated;
        }


        private void Start()
        {
            _objectDropdown.onValueChanged.AddListener(OnObjectDropdownValueChanged);
            _modifierDropdown.onValueChanged.AddListener(OnModifierDropdownValueChanged);

            modifierLogic.onUpdate += OnModifierUpdated;
            conditionalLogic.onUpdate += OnConditionalUpdated;
        }
    }
}
