using System;
using TMPro;
using UnitsAndTechs;
using UnitsAndTechs.Units;
using UnityEngine;

namespace Gui
{
    public class SelectedObjectInformation : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ObjectName;
        [SerializeField] private TextMeshProUGUI Groups;
        [SerializeField] private TextMeshProUGUI AttackType;
        [SerializeField] private TextMeshProUGUI AttackPower;
        [SerializeField] private TextMeshProUGUI AttackSpeed;
        [SerializeField] private TextMeshProUGUI AttackRange;

        public void UpdateView(Unit worker)
        {
            String objectNameText = "";
            String groupsText = "";
            String attackTypeText = "";
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
                attackTypeText = "";
                attackPowerText = "";
                attackSpeedText = "";
                attackRangeText = "";
            }
            
            SetAllValues(objectNameText, groupsText, attackTypeText, attackPowerText, attackSpeedText, attackRangeText);
        }

        public void UpdateView(Building building)
        {
            String objectNameText = "";
            String groupsText = "";
            String attackTypeText = "";
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
                attackTypeText = "";
                attackPowerText = "";
                attackSpeedText = "";
                attackRangeText = "";
            }
            SetAllValues(objectNameText, groupsText, attackTypeText, attackPowerText, attackSpeedText, attackRangeText);
        }

        public void SetAllValues(String objectNameText, String groupsText, String attackTypeText,
            String attackPowerText, String attackSpeedText, String attackRangeText)
        {
            ObjectName.SetText(objectNameText);
            Groups.SetText(groupsText);
            AttackType.SetText(attackTypeText);
            AttackPower.SetText(attackPowerText);
            AttackRange.SetText(attackSpeedText);
            AttackSpeed.SetText(attackRangeText);
        }

        public void EmptyView()
        {
            SetAllValues("No objects selected", "", "", "", "", "");
        }
    }
}
