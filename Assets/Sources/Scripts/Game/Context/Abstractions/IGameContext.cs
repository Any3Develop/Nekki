using Nekki.Common.DependencyInjection;
using Nekki.Game.Entities.Player.Abstractions;
using Nekki.Game.Scenarios.Data;

namespace Nekki.Game.Context.Abstractions
{
    public interface IGameContext
    {
        IPlayerViewModel Player { get; }
        IServiceProvider ServiceProvider { get; }
        int Time { get; }
        int TimeLeft { get; }
        
        int UnitsTotalMax { get; }
        int UnitsSceneMax { get; }
        int UnitsAlive { get; }
        int UnitsSpawned { get; }
        int UnitsDied { get; }
        int UnitsLeft { get; }
        bool Initialized { get; }
        
        
        void Pause(bool value);
        void Start(ScenarioConfig scenarioCfg, IPlayerViewModel player);
        void End();
    }
}