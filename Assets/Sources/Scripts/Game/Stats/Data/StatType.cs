namespace Nekki.Game.Stats.Data
{
    public enum StatType
    {
        None = 0,
        
        Heath = 20,
        Manna = 21,
        Armor = 22,
        Damage = 23,
        Stamina = 24,
        Lethality = 25,
        Level = 26,
        Range = 27,
        
        Heght = 40,
        Radius = 41,
        Priority = 42,
        Countdown = 43,
        StartDelay = 44,
        EndDelay = 45,
        Delay = 46,
        IgnoreLayers = 47,
        
        // Move Components
        MoveSpeed = 60,
        MoveAcceleration = 61,
        MoveDumpingFactor = 62,
        MoveTurnSpeed = 63,
        
        // Fly Components
        FlyAltitude = 80,
        FlyDumping = 81,
        FlyTrunDumping = 82,
    }
}