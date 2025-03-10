using System.Collections.Generic;
using Nekki.Game.Context.Data.Spawn;
using Nekki.Game.Context.Spawn;
using Nekki.Game.Scenarios.Data;

namespace Nekki.Game.Context.Abstractions
{
    public interface ISpawnPointProvider
    {
        void Start(ScenarioConfig config);
        void End();

        void Add(SpawnPoint point, bool defaultParent = true);
        void Remove(SpawnPoint point);
        void Remove(SpawnId id);
        SpawnPoint Get(SpawnId id);
        SpawnPoint[] GetAll();
        SpawnPoint Get(string groupId, int index, List<SpawnId> ids, FunctionSelector selector);
        void Dispose();
    }
}