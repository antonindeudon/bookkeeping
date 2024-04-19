using UnityEngine;
using UnityEngine.UI;

namespace Bookkeeping
{
    public abstract class FilterUI : MonoBehaviour
    {
        public Toggle toggle;

        protected abstract Filter Filter { get; }

        protected virtual void Awake()
        {
            toggle.onValueChanged.AddListener((value) => Filter.Active = value);
        }

    }
}