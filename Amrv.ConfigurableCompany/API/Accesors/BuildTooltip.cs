using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.API.Accesors
{
    public sealed class BuildTooltip
    {
        private readonly string Value;

        public BuildTooltip(string value)
        {
            Value = value;
        }

        public BuildTooltip(params string[] lines)
        {
            Value = string.Join('\n', lines);
        }

        public static BuildTooltip Create(params string[] lines)
        {
            return new(lines);
        }

        public static BuildTooltip Create(IEnumerable<string> lines)
        {
            return new(string.Join('\n', lines));
        }

        public static implicit operator BuildTooltip(string text)
        {
            return new BuildTooltip(text);
        }

        public static implicit operator BuildTooltip(string[] lines)
        {
            return new(lines);
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
