using System.Windows;

namespace Texter.Localization
{
    public class LanguageChangedEventManager : WeakEventManager
    {
        public static void AddListener(TranslationManager source, IWeakEventListener listener) => CurrentManager.ProtectedAddListener(source, listener);

        public static void RemoveListener(TranslationManager source, IWeakEventListener listener) => CurrentManager.ProtectedRemoveListener(source, listener);

        private void OnLanguageChanged(object sender, System.EventArgs e) => DeliverEvent(sender, e);

        protected override void StartListening(object source) => ((TranslationManager)source).LanguageChanged += OnLanguageChanged;

        protected override void StopListening(object source) => ((TranslationManager)source).LanguageChanged -= OnLanguageChanged;

        private static LanguageChangedEventManager CurrentManager
        {
            get
            {
                System.Type managerType = typeof(LanguageChangedEventManager);
                var manager = (LanguageChangedEventManager)GetCurrentManager(managerType);
                if (manager == null)
                {
                    manager = new LanguageChangedEventManager();
                    SetCurrentManager(managerType, manager);
                }
                return manager;
            }
        }

    }
}
