namespace DefaultNamespace
{
    public class Health
    {
        public Health(int startingAmount)
        {
            StartingAmount = startingAmount;
            CurrentAmount = startingAmount;
        }

        public int StartingAmount { get; }
        public int CurrentAmount { get; set; }
        
        
    }
}