using System.Collections.Generic;
using Nekki.Game.Context.Data.Spawn;

namespace Nekki.Game.Context.Abstractions
{
    public interface IPositionProvider
    {
        void ResetAll();
        void Reset(string groupId);
        T Apply<T>(string groupId, int index, IList<T> ids, FunctionSelector selector);
    }
}