using Amrv.ConfigurableCompany.content.unity;
using UnityEngine;
using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content.display.customObject
{
    public sealed class BorderGameObject
    {
        public readonly GameObject Container;
        private readonly RectTransform Container_Rect;

        private readonly RectTransform Top_Rect;
        private readonly RectTransform Bottom_Rect;
        private readonly RectTransform Left_Rect;
        private readonly RectTransform Right_Rect;

        private readonly Image Top_Image;
        private readonly Image Bottom_Image;
        private readonly Image Left_Image;
        private readonly Image Right_Image;

        private float _size = 1;
        public float Size
        {
            get => _size; set
            {
                if (_size == value) return;

                Top_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
                Bottom_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
                Left_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
                Right_Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);

                _size = value;
            }
        }

        private float _padding = 0;
        public float Padding
        {
            get => _padding; set
            {
                if (_padding == value) return;

                Container_Rect.offsetMin = new(value, value);
                Container_Rect.offsetMax = new(-value, -value);

                _padding = value;
            }
        }

        private Color _color = Color.white;
        public Color Color
        {
            get => _color;
            set
            {
                if (_color == value) return;

                Top_Image.color = value;
                Bottom_Image.color = value;
                Left_Image.color = value;
                Right_Image.color = value;

                _color = value;
            }
        }

        public BorderGameObject(string name, GameObject parent = null)
        {
            Container = UnityObject.Create(name)
                .AddComponent(out Container_Rect);

            if (parent != null)
            {
                SetParent(parent.transform);
            }

            Container_Rect.anchorMin = new(0, 0);
            Container_Rect.anchorMax = new(1, 1);
            Container_Rect.offsetMin = new(0, 0);
            Container_Rect.offsetMax = new(0, 0);

            UnityObject.Create("Top")
                .SetParent(Container_Rect)
                .AddComponent(out Top_Rect)
                .AddComponent(out Top_Image);

            Top_Rect.pivot = new(.5f, 1);
            Top_Rect.anchorMin = new(0, 1);
            Top_Rect.anchorMax = new(1, 1);
            Top_Rect.offsetMin = new(0, 0);
            Top_Rect.offsetMax = new(0, 0);

            UnityObject.Create("Bottom")
                .SetParent(Container_Rect)
                .AddComponent(out Bottom_Rect)
                .AddComponent(out Bottom_Image);

            Bottom_Rect.pivot = new(.5f, 0);
            Bottom_Rect.anchorMin = new(0, 0);
            Bottom_Rect.anchorMax = new(1, 0);
            Bottom_Rect.offsetMin = new(0, 0);
            Bottom_Rect.offsetMax = new(0, 0);

            UnityObject.Create("Left")
                .SetParent(Container_Rect)
                .AddComponent(out Left_Rect)
                .AddComponent(out Left_Image);

            Left_Rect.pivot = new(0, .5f);
            Left_Rect.anchorMin = new(0, 0);
            Left_Rect.anchorMax = new(0, 1);
            Left_Rect.offsetMin = new(0, 0);
            Left_Rect.offsetMax = new(0, 0);

            UnityObject.Create("Right")
                .SetParent(Container_Rect)
                .AddComponent(out Right_Rect)
                .AddComponent(out Right_Image);

            Right_Rect.pivot = new(1, .5f);
            Right_Rect.anchorMin = new(1, 0);
            Right_Rect.anchorMax = new(1, 1);
            Right_Rect.offsetMin = new(0, 0);
            Right_Rect.offsetMax = new(0, 0);
        }

        public void SetParent(Transform transform, bool worldPositionStays = false)
        {
            Container.transform.SetParent(transform, worldPositionStays);
        }

        public static implicit operator GameObject(BorderGameObject self)
        {
            return self.Container;
        }
    }
}
