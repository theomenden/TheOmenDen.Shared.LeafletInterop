namespace TheOmenDen.Shared.LeafletInterop.Components.Providers;

public class DefaultLeafletCssProvider : ILeafletCssProvider
{
    public string MapContainerClass => "leaflet-container";
    public string MarkerClass => "leaflet-marker";
    public string PopupClass => "leaflet-popup";
}