using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.display.component
{
    public class RegionButton : Button
    {
        public event EventHandler<PointerEventData> OnMouseEnter;
        public event EventHandler<PointerEventData> OnMouseExit;

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
