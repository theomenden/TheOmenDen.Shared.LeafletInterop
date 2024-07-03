namespace TheOmenDen.Shared.LeafletInterop.Options;

public sealed class LeafletMapConfigurationOptions
{
    public const string SectionName = "Leaflet";

    public string? MapId { get; set; } = "mapId";
    public double Latitude { get; set; } = 51.505;
    public double Longitude { get; set; } = -0.09;
    public int Zoom { get; set; } = 13;
}