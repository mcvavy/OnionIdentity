using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleInjector;
using SimpleInjector.Diagnostics;

namespace Web.Common
{
    public static class Transient
    {
        public static void RegisterDisposableTransient<TService, TImplementation>(
            this Container c)
            where TImplementation : class, IDisposable, TService
            where TService : class
        {
            var scoped = Lifestyle.Scoped;
            var r = Lifestyle.Transient.CreateRegistration<TService, TImplementation>(c);
            r.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "ignore");
            c.AddRegistration(typeof(TService), r);
            c.RegisterInitializer<TImplementation>(o => scoped.RegisterForDisposal(c, o));
        }
    }
}