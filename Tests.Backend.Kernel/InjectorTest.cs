using System;
using Avace.Backend.Kernel.Injection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Utils;
using Tests.Utils.Injection;

namespace Tests.Backend.Kernel
{
    [TestClass]
    public class InjectorTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Injector.Initialize();
        }

        [TestMethod]
        public void ShouldGetService()
        {
            var serviceForTest = new ServiceForInjectorTest();
            using (new InjectorReplacer<IServiceForInjectorTest>(serviceForTest))
            {
                Injector.Get<IServiceForInjectorTest>().Should().Be(serviceForTest);
            }
        }

        [TestMethod]
        public void ShouldFailToGetNonExistentService()
        {
            Action get = () => Injector.Get<IServiceForInjectorTest>();
            get.Should().Throw<Ninject.ActivationException>();
        }

        [TestMethod]
        public void ShouldTryGetService()
        {
            var serviceForTest = new ServiceForInjectorTest();
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
            var serviceForTest1 = new ServiceForInjectorTest();
            var serviceForTest2 = new ServiceForInjectorTest();
            using (new InjectorReplacer<IServiceForInjectorTest>(serviceForTest1, serviceForTest2))
            {
                Injector.GetAll<IServiceForInjectorTest>().Should()
                    .BeEquivalentTo(new[] {serviceForTest1, serviceForTest2});
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