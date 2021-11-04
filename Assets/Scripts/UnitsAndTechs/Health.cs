using System;
using UnitsAndTechs;

namespace DefaultNamespace
{
    public class Health
    {
        private IPlaceable element;

        public IPlaceable Element => element;

        //For serialization
        public Health()
        {
            
        }

        private Health(int startingAmount, int maxAmount, IPlaceable element)
        {
            AssignElement(element);
            Element.Health = this;
            CurrentAmount = startingAmount;
            MaxAmount = maxAmount;
        }

        private void AssignElement(IPlaceable element)
        {
            if (element == null) {
                throw new Exception("Element cannot be null");
            }
            
            var oldHealth = element.Health;
            if (oldHealth != null)
            {
                oldHealth.RemoveElement();
            }

            element.Health = this;
            this.element = element;
        }

        private void RemoveElement()
        {
            element = null;
        }

        public static Health CreateAndAssign(int startingAmount, int maxAmount, IPlaceable element)
        {
            if (element == null) {
                throw new Exception("Element cannot be null");
            }

            return new Health(startingAmount, maxAmount, element);
        }

        public void AddHealth(int amount)
        {
            CurrentAmount += amount;
            if (CurrentAmount > MaxAmount)
            {
                CurrentAmount = MaxAmount;
            }
        }
        
        public void RemoveHealth(int amount)
        {
            CurrentAmount -= amount;
            if (CurrentAmount <= 0)
            {
                Element.Destroyed();
            }
        }

        public float Percentage => CurrentAmount / (float) MaxAmount;

        public bool NotFull => MaxAmount > CurrentAmount;
        
        public int CurrentAmount { get; set; }
        
        public int MaxAmount { get; set; }
        
    }
}