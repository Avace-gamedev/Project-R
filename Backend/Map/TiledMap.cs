using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Avace.Backend.Interfaces.Logging;
using Avace.Backend.Interfaces.Map;
using Avace.Backend.Interfaces.Math;
using Avace.Backend.Kernel.Injection;

namespace Avace.Backend.Map
{
    internal static class TiledMapBuilder
    {
        private static readonly ICustomLogger Log = Injector.Get<ICustomLoggerProvider>()
            .GetLogger(MethodBase.GetCurrentMethod().Name);

        /// <summary>
        ///     Create a map from a tiled file (.tmx)
        /// </summary>
        /// <param name="path">Path should be absolute or relative to executing assembly.</param>
        /// <returns></returns>
        public static Map FromPath(string path)
        {
            if (!File.Exists(path)) throw MakeParseException(path, "File not found");

            string extension = Path.GetExtension(path);
            if (extension == "tmx")
                throw MakeParseException(path, $"File is not a valid Tiled map, expected a .tmx file but found .{extension}");

            XDocument doc = XDocument.Load(path);

            if (doc.Root == null) throw MakeParseException(path, "Could not load map: Root is null");

            try
            {
                int width = GetAttributeValueAsInt(doc.Root, "width");
                int height = GetAttributeValueAsInt(doc.Root, "height");
                int tileWidth = GetAttributeValueAsInt(doc.Root, "tilewidth");
                int tileHeight = GetAttributeValueAsInt(doc.Root, "tileheight");

                Map result = new Map(width, height);

                XElement tileset = doc.Root.Element("tileset")
                                   ?? throw new InvalidOperationException("Could not find tag <tileset>");

                int firstGid = GetAttributeValueAsInt(tileset, "firstgid");

                foreach (XElement layer in doc.Root.Elements("layer")) ParseLayer(layer, result, firstGid);

                foreach (XElement objectGroup in doc.Root.Elements("objectgroup"))
                    ParseObjectGroup(objectGroup, result, tileWidth, tileHeight);

                return result;
            }
            catch (InvalidOperationException e)
            {
                throw MakeParseException(path, e.Message);
            }
        }

        private static void ParseLayer(XElement layer, Map result, int firstGid)
        {
            string layerName = GetAttributeValue(layer, "name");

            if (result.LayersInternal.Any(l => l.Name == layerName))
                throw new InvalidOperationException($"Layer with name {layerName} already exists");

            int layerOrder = GetPropertyValueAsInt(layer, "layer-order");
            bool collision = GetPropertyValueAsBool(layer, "collision");

            string layerData = GetChildContent(layer, "data");
            int?[] tiles = layerData.Split(',')
                .Select(t => t.Trim())
                .Select(t =>
                    int.TryParse(t, out int i)
                        ? i
                        : throw new InvalidOperationException($"Could not convert tile to int: {t}"))
                .Select(i => i < firstGid ? (int?)null : i - firstGid)
                .ToArray();

            result.LayersInternal.Add(new MapLayer(layerName, layerOrder, tiles, collision));
        }

        private static void ParseObjectGroup(XElement objectGroup, Map result, int tileWidth, int tileHeight)
        {
            foreach (XElement obj in objectGroup.Elements("object"))
            {
                string? type = TryGetAttributeValue(obj, "type");
                switch (type)
                {
                    case "player_spawn":
                        double x = GetAttributeValueAsDouble(obj, "x");
                        double y = GetAttributeValueAsDouble(obj, "y");

                        result.PlayerSpawn = new Vector2Int((int)(x / tileWidth), (int)(y / tileHeight));
                        break;
                    case "area":
                        ParseArea(result, obj);
                        break;
                    case null:
                        Log.Warn($"Found object with no type attribute: {obj}");
                        break;
                    default:
                        Log.Warn($"Object with type {type} ignored.");
                        break;
                }
            }
        }

        private static void ParseArea(Map result, XElement obj)
        {
            MapArea area = new MapArea
            {
                Name = GetPropertyValue(obj, "area-name"),
                MaxGroups = GetPropertyValueAsInt(obj, "area-max-groups"),
            };

            XElement? polygon = obj.Element("polygon");
            if (polygon == null)
            {
                Log.Error($"Ignoring area {area.Name} because it doesn't define a shape.");
                return;
            }

            string polygonStr = GetAttributeValue(polygon, "points");

            List<Vector2Int> polygonVec = new List<Vector2Int>();

            foreach (string pointStr in polygonStr.Split(' '))
            {
                string[] coords = pointStr.Split(',');
                if (coords.Length != 2)
                {
                    Log.Error($"Ignoring area {area.Name} because polygon string is invalid: {polygonStr}");
                    return;
                }

                string xStr = coords[0].Trim();
                string yStr = coords[1].Trim();

                double xDouble = Convert.ToDouble(xStr, CultureInfo.InvariantCulture);
                double yDouble = Convert.ToDouble(yStr, CultureInfo.InvariantCulture);

                Vector2Int point = new Vector2Int((int)xDouble, (int)yDouble);
                polygonVec.Add(point);
            }

            area.Shape = polygonVec.ToArray();

            result.AreasInternal.Add(area);
        }

        private static string GetChildContent(XElement element, string childName)
        {
            return element.Element(childName)?.Value
                   ?? throw new InvalidOperationException($"Could not find child '{childName}' on element <{element.Name}>");
        }

        private static string GetAttributeValue(XElement element, string attribute)
        {
            return TryGetAttributeValue(element, attribute)
                   ?? throw new InvalidOperationException($"Could not find attribute '{attribute}' on element <{element.Name}>");
        }

        private static string? TryGetAttributeValue(XElement element, string attribute)
        {
            return element.Attribute(attribute)?.Value;
        }

        private static int GetAttributeValueAsInt(XElement element, string attribute)
        {
            string attrStr = GetAttributeValue(element, attribute);

            if (!int.TryParse(attrStr, out int attrInt))
                throw new InvalidOperationException($"Could not convert value to int: {attrStr}");

            return attrInt;
        }

        private static double GetAttributeValueAsDouble(XElement element, string attribute)
        {
            string attrStr = GetAttributeValue(element, attribute);

            if (!double.TryParse(attrStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double attrDouble))
                throw new InvalidOperationException($"Could not convert value to double: {attrStr}");

            return attrDouble;
        }

        private static string GetPropertyValue(XElement element, string property)
        {
            return TryGetPropertyValue(element, property)
                   ?? throw new InvalidOperationException($"Could not find element property with name {property}");
        }

        private static string? TryGetPropertyValue(XElement element, string property)
        {
            XElement? properties = element.Element("properties");
            if (properties == null) return null;

            return properties.Elements("property")
                .Where(propertyElement => TryGetAttributeValue(propertyElement, "name") == property)
                .Select(propertyElement => TryGetAttributeValue(propertyElement, "value"))
                .FirstOrDefault();
        }

        private static int GetPropertyValueAsInt(XElement element, string property)
        {
            string propStr = GetPropertyValue(element, property);

            if (!int.TryParse(propStr, out int propInt))
                throw new InvalidOperationException($"Could not convert value to int: {propStr}");

            return propInt;
        }

        private static bool GetPropertyValueAsBool(XElement element, string property, bool defaultIfNotFound = false)
        {
            string? propStr = TryGetPropertyValue(element, property);

            return !string.IsNullOrEmpty(propStr) && bool.TryParse(propStr, out bool propBool) ? propBool : defaultIfNotFound;
        }

        private static Exception MakeParseException(string path, string message)
        {
            return new InvalidOperationException($"Error while parsing map {path}: {message}");
        }
    }
}
