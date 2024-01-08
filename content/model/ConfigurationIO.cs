using Amrv.ConfigurableCompany.content.model.data;
using System;
using System.IO;

namespace Amrv.ConfigurableCompany.content.model
{
    public sealed class ConfigurationIO
    {
        private ConfigurationIO() { }

        public static void ReadAll() => ReadAll(GameNetworkManager.Instance?.currentSaveFileName ?? "UnknownFile");
        public static void ReadAll(string file)
        {
            ConfigurableCompanyPlugin.Info("ReadAll from file " + file);

            if (!File.Exists(file))
            {
                // Set to default
                foreach (Configuration config in Configuration.Configs)
                {
                    config.Reset(ChangeReason.CONFIGURATION_CREATED);
                }
                return;
            }

            string content;
            try
            {
                content = File.ReadAllText(file);
            }
            catch (Exception ex)
            {
                throw new IOException($"Unable to read configuration from file {file}", ex);
            }

            ConfigurationBundle bundle = new DefaultConfigurationBundle();

            try
            {
                bundle.Unpack(content);
            }
            catch (Exception ex)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex2)
                {
                    throw new IOException($"Unable to remove invalid configuration file", ex2);
                }
                throw new IOException($"Unable to unpack configuration bundle", ex);
            }

            foreach (Configuration config in Configuration.Configs)
            {
                if (bundle.TryGet(config.ID, out string value))
                    config.TrySet(value, ChangeReason.READ_FROM_FILE);
            }
        }

        public static void SaveAll() => SaveAll(GameNetworkManager.Instance?.currentSaveFileName ?? "UnknownFile");
        public static void SaveAll(string file)
        {
            ConfigurableCompanyPlugin.Info("SaveAll to file " + file);

            ConfigurationBundle bundle = new DefaultConfigurationBundle();

            // Get all the contents of the file so configs that are not currently loaded dont get deleted
            string existingContent;
            try
            {
                existingContent = File.Exists(file) ? File.ReadAllText(file) : "";

                bundle.Unpack(existingContent);
            }
            catch (Exception ex)
            {
                throw new IOException($"Unable to read configuration from file {file}", ex);
            }
            finally
            {
                // Save current configs replacing those that already exist
                foreach (Configuration config in Configuration.Configs)
                {
                    bundle.Add(config.ID, config.Value?.ToString() ?? "");
                }

                try
                {
                    string content = bundle.Pack();

                    File.WriteAllText(file, content);
                }
                catch (Exception ex)
                {
                    throw new IOException($"Can't save config to file {file}", ex);
                }
            }
        }

        public static void RemoveFile() => RemoveFile(GameNetworkManager.Instance?.currentSaveFileName ?? "UnknownFile");
        public static void RemoveFile(string file)
        {
            if (File.Exists(file))
                File.Delete(file);
        }
    }
}
