namespace DefaultNamespace
{
    public class Health
    {
        public Health(int startingAmount, int maxAmount)
        {
            StartingAmount = startingAmount;
            CurrentAmount = startingAmount;
            MaxAmount = maxAmount;
        }

        public void AddHealth(int amount)
        {
            CurrentAmount += amount;
            if (CurrentAmount > MaxAmount)
            {
                CurrentAmount = MaxAmount;
            }
        }

        public bool NotFull => StartingAmount > CurrentAmount;

        public int StartingAmount { get; }
        public int CurrentAmount { get; set; }
        
        public int MaxAmount { get; set; }
        
    }
}