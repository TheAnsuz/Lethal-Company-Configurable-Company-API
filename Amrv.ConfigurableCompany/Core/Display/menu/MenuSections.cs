using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Items;
using Amrv.ConfigurableCompany.Plugin;
using Amrv.ConfigurableCompany.Utils;
using System.Collections;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuSections : IMenuPart
    {
        protected Reference<MenuBind> Bind;

        protected Dictionary<CSection, MenuSection> _sections = [];

        internal MenuSections(Reference<MenuBind> bind)
        {
            Bind = bind;
        }

        public void AddSection(CSection section)
        {
            MenuSection menuSection = MenuSection.CreateSection(Bind.Item.Categories.Item.GetCategory(section.Category).Content.transform, section);
            _sections[section] = menuSection;
        }

        public MenuSection GetSection(CSection section)
        {
            return _sections[section];
        }

        public void Destroy()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuSections deletion in progress ({_sections.Count} sections)");
            Bind = null;
            foreach (MenuSection section in _sections.Values)
            {
                section.Destroy();
            }
            _sections.Clear();
            _sections = null;
        }

        public IEnumerator UpdateContent()
        {
            foreach (CSection section in CSection.Storage.Values)
            {
                AddSection(section);
            }

            yield break;
        }

        public IEnumerator UpdateSelf()
        {
            yield break;
        }

#if DEBUG
        ~MenuSections()
        {
            ConfigurableCompanyPlugin.Debug($"[Destroy] MenuSections deleted");
        }
#endif
    }
}
