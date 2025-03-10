using System.Collections.Generic;
using Nekki.Common.LifecycleService;
using Zenject;

namespace Nekki.Infrastructure.Common.LifecycleService
{
    public class ZenjectLifecycleSystem : LifecycleSystem
    {
        [Inject(Optional = true, Source = InjectSources.Local)]
        public ZenjectLifecycleSystem(
            List<IInitable> init,
            List<IUpdatable> update,
            List<ILateUpdatable> late,
            List<IFixedUpdatable> fix) 
            : base(init, update, late, fix) {}
    }
}