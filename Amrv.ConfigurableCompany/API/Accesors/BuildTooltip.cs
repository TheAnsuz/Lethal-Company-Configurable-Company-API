using System;
using System.Collections.ObjectModel;

namespace Amrv.ConfigurableCompany.API.Accesors
{
    public sealed class BuildTooltip
    {
        private readonly string Value;

        private BuildTooltip(string value)
        {
            Value = value;
        }

        public static BuildTooltip Create(params string[] lines)
        {
            return lines;
        }

        public static implicit operator BuildTooltip(string text)
        {
            return new BuildTooltip(text);
        }

        public static implicit operator BuildTooltip(string[] lines)
        {
            return new BuildTooltip(string.Join("\n", lines));
        }

        public static implicit operator string[](BuildTooltip tooltip)
        {
            return tooltip.Value.Split('\n');
        }

        public static implicit operator string(BuildTooltip tooltip)
        {
            return tooltip.Value;
        }

        public override string ToString()
        {
            return $"BuildTooltip[text: {Value}]";
        }
    }
}
