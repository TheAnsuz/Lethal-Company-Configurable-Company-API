using Amrv.ConfigurableCompany.API;
using Amrv.ConfigurableCompany.Core.Display.Items;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.Core.Display.Menu
{
    internal class MenuSections : IMenuPart
    {
        protected readonly MenuBind Bind;

        protected readonly Dictionary<CSection, MenuSection> _sections = [];

        internal MenuSections(MenuBind bind)
        {
            Bind = bind;
        }

        public void AddSection(CSection section)
        {
            MenuSection menuSection = MenuSection.CreateSection(Bind.Categories.GetCategory(section.Category).Content.transform, section);
            _sections[section] = menuSection;
        }

        public MenuSection GetSection(CSection section)
        {
            return _sections[section];
        }

        public void Destroy()
        {
            _sections.Clear();
        }

        public void UpdateContent()
        {
            foreach (CSection section in CSection.Storage.Values)
            {
                AddSection(section);
            }
        }

        public void UpdateSelf()
        {
        }
    }
}
