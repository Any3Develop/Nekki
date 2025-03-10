using System;

namespace Nekki.Game.Abilities.Data
{
    /// <summary>
    /// How the ability will be manipulated by the player and the game's environment.
    /// </summary>
    [Flags]
    public enum AbilityType
    {
        /// <summary>
        /// Defines the interactivity of the ability as Active use.
        /// </summary>
        Active = 0,
        
        /// <summary>
        /// Defines the interactivity of the ability as Passive use.
        /// </summary>
        Passive = 2,
    }
}