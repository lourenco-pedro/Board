namespace App
{
    [System.Serializable]
    public struct MovementPattern
    {
        public string Name;
        public bool Inclusive;
        public PathDirection[] NeighborhoodSettings;
    }
    
    [System.Serializable]
    public struct PathDirection
    {
        public Direction Direction;
        public int Length;
    }
}