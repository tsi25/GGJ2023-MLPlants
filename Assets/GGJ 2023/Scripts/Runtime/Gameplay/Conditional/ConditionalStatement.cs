using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GGJRuntime
{
    public class ConditionalStatement : MonoBehaviour
    {
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

        }

        private void OnModifierDropdownValueChanged(int i)
        {
            
        }

        private void Start()
        {
            _objectDropdown.onValueChanged.AddListener(OnObjectDropdownValueChanged);
            _modifierDropdown.onValueChanged.AddListener(OnModifierDropdownValueChanged);
        }
    }
}
