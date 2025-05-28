using System;
using System.Globalization;
using System.Resources;
using System.Windows.Markup;
using System.Windows.Data;
using System.Threading;

namespace Version_3._0
{
    public class LocExtension : MarkupExtension
    {
        private readonly string _key;
        private static readonly ResourceManager _resourceManager = new ResourceManager("Version_3._0.Ressources.string", typeof(LocExtension).Assembly);

        public LocExtension(string key)
        {
            _key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var culture = Thread.CurrentThread.CurrentUICulture;
            return _resourceManager.GetString(_key, culture) ?? $"[{_key}]";
        }
    }
}
