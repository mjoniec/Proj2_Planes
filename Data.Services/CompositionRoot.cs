using Data.Repositories;
using LightInject;

namespace Data.Services
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IRequestRepository>(factory => new RequestRepository(new RequestContext()), new PerRequestLifeTime());
            serviceRegistry.Register<IMarkRepository>(factory => new MarkRepository(new MarkContext()), new PerRequestLifeTime());
            serviceRegistry.Register<IMarkService, MarkService>(new PerRequestLifeTime());
            serviceRegistry.Register<IGoldService, GoldService>(new PerContainerLifetime());
            serviceRegistry.Register<IRequestService, RequestService>(new PerRequestLifeTime());
        }
    }
}
