using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationApp.Contracts.Services.Navigation;

namespace CommunicationApp.Contracts.Services
{
    public class ShellNavigationService : NavigationServiceBase
    {
        public ShellNavigationService(IPageService pageService)
            : base(pageService) { }
    }
}
