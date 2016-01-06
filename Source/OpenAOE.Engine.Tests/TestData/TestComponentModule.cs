using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using OpenAOE.Engine.Tests.TestData.Components;

namespace OpenAOE.Engine.Tests.TestData
{
    class TestComponentModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISimpleComponent>().To<SimpleComponent>();
            Bind<IOtherSimpleComponent>().To<OtherSimpleComponent>();
        }
    }
}
