using Dalamud.Game.Command;
using Dalamud.Game.ClientState;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Logging;
using System.IO;
using System.Reflection;
using Dalamud.Interface.Windowing;
using FFXIVEureka.Windows;

namespace FFXIVEureka
{
    public sealed class Plugin : IDalamudPlugin
    {

        // var territory = Service.Data.Excel.GetSheet<TerritoryType>().GetRow(Service.ClientState.TerritoryType);
        // var name = Service.Data.Excel.GetSheet<PlaceName>().GetRow(territory.PlaceName).Name;

        public string Name => "FFXIV Eureka";
        private const string CommandName = "/eurekatrack";
        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }

        private ClientState cstate {get; init; }

        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("SamplePlugin");
        
        

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

        
            // PluginLog.Log(ClientState.TerritoryType.get());
            
            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            WindowSystem.AddWindow(new ConfigWindow(this));
            WindowSystem.AddWindow(new MainWindow(this, goatImage));

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
            // ClientState.equals();
            // PluginLog.Log(ClientState.TerritoryType, new string[] {});
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
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
