using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace DCx.Webshop.Setup
{
    public class StringLocalizer : IStringLocalizer
    {
        private ResourceManager resourceManager;

        public StringLocalizer()
        {
            var resAss = Assembly.Load(new AssemblyName("DCx.res.Webshop"));
            resourceManager = new ResourceManager("DCx.res.Webshop.Resources.Resources", resAss);
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = resourceManager.GetString(name, Thread.CurrentThread.CurrentCulture);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = resourceManager.GetString(name, Thread.CurrentThread.CurrentCulture);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new System.NotImplementedException();
        }
    }
}
