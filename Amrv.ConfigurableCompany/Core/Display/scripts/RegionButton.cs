using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Scripts
{
    public class RegionButton : Button
    {
        public static readonly Navigation DEFAULT_NAVIGATION = new Navigation()
        {
            mode = Navigation.Mode.None
        };
        protected override void Start()
        {
            base.Start();
            navigation = DEFAULT_NAVIGATION;
        }

        public event EventHandler<PointerEventData> OnMouseEnter;
        public event EventHandler<PointerEventData> OnMouseExit;
        public event EventHandler<PointerEventData> OnMouseClick;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnMouseClick?.Invoke(this, eventData);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            OnMouseEnter?.Invoke(this, eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            OnMouseExit?.Invoke(this, eventData);
        }
    }
}
