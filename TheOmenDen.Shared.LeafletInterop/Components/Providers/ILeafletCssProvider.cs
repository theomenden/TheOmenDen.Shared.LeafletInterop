namespace TheOmenDen.Shared.LeafletInterop.Components.Providers;

public interface ILeafletCssProvider
{
    string MapContainerClass { get; }
    string MarkerClass { get; }
    string PopupClass { get; }
}