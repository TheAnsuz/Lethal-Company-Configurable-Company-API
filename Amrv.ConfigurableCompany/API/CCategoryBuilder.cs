using Amrv.ConfigurableCompany.API.Accesors;
using Amrv.ConfigurableCompany.Core;
using System;
using UnityEngine;

namespace Amrv.ConfigurableCompany.API
{
    public sealed class CCategoryBuilder : InstanceBuilder<CCategory>
    {
        public string ID;
        public string Name;
        public BuildColor Color;
        public BuildPage Page;
        public bool HideIfEmpty;

        [Obsolete("Use Page")]
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

        [Obsolete("Use Color")]
        public (byte, byte, byte) ColorRGB
        {
            set => Color = value;
            get => Color;
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
            Page ??= CPage.Default;

            return new CCategory(this);
        }

        protected override bool TryGetExisting(out CCategory item)
        {
            return CCategory.Storage.TryGetValue(ID, out item);
        }
    }
}
