using System.Text.Json.Serialization;

/// <summary>
/// Represents the top-level GeoJSON object returned by the USGS earthquake feed.
/// It contains a list of Features, where each Feature represents one earthquake.
/// </summary>
public class FeatureCollection
{
    public string Type { get; set; } = "";
    public List<Feature> Features { get; set; } = new();
}

/// <summary>
/// Represents a single earthquake event ("Feature") in the GeoJSON data.
/// Only the Properties are needed for this assignment.
/// </summary>
public class Feature
{
    public string Type { get; set; } = "";
    public Properties Properties { get; set; } = new();
}

/// <summary>
/// Represents the "properties" object of a single earthquake feature.
/// Only 'place' and 'mag' are needed for this assignment, but more
/// properties could be added here if needed in the future.
/// </summary>
public class Properties
{
    public double Mag { get; set; }
    public string Place { get; set; } = "";
}