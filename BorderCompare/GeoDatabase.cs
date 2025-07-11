using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottReece.BorderCompare.App
{
    public class GeoDatabase
    {
        private string _jsonPath = @"..\..\..\..\data\countries.geojson";

        public async Task<List<GeoArea>> GetAllCountries()
        {
            var json = await File.ReadAllTextAsync(_jsonPath);
            var root = JsonConvert.DeserializeObject<Root>(json) ?? throw new Exception("No json object returned from parsing");

            var countries = new List<GeoArea>();
            foreach (var geoArea in root.Features)
            {
                var country = new GeoArea();
                country.Name = geoArea.Properties.Admin;
                foreach (var a in geoArea.Geometry.Coordinates)
                {
                    foreach (var b in a)
                    {
                        var shape = new Shape();
                        foreach (var c in b)
                        {
                            shape.BoundaryPoints.Add(new LatLong(c[1], c[0]));
                        }
                        country.Shapes.Add(shape);
                    }
                }
                countries.Add(country);
            }

            return countries;
        }

        private class Geometry
        {
            [JsonProperty("coordinates")]
            public List<List<List<List<double>>>> Coordinates { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        private class Properties
        {
            [JsonProperty("ADMIN")]
            public string Admin { get; set; }

            [JsonProperty("ISO_A2")]
            public string IsoA2 { get; set; }

            [JsonProperty("ISO_A3")]
            public string IsoA3 { get; set; }
        }

        private class Feature
        {
            [JsonProperty("geometry")]
            public Geometry Geometry { get; set; }

            [JsonProperty("properties")]
            public Properties Properties { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        private class Root
        {
            [JsonProperty("features")]
            public List<Feature> Features { get; set; }
        }
    }

    public class GeoArea
    {
        public string Name;
        public List<Shape> Shapes = new();
    }

    public class Shape
    {
        public List<LatLong> BoundaryPoints = new();
    }

    public class LatLong
    {
        public double Latitude;
        public double Longitude;

        public LatLong()
        {
        }

        public LatLong(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
