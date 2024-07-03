namespace TheOmenDen.Shared.LeafletInterop.Components;

/// <summary>
/// A representation of a quadrilateral boundary in a Leaflet map in pixel coordinates
/// </summary>
public readonly record struct LeafletBounds
{
    /// <summary>
    /// The south-west corner of the boundary
    /// </summary>
    /// <value>A <see cref="LeafletPoint"/></value>
    public LeafletPoint Southwest { get; }
    /// <summary>
    /// The north-east corner of the boundary
    /// </summary>
    /// <value>A <see cref="LeafletPoint"/></value>
    public LeafletPoint Northeast { get; }

    private LeafletPoint Min => LeafletPoint.Create(Southwest.Latitude, Northeast.Longitude);
    private LeafletPoint Max => LeafletPoint.Create(Northeast.Latitude, Southwest.Longitude);

    /// <summary>
    /// Creates a boundary from the provided <paramref name="southwest"/> and <paramref name="northeast"/> coordinate pairs
    /// </summary>
    /// <param name="southwest">The first corner</param>
    /// <param name="northeast">The second corner</param>
    private LeafletBounds(LeafletPoint southwest, LeafletPoint northeast)
    {
        Southwest = southwest;
        Northeast = northeast;
    }

    public static LeafletBounds Create(LeafletPoint southwest, LeafletPoint northeast) => new(southwest, northeast);

    /// <summary>
    /// Creates a boundary from the provided <paramref name="points"/>
    /// </summary>
    /// <param name="points">The provided points to contain</param>
    /// <exception cref="ArgumentException"></exception>
    private LeafletBounds(LeafletPoint[] points)
    {
        if (points?.Length == 0)
        {
            throw new ArgumentException("At least one point is required to create a bounds.", nameof(points));
        }

        var minLat = points.Min(p => p.Latitude);
        var minLong = points.Min(p => p.Longitude);
        var maxLat = points.Max(p => p.Latitude);
        var maxLong = points.Max(p => p.Longitude);

        Southwest = LeafletPoint.Create(minLat, minLong);
        Northeast = LeafletPoint.Create(maxLat, maxLong);
    }

    public static LeafletBounds Create(LeafletPoint[] points) => new(points);

    /// <summary>
    /// The width of the boundary in pixels
    /// </summary>
    public double Width => Northeast.Longitude - Southwest.Longitude;
    /// <summary>
    /// The height of the boundary in pixels
    /// </summary>
    public double Height => Northeast.Latitude - Southwest.Latitude;

    /// <summary>
    /// Determines if this bounded quadrilateral contains the provided <paramref name="point"/>
    /// </summary>
    /// <param name="point">The point to check</param>
    /// <returns><see langword="true"/> if the point falls in the boundary; <see langword="false" /> otherwise</returns>
    public bool Contains(LeafletPoint point) =>
        point.Latitude >= Southwest.Latitude
        && point.Latitude <= Northeast.Latitude
        && point.Longitude >= Southwest.Longitude
        && point.Longitude <= Northeast.Longitude;

    /// <summary>
    /// Determines if the provided <paramref name="bounds"/> are contained within this boundary
    /// </summary>
    /// <param name="bounds">The boundary to check</param>
    /// <returns><see langword="true"/> if the point falls in the boundary; <see langword="false" /> otherwise</returns>
    public bool Contains(LeafletBounds bounds) =>
        bounds.Southwest.Latitude >= Southwest.Latitude
        && bounds.Northeast.Latitude <= Northeast.Latitude
        && bounds.Southwest.Longitude >= Southwest.Longitude
        && bounds.Northeast.Longitude <= Northeast.Longitude;

    /// <summary>
    /// Calculates the center of this boundary 
    /// </summary>
    /// <param name="round">If rounding logic provided by <see cref="System.Math.Round"/> should be used</param>
    /// <returns>The center expressed as a <see cref="LeafletPoint"/></returns>
    public LeafletPoint Center(bool round = false) => round
    ? LeafletPoint.Create(Math.Round((Southwest.Latitude + Northeast.Latitude) * 0.5d),
        Math.Round((Southwest.Longitude + Northeast.Longitude) * 0.5d))
    : LeafletPoint.Create((Southwest.Latitude + Northeast.Latitude) * 0.5d,
        (Southwest.Longitude + Northeast.Longitude) * 0.5d);

    /// <summary>
    /// The south-west corner of the boundary
    /// </summary>
    /// <returns>The bottom-left point of the bounds</returns>
    public LeafletPoint GetBottomLeft() => Southwest;
    /// <summary>
    /// The north-east corner of the boundary
    /// </summary>
    /// <returns>The top-right point of the bounds</returns>
    public LeafletPoint GetTopRight() => Northeast;
    /// <summary>
    /// The south-east corner of the boundary
    /// </summary>
    /// <returns>The top-left point of the bounds (example: <see cref="Min"/>)</returns>
    public LeafletPoint GetTopLeft() => LeafletPoint.Create(Southwest.Latitude, Northeast.Longitude);
    /// <summary>
    /// The north-west corner of the boundary
    /// </summary>
    /// <returns>The bottom right corner of the bounds (example: <see cref="Max"/>)</returns>
    public LeafletPoint GetBottomRight() => LeafletPoint.Create(Northeast.Latitude, Southwest.Longitude);

    /// <summary>
    /// Gets the size of the boundary
    /// </summary>
    /// <returns>A <see cref="LeafletPoint"/> describing the <see cref="LeafletBounds.Width"/> and <seealso cref="LeafletBounds.Height"/></returns>
    public LeafletPoint GetSize() => LeafletPoint.Create(Width, Height);

    /// <summary>
    /// Extends the bounds to contain the given <paramref name="point"/>
    /// </summary>
    /// <param name="point">The provided point</param>
    /// <returns>The new <see cref="LeafletBounds"/> that encompass the <paramref name="point"/></returns>
    public LeafletBounds Extend(LeafletPoint point)
    {
        var south = Math.Min(Southwest.Latitude, point.Latitude);
        var west = Math.Min(Southwest.Longitude, point.Longitude);
        var north = Math.Max(Northeast.Latitude, point.Latitude);
        var east = Math.Max(Northeast.Longitude, point.Longitude);

        return Create(LeafletPoint.Create(south, west),
            LeafletPoint.Create(north, east));
    }

    /// <summary>
    /// Extend the current bounds to contain the given <paramref name="bounds"/>
    /// </summary>
    /// <param name="bounds">The new boundaries to contain</param>
    /// <returns>A boundary area that contains the provided <paramref name="bounds"/></returns>
    public LeafletBounds Extend(LeafletBounds bounds) =>
        Create(LeafletPoint.Create(Math.Min(Southwest.Latitude, bounds.Southwest.Latitude),
                Math.Min(Southwest.Longitude, bounds.Southwest.Longitude)),
            LeafletPoint.Create(Math.Max(Northeast.Latitude, bounds.Northeast.Latitude),
                Math.Max(Northeast.Longitude, bounds.Northeast.Longitude)));

    /// <summary>
    /// Calculates if the supplied <paramref name="other"/> have at least one point in common with this boundary
    /// </summary>
    /// <param name="other">The boundary to check against</param>
    /// <returns><see langword="true"/> when the boundaries have at least one point in common. <see langword="false"/> otherwise</returns>
    public bool Intersects(LeafletBounds other) =>
    !(other.Southwest.Longitude > Northeast.Longitude
        || other.Northeast.Longitude < Southwest.Longitude
        || other.Southwest.Latitude > Northeast.Latitude
        || other.Northeast.Latitude < Southwest.Latitude
        );

    /// <summary>
    /// Determines if the provided <paramref name="other"/> boundary overlaps with this boundary
    /// </summary>
    /// <param name="other">The boundary to check against</param>
    /// <returns><see langword="true"/> when the intersection is an area. <see langword="false"/> otherwise</returns>
    public bool Overlaps(LeafletBounds other)
    {
        if (!Intersects(other))
        {
            return false;
        }

        var intersectionSouthWest = LeafletPoint.Create(Math.Max(Southwest.Latitude, other.Southwest.Latitude),
            Math.Max(Southwest.Longitude, other.Southwest.Longitude));

        var intersectionNorthEast = LeafletPoint.Create(Math.Min(Northeast.Latitude, other.Northeast.Latitude),
            Math.Min(Northeast.Longitude, other.Northeast.Longitude));

        return intersectionSouthWest.Latitude < intersectionNorthEast.Latitude
            && intersectionSouthWest.Longitude < intersectionNorthEast.Longitude;
    }

    /// <summary>
    /// Creates a new <see cref="LeafletBounds"/> that has been expanded or retracted by the provided <paramref name="bufferRatio"/>
    /// </summary>
    /// <param name="bufferRatio">The ratio provided to expand or retract the bounds</param>
    /// <returns>A <see cref="LeafletBounds"/> that has been expanded or retracted by the provided <paramref name="bufferRatio"/></returns>
    public LeafletBounds Pad(double bufferRatio) =>
        Create(LeafletPoint.Create(Southwest.Latitude - Height * bufferRatio,
            Southwest.Longitude - Width * bufferRatio),
        LeafletPoint.Create(Northeast.Latitude + Height * bufferRatio,
            Northeast.Longitude + Width * bufferRatio));

}