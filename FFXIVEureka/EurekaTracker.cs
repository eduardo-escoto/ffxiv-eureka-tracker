using Dalamud.Game.Command;
using Dalamud.Game.ClientState;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Logging;
using System.IO;
using System.Reflection;
using Lumina.Excel.GeneratedSheets;
using Dalamud.Interface.Windowing;
using FFXIVEureka.Windows;

namespace FFXIVEureka
{
    public class EurekaTracker : IDalamudPlugin
    {
        public string Name => "FFXIV Eureka";
        private const string CommandName = "/eurekatrack";
        private DalamudPluginInterface PluginInterface { get; init; }
        public static EurekaTracker Plugin { get; private set; }

        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("SamplePlugin");
        
        

        public EurekaTracker(DalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<Service>();
            //FFXIVClientStructs.Resolver.Initialize(Service.SigScanner.SearchBase);

            Plugin = this;

            this.PluginInterface = pluginInterface;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);


            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            WindowSystem.AddWindow(new ConfigWindow(this));
            WindowSystem.AddWindow(new MainWindow(this, goatImage));

            Service.Commands.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            var territory = Service.Data.Excel.GetSheet<TerritoryType>().GetRow(Service.ClientState.TerritoryType);
            //var name = Service.Data.Excel.GetSheet<PlaceName>().GetRow(territory.PlaceName.Row).Name;
            PluginLog.Log($"You are in territory: {territory.PlaceName.Value.Name}, {territory.PlaceNameRegion.Value.Name}, {territory.TerritoryIntendedUse}");
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            Service.Commands.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            WindowSystem.GetWindow("My Amazing Window").IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            PluginLog.Log("Opening Config", new string[] {});
            WindowSystem.GetWindow("Eureka Config").IsOpen = true;
        }
    }
}
