using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.Core.Display.Scripts
{
    internal class NoDrawGraphic : Graphic
    {
        public override void SetMaterialDirty() { }
        public override void SetVerticesDirty() { }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}
