using Amrv.ConfigurableCompany.API.Accesors;
using Amrv.ConfigurableCompany.Core;
using System;
using UnityEngine;

namespace Amrv.ConfigurableCompany.API
{
    public sealed class CCategoryBuilder : InstanceBuilder<CCategory>
    {
#pragma warning disable
        public string ID;
        public string Name;
        [Obsolete("Use BColor")]
        public Color Color;
        [Obsolete("Use BPage")]
        public string Page;
        public bool HideIfEmpty;

        public BuildPage BPage { get; set; }
        public BuildColor BColor { get; set; }

        [Obsolete("Use BPage")]
        public CPage CPage
        {
            set => BPage = value;
            get => BPage;
        }

        [Obsolete("Use BColor")]
        public (byte, byte, byte) ColorRGB
        {
            set => BColor = value;
            get => BColor;
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
            BColor = color;
            return this;
        }

        public CCategoryBuilder SetColor(byte red, byte green, byte blue, byte alpha = 255)
        {
            BColor = new Color32(red, green, blue, alpha);
            return this;
        }

        public CCategoryBuilder SetPage(CPage page)
        {
            BPage = page.ID;
            return this;
        }

        public CCategoryBuilder SetPage(string pageId)
        {
            BPage = pageId;
            return this;
        }

        public CCategoryBuilder SetHideIfEmpty(bool hide)
        {
            HideIfEmpty = hide;
            return this;
        }

        protected override CCategory BuildInstance()
        {
            if (Page != null)
                BPage ??= Page;

            BColor ??= Color;

            BPage ??= CPage.Default;

            return new CCategory(this);
        }

        protected override bool TryGetExisting(out CCategory item)
        {
            return CCategory.Storage.TryGetValue(ID, out item);
        }
#pragma warning enable
    }
}
