using System;
using System.Linq;
using Avace.Backend.Kernel.Injection;

namespace Tests.Utils.Injection
{
    public class InjectorReplacer<T> : IDisposable
    {
        private readonly T[] _oldValues;

        public InjectorReplacer(T replacementValue, params T[] additionalValues)
            : this(new[] {replacementValue}.Concat(additionalValues).ToArray())
        {
        }

        protected InjectorReplacer(params T[] values)
        {
            _oldValues = Injector.GetAll<T>().ToArray();
            Injector.RemoveAll<T>();
            Injector.Bind(values);
        }

        public void Dispose()
        {
            Injector.RemoveAll<T>();
            Injector.Bind(_oldValues);
            GC.SuppressFinalize(this);
        }
    }
}
