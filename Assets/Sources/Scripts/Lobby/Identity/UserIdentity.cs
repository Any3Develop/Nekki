using System.Collections.Generic;
using System.Linq;
using Nekki.Lobby.Identity.Abstractions;

namespace Nekki.Lobby.Identity
{
    public class RedirectionCollection : List<IRedirectionArg>
    {
        public T GetArg<T>() where T : IRedirectionArg => (T) this.FirstOrDefault(x => x is T);
        public T[] GetArgs<T>() where T : IRedirectionArg => this.OfType<T>().ToArray();
    }

    public static class UserIdentity // For example user identity
    {
        public static readonly RedirectionCollection Redirections = new();
    }
}