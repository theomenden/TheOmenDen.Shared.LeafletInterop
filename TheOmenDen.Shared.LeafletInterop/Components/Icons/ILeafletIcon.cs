namespace TheOmenDen.Shared.LeafletInterop.Components.Icons;

public interface ILeafletIcon : ILeafletShadowIcon, ILeafletAnchorIcon
{
    string IconUrl { get; }
    string IconRetinaUrl { get; }
    LeafletPoint IconSize { get; }
    string ClassName { get; }
}

public interface ILeafletAnchorIcon
{
    LeafletPoint IconAnchor { get; }
    LeafletPoint PopupAnchor { get; }
    LeafletPoint TooltipAnchor { get; }
    LeafletPoint ShadowAnchor { get; }
}

public interface ILeafletShadowIcon
{
    string? ShadowUrl { get; }
    string? ShadowRetinaUrl { get; }
    LeafletPoint ShadowSize { get; }
}