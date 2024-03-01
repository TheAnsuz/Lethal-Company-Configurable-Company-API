using Amrv.ConfigurableCompany.Core;
using UnityEngine;

namespace Amrv.ConfigurableCompany.API
{
    public sealed class CCategoryBuilder : InstanceBuilder<CCategory>
    {
        public string ID;
        public string Name;
        public Color Color;
        public string Page;
        public bool HideIfEmpty;

        public CPage CPage
        {
            set
            {
                Page = value?.ID ?? null;
            }
            get
            {
                if (CPage.Storage.TryGetValue(Page, out var page))
                {
                    return page;
                }
                return null;
            }
        }

        public (byte, byte, byte) ColorRGB
        {
            set
            {
                Color = new Color32(value.Item1, value.Item2, value.Item3, byte.MaxValue);
            }
            get
            {
                return ((byte)(Color.r * byte.MaxValue), (byte)(Color.g * byte.MaxValue), (byte)(Color.b * byte.MaxValue));
            }
        }

        public CCategoryBuilder SetID(string id)
        {
            ID = id;
            return this;
        }

        public CCategoryBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public CCategoryBuilder SetColor(Color color)
        {
            Color = color;
            return this;
        }

        public CCategoryBuilder SetColor(byte red, byte green, byte blue, byte alpha = 255)
        {
            Color = new Color32(red, green, blue, alpha);
            return this;
        }

        public CCategoryBuilder SetPage(CPage page)
        {
            Page = page.ID;
            return this;
        }

        public CCategoryBuilder SetPage(string pageId)
        {
            Page = pageId;
            return this;
        }

        public CCategoryBuilder SetHideIfEmpty(bool hide)
        {
            HideIfEmpty = hide;
            return this;
        }

        protected override CCategory BuildInstance()
        {
            CPage ??= CPage.Default;

            return new CCategory(this);
        }

        protected override bool TryGetExisting(out CCategory item)
        {
            return CCategory.Storage.TryGetValue(ID, out item);
        }
    }
}
