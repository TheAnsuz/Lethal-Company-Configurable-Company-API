using Amrv.ConfigurableCompany.Core;
using Amrv.ConfigurableCompany.Core.Config;
using Amrv.ConfigurableCompany.Core.IO;
using System;
using System.Collections.Generic;

namespace Amrv.ConfigurableCompany.API
{
    public sealed class CConfig
    {
        private static readonly Dictionary<string, CConfig> _configs = [];
        public static readonly IDStorage<CConfig> Storage = new(_configs);

        public static CConfigBuilder Builder() => new();

        public readonly CCategory Category;
        public readonly CSection Section;

        public readonly bool Synchronized;
        public readonly bool Experimental;
        public readonly bool Toggleable;
        public readonly string ID;
        public readonly string Name;
        public readonly string Tooltip;
        public readonly CType Type;
        public readonly object Default;
        private object _value;
        public object Value
        {
            get => _value;
            set => TrySet(value);
        }
        private readonly bool _defaultEnabled;
        private bool _enabled = true;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (!Toggleable) return;
                if (_enabled == value) return;

                _enabled = value;
                ConfigEventRouter.OnToggle_Config(this, value);
            }
        }
        // Si esta en una seccion no debe pertenecer a una categoria, si esta en una categoria no debe pertenecer a una seccion
        internal CConfig(CConfigBuilder builder)
        {
            // Checks
            if (builder == null) throw new BuildingException("Tried to create configuration without builder");

            if (builder.ID == null) throw new BuildingException("Unable to build configuration without ID");

            if (builder.Type == null) throw new BuildingException($"Unable to build configuration {ID} without type");

            if (_configs.ContainsKey(builder.ID)) throw new BuildingException($"Tried to create a configuration with an existing ID ({builder.ID})");

            // Si esta dentro de una seccion
            if (builder.Section != null && CSection.Storage.TryGetValue(builder.Section, out CSection section))
                Section = section;
            // Sino, si esta dentro de una categoria
            else if (builder.Category != null && CCategory.Storage.TryGetValue(builder.Category, out CCategory category))
                Category = category;
            // Sino, error
            else
                throw new BuildingException("Tried to create configuration without section or category");

            // Sets
            ID = builder.ID;
            Name = builder.Name ?? "";
            Tooltip = builder.Tooltip;
            Type = builder.Type;

            Synchronized = builder.Synchronized;
            Experimental = builder.Experimental;
            Toggleable = builder.Toggleable;

            if (!TrySet(builder.Value ?? Type.Default, ChangeReason.CREATION))
                throw new BuildingException($"Can't create configuration with default value {builder.Value ?? Type.Default}[{(builder.Value == null ? "Def" : "Val")}] using type {Type.GetType().Name}");

            Default = builder.DefaultValue ?? Value;

            _defaultEnabled = !Toggleable || builder.Enabled;
            _enabled = _defaultEnabled;

            // Index itself
            _configs.Add(ID, this);
            Section?.AddConfig(this);
            Category?.AddConfig(this);

            // Notify
            IOController.GetConfigCache(this);
            ConfigEventRouter.OnCreate_Config(this);
        }

        public void Reset() => TrySet(Default, ChangeReason.SCRIPT_RESET);
        internal void Reset(ChangeReason reason)
        {
            TrySet(Default, reason);
            Enabled = _defaultEnabled;
        }

        public bool TrySet(object value, IFormatProvider format = null) => TrySet(value, ChangeReason.SCRIPT_CHANGE, format);

        internal bool TrySet(object value, ChangeReason reason, IFormatProvider format = null)
        {
            object old = _value;

            if (Type.IsValidValue(value))
            {
                _value = value;
                ConfigEventRouter.OnChange_Config(this, reason, old, value, true, false);
                return true;
            }

            if (Type.TryConvert(value, out object result, format))
            {
                _value = result;
                ConfigEventRouter.OnChange_Config(this, reason, old, value, true, true);
                return true;
            }

            ConfigEventRouter.OnChange_Config(this, reason, old, value, false, false);
            return false;
        }

        public bool TryGet<T>(out T result) => Type.TryGetAs(Value, out result);
        public T Get<T>() => Get(default(T));
        public T Get<T>(T @defaut)
        {
            if (Type.TryGetAs(Value, out T result))
                return result;
            return @defaut;
        }

        internal bool SerializeValue(out string data)
        {
            return Type.Serialize(Value, out data);
        }

        internal bool DeserializeValue(in string data, ChangeReason reason)
        {
            if (Type.Deserialize(in data, out object value))
                return TrySet(value, reason);
            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is CConfig other && other.ID.Equals(ID);
        }

        public override int GetHashCode()
        {
            return 879213648 | ID.GetHashCode();
        }

        public override string ToString()
        {
            return $"Config[{ID}:{Name}:{_value}]";
        }
    }
}
