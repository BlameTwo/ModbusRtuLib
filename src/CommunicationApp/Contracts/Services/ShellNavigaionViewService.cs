using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationApp.Contracts.Services.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationApp.Contracts.Services
{
    public class ShellNavigaionViewService : NavigationViewServiceBase
    {
        public ShellNavigaionViewService(
            [FromKeyedServices(ProgramLife.ShellNavigationKey)]
                INavigationService navigationService,
            IPageService pageService
        )
            : base(navigationService, pageService) { }
    }
}
