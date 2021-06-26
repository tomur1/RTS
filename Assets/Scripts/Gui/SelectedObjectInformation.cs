using System;
using TMPro;
using UnitsAndTechs;
using UnitsAndTechs.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Gui
{
    public class SelectedObjectInformation : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ObjectName;
        [SerializeField] private TextMeshProUGUI Groups;
        [SerializeField] private TextMeshProUGUI AttackPower;
        [SerializeField] private TextMeshProUGUI AttackSpeed;
        [SerializeField] private TextMeshProUGUI AttackRange;
        [SerializeField] private Slider HealthBar;
        private IPlaceable selectedObject;

        public void UpdateView(Unit worker)
        {
            selectedObject = worker;
            String objectNameText = "";
            String groupsText = "";
            String attackPowerText = "";
            String attackSpeedText = "";
            String attackRangeText = "";
            if (worker == null)
            {
                EmptyView();
            }
            else
            {
                objectNameText = worker.GetType().Name;
                groupsText = worker.GroupsNumberString();
                attackPowerText = "";
                attackSpeedText = "";
                attackRangeText = "";
            }
            
            SetAllValues(objectNameText, groupsText, attackPowerText, attackSpeedText, attackRangeText);
        }
        
        public void UpdateView(Soldier soldier)
        {
            selectedObject = soldier;
            String objectNameText = "";
            String groupsText = "";
            String attackPowerText = "";
            String attackSpeedText = "";
            String attackRangeText = "";
            if (soldier == null)
            {
                EmptyView();
            }
            else
            {
                objectNameText = soldier.GetType().Name;
                groupsText = soldier.GroupsNumberString();
                attackPowerText = "Attack Power: " + soldier.AttackAbility.attackPower;
                attackSpeedText = "Attack Speed:" + soldier.AttackAbility.GetAttackSpeed();
                attackRangeText = "Attack Range:" + soldier.AttackAbility.range;
            }
            
            SetAllValues(objectNameText, groupsText, attackPowerText, attackSpeedText, attackRangeText);
        }

        public void UpdateView(Building building)
        {
            selectedObject = building;
            String objectNameText = "";
            String groupsText = "";
            String attackPowerText = "";
            String attackSpeedText = "";
            String attackRangeText = "";
            if (building == null)
            {
                EmptyView();
            }
            else
            {
                objectNameText = building.GetType().Name;
                groupsText = "";
                attackPowerText = "";
                attackSpeedText = "";
                attackRangeText = "";
            }
            SetAllValues(objectNameText, groupsText, attackPowerText, attackSpeedText, attackRangeText);
        }

        public void SetAllValues(String objectNameText, String groupsText,
            String attackPowerText, String attackSpeedText, String attackRangeText)
        {
            ObjectName.SetText(objectNameText);
            Groups.SetText(groupsText);
            AttackPower.SetText(attackPowerText);
            AttackRange.SetText(attackSpeedText);
            AttackSpeed.SetText(attackRangeText);
        }

        public void EmptyView()
        {
            SetAllValues("No objects selected", "", "", "", "");
            selectedObject = null;
        }

        private void Update()
        {
            if (selectedObject != null)
            {
                HealthBar.value = selectedObject.Health.Percentage;
            }
        }
    }
}
