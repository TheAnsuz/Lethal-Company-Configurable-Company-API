using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Amrv.ConfigurableCompany.Core.Display.Scripts
{
    public class UIRegion : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<PointerEventData> OnEnter;
        public event Action<PointerEventData> OnExit;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnExit?.Invoke(eventData);
        }
    }
}
