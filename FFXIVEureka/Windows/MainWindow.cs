using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;
using Lumina.Excel.GeneratedSheets;
namespace FFXIVEureka.Windows;

public class MainWindow : Window, IDisposable
{
    private TextureWrap GoatImage;
    private EurekaTracker Plugin;
    private TerritoryType territory;

    public MainWindow(EurekaTracker plugin, TextureWrap goatImage) : base(
        "My Amazing Window", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.GoatImage = goatImage;
        this.Plugin = plugin;
    }

    public void Dispose()
    {
        this.GoatImage.Dispose();
    }

    public override void Draw()
    {
        ImGui.Text($"The random config bool is {this.Plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");

        if (ImGui.Button("Show Settings"))
        {
            this.Plugin.DrawConfigUI();
        }

        ImGui.Spacing();

        if (ImGui.Button("Get the location"))
        {
            this.territory = Service.Data.Excel.GetSheet<TerritoryType>().GetRow(Service.ClientState.TerritoryType);
        }
        ImGui.Text(this.territory != null? $"Location: {this.territory.PlaceName.Value.Name}, {this.territory.TerritoryIntendedUse}, {this.territory.Name.RawString}" : "");
        ImGui.Text("Have a goat:");
        ImGui.Indent(55);
        ImGui.Image(this.GoatImage.ImGuiHandle, new Vector2(this.GoatImage.Width, this.GoatImage.Height));
        ImGui.Unindent(55);
    }
}
