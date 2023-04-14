using Tasks;

namespace Ads.Tasks
{
    public class TaskFullScreenAd
    {
        private readonly Awaiter _hiddenAwaiter = new();
        private readonly Awaiter<string> _loadedAwaiter = new();
        private readonly Awaiter _showedAwaiter = new();
        private ITask _hidden;
        private ITask<string> _loaded;
        private ITask<string> _showed;
        private IFullScreenAd _fullScreenAd;

        public TaskFullScreenAd()
        {
            _hidden = new AwaiterTask(_hiddenAwaiter);
        }

        public IFullScreenAd FullScreenAd
        {
            set
            {
                _fullScreenAd = value;
                value.Events.Hidden = OnHide;
                value.Events.Loaded = OnLoad;
                value.Events.Showed = OnShow;
            }
        }
        public ITask Hidden => _hidden;
        public ITask<string> Showed => _showed;

        private void Clear()
        {
            _fullScreenAd.Events.Hidden = null;
            _fullScreenAd.Events.Loaded = null;
            _fullScreenAd.Events.Showed = null;
            _fullScreenAd = null;
            _hiddenAwaiter.Clear();
            _loadedAwaiter.Clear();
            _showedAwaiter.Clear();
        }

        public ITask<string> LoadAsync()
        {
            _loadedAwaiter.Clear();
            _fullScreenAd.Load();
            return _loaded;
        }

        private void OnHide()
        {
            _hiddenAwaiter.Complete();
        }

        private void OnLoad(string message)
        {
            _loadedAwaiter.Complete(message);
        }

        private void OnShow()
        {
            _showedAwaiter.Complete();
        }

        public void Show()
        {
            _hiddenAwaiter.Clear();
            _showedAwaiter.Clear();
            _fullScreenAd.Show();
        }
    }
}