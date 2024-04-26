using UnityEngine.UI;

namespace Amrv.ConfigurableCompany.content
{
    public class MenuController : Graphic
    {
        public override void SetMaterialDirty() { }
        public override void SetVerticesDirty() { }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}
