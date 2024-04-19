using System;

namespace Bookkeeping
{
    public abstract class Filter
    {
        private bool active;

        public bool Active
        {
            get => active;
            set
            {
                if (active != value)
                {
                    active = value;
                    OnFilterUpdated();
                }
            }
        }

        public event Action onFilterUpdated;

        public abstract bool AcceptEntry(Entry entry);

        protected void OnFilterUpdated()
        {
            onFilterUpdated?.Invoke();
        }
    }
}