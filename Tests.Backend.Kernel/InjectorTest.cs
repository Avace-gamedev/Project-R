using System;
using Avace.Backend.Kernel.Injection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Tests.Utils.Injection;

namespace Tests.Backend.Kernel
{
    [TestClass]
    public class InjectorTest
    {
        [TestMethod]
        public void ShouldGetService()
        {
            ServiceForInjectorTest serviceForTest = new ServiceForInjectorTest();
            using (new InjectorReplacer<IServiceForInjectorTest>(serviceForTest))
            {
                Injector.Get<IServiceForInjectorTest>().Should().Be(serviceForTest);
            }
        }

        [TestMethod]
        public void ShouldFailToGetNonExistentService()
        {
            Action get = () => Injector.Get<IServiceForInjectorTest>();
            get.Should().Throw<ActivationException>();
        }

        [TestMethod]
        public void ShouldTryGetService()
        {
            ServiceForInjectorTest serviceForTest = new ServiceForInjectorTest();
            using (new InjectorReplacer<IServiceForInjectorTest>(serviceForTest))
            {
                Injector.TryGet<IServiceForInjectorTest>().Should().Be(serviceForTest);
            }
        }

        [TestMethod]
        public void ShouldReturnNullIfTryGetNonExistentService()
        {
            Injector.TryGet<IServiceForInjectorTest>().Should().BeNull();
        }

        [TestMethod]
        public void ShouldGetAllServices()
        {
            ServiceForInjectorTest serviceForTest1 = new ServiceForInjectorTest();
            ServiceForInjectorTest serviceForTest2 = new ServiceForInjectorTest();
            using (new InjectorReplacer<IServiceForInjectorTest>(serviceForTest1, serviceForTest2))
            {
                Injector.GetAll<IServiceForInjectorTest>().Should()
                    .BeEquivalentTo(new[] { serviceForTest1, serviceForTest2 });
            }
        }

        [TestMethod]
        public void ShouldGetAllNonExistentServices()
        {
            Injector.GetAll<IServiceForInjectorTest>().Should().BeEmpty();
        }
    }

    internal interface IServiceForInjectorTest
    {
    }

    internal class ServiceForInjectorTest : IServiceForInjectorTest
    {
    }
}