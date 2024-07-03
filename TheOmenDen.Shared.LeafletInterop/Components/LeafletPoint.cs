namespace TheOmenDen.Shared.LeafletInterop.Components;

public readonly record struct LeafletPoint
{
    private LeafletPoint(double Latitude, double Longitude)
    {
        this.Latitude = Latitude;
        this.Longitude = Longitude;
    }

    public static LeafletPoint Create(double latitude, double longitude)
    {
        return new LeafletPoint(latitude, longitude);
    }

    public static LeafletPoint Default => Create(51.505, -0.09);
    public double Latitude { get; init; }
    public double Longitude { get; init; }

    public override string ToString() => $"[{Latitude}, {Longitude}]";

    public static implicit operator double[](LeafletPoint point) => new double[] { point.Latitude, point.Longitude };

    public static implicit operator LeafletPoint(double[] point) => Create(point[0], point[1]);

    public static implicit operator LeafletPoint((double Latitude, double Longitude) point) => Create(point.Latitude, point.Longitude);

    public static implicit operator (double Latitude, double Longitude)(LeafletPoint point) => (point.Latitude, point.Longitude);

    public static LeafletPoint operator +(LeafletPoint point1, LeafletPoint point2) => Create(point1.Latitude + point2.Latitude, point1.Longitude + point2.Longitude);
    public static LeafletPoint operator -(LeafletPoint point1, LeafletPoint point2) => Create(point1.Latitude - point2.Latitude, point1.Longitude - point2.Longitude);
    public static LeafletPoint operator *(LeafletPoint point, double factor) => Create(point.Latitude * factor, point.Longitude * factor);
    public static LeafletPoint operator /(LeafletPoint point, double factor) => Create(point.Latitude / factor, point.Longitude / factor);

    public double DistanceTo(LeafletPoint other)
    {
        var dLat = Latitude - other.Latitude;
        var dLong = Longitude - other.Longitude;
        return Math.Sqrt(Math.Pow(dLat, 2) + Math.Pow(dLong, 2));
    }

    public LeafletPoint Scale(double factor) => Create(Latitude * factor, Longitude * factor);

    public void Deconstruct(out double Latitude, out double Longitude)
    {
        Latitude = this.Latitude;
        Longitude = this.Longitude;
    }
}