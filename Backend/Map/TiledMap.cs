using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Avace.Backend.Interfaces.Map;

namespace Avace.Backend.Map;

internal class TiledMap : IMap
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public List<string> Layers { get; } = new();
    public Dictionary<string, int?[]> LayerTiles { get; } = new();
    IEnumerable<string> IMap.Layers => Layers;

    public TiledMap(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public int? GetTerrainAt(int x, int y, string layer)
    {
        int?[] layerTiles = GetLayer(layer);
        int index = CoordsToIndex(x, y);
        
        return layerTiles[index];
    }

    private int?[] GetLayer(string layer)
    {
        if (!LayerTiles.ContainsKey(layer))
        {
            throw new InvalidOperationException($"Could not find layer {layer}");
        }

        return LayerTiles[layer];
    }

    private int CoordsToIndex(int x, int y)
    {
        return x + y * Width;
    }

    /// <summary>
    /// Create a map from a tiled file (.tmx)
    /// </summary>
    /// <param name="path">Path should be absolute or relative to executing assembly.</param>
    /// <returns></returns>
    public static TiledMap FromPath(string path)
    {
        if (!File.Exists(path))
        {
            throw MakeParseException(path , "File not found");
        }

        string extension = Path.GetExtension(path);
        if (extension == "tmx")
        {
            throw MakeParseException(path, $"File is not a valid Tiled map, expected a .tmx file but found .{extension}");
        }

        XDocument doc = XDocument.Load(path);

        if (doc.Root == null)
        {
            throw MakeParseException(path, "Could not load map: Root is null");
        }

        try
        {
            int width = GetAttributeValueAsInt(doc.Root, "width");
            int height = GetAttributeValueAsInt(doc.Root, "height");

            TiledMap result = new TiledMap(width, height);

            XElement tileset = doc.Root.Element("tileset") ?? throw new InvalidOperationException("Could not find tag <tileset>");

            int firstGid = GetAttributeValueAsInt(tileset, "firstgid");

            foreach (XElement layer in doc.Root.Elements("layer"))
            {
                string layerName = GetAttributeValue(layer, "name");

                if (result.Layers.Contains(layerName))
                {
                    throw new InvalidOperationException($"Layer with name {layerName} already exists");
                }


                string layerData = GetChildContent(layer, "data");
                int?[] tiles = layerData.Split(',')
                    .Select(t => t.Trim())
                    .Select(t =>
                        int.TryParse(t, out int i)
                            ? i
                            : throw new InvalidOperationException($"Could not convert tile to int: {t}"))
                    .Select(i => i < firstGid ? (int?)null : i - firstGid)
                    .ToArray();
                
                result.Layers.Add(layerName);
                result.LayerTiles.Add(layerName, tiles);
            }

            return result;
        }
        catch (InvalidOperationException e)
        {
            throw MakeParseException(path, e.Message);
        }
    }

    private static string GetChildContent(XElement element, string childName)
    {
        return element.Element(childName)?.Value
               ?? throw new InvalidOperationException($"Could not find child '{childName}' on element <{element.Name}>");
    }

    private static string GetAttributeValue(XElement element, string attribute)
    {
        return element.Attribute(attribute)?.Value
               ?? throw new InvalidOperationException($"Could not find attribute '{attribute}' on element <{element.Name}>");
    }

    private static int GetAttributeValueAsInt(XElement element, string attribute)
    {
        string attrStr = GetAttributeValue(element, attribute);

        if (!int.TryParse(attrStr, out int attrInt))
        {
            throw new InvalidOperationException($"Could not convert value to int: {attrStr}");
        }

        return attrInt;
    }

    private static Exception MakeParseException(string path, string message)
    {
        return new InvalidOperationException($"Error while parsing map {path}: {message}");
    }
}