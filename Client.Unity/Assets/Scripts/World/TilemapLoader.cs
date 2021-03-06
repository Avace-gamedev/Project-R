using System;
using System.Collections.Generic;
using System.Linq;
using Avace.Backend.Interfaces.Map;
using Avace.Backend.Kernel.Injection;
using Configuration;
using Extensions;
using Misc;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World
{
    /// <summary>
    /// This should be attached to a grid. It will load at initialization the map provided by the bound IMapProvider.
    /// </summary>
    public class TilemapLoader : CustomMonoBehaviour, IPlayerStartPositionProvider, ICoordinatesConverter
    {
        public TilemapConfiguration configuration;
        public Vector2Int PlayerStartPosition { get; private set; }

        private IMap _map;
        private Grid _grid;
        private readonly Dictionary<string, Tilemap> _tilemaps = new Dictionary<string, Tilemap>();

        protected override void RegisterInjectionBindings()
        {
            Injector.BindSingleton<IPlayerStartPositionProvider>(this);
            Injector.BindSingleton<ICoordinatesConverter>(this);
        }

        protected override void UnregisterInjectionBindings()
        {
            Injector.RemoveAll<IPlayerStartPositionProvider>();
            Injector.RemoveAll<ICoordinatesConverter>();
        }

        private void Awake()
            {
            if (configuration == null)
            {
                throw new InvalidOperationException($"Provided {nameof(TilemapConfiguration)} is null.");
            }

            _grid = gameObject.GetComponent<Grid>();
            if (_grid == null)
            {
                throw new InvalidOperationException("Expected game object to have a grid component");
            }

            IMapProvider tilemapProvider = Injector.TryGet<IMapProvider>();

            if (tilemapProvider == null)
            {
                throw new InvalidOperationException($"Could not find a {nameof(tilemapProvider)}");
            }

            _map = tilemapProvider.Get();

            PlayerStartPosition = new Vector2Int(_map.PlayerSpawn.X, _map.Height - _map.PlayerSpawn.Y);

            CreateTilemaps();
            ClearAndFillTilemaps();
        }

        private void CreateTilemaps()
        {
            // Layer with order 0 is the player's layer
            
            MapLayer[] layers = _map.Layers.ToArray();
            foreach (MapLayer layer in layers)
            {
                GameObject tilemapObject = gameObject.CreateChildWithComponents($"Tilemap_{layer.Name}", typeof(Tilemap), typeof(TilemapRenderer));
                tilemapObject.transform.position = layer.Order == 0 ? new Vector3(0, 0, -(float)layer.Order / 1000) : Vector3.zero;

                TilemapRenderer tilemapRenderer = tilemapObject.GetComponent<TilemapRenderer>();
                tilemapRenderer.mode = TilemapRenderer.Mode.Individual;
                tilemapRenderer.sortingOrder = layer.Order;

                if (layer.Collision)
                {
                    tilemapObject.AddComponent<TilemapCollider2D>();
                }

                Tilemap tilemap = tilemapObject.GetComponent<Tilemap>();

                _tilemaps.Add(layer.Name, tilemap);
            }
        }

        private void ClearAndFillTilemaps()
        {
            foreach (string layerName in _tilemaps.Keys)
            {
                Tilemap tilemap = _tilemaps[layerName];

                tilemap.ClearAllTiles();

                for (int x = 0; x < _map.Width; x++)
                for (int y = 0; y < _map.Height; y++)
                {
                    int? terrain = _map.GetTerrainAt(x, y, layerName);
                    TileBase tile = TerrainTypeToTile(terrain);
                    tilemap.SetTile(new Vector3Int(x, _map.Height - y, 0), tile);
                }

                tilemap.RefreshAllTiles();
            }
        }

        private TileBase TerrainTypeToTile(int? terrain)
        {
            return terrain.HasValue && terrain.Value > 0 && terrain.Value < configuration.tiles.Count ? configuration.tiles[terrain.Value] : null;
        }

        public Vector3 Convert(Vector2Int cell)
        {
            return _grid.CellToWorld((Vector3Int)cell);
        }

        public Vector2Int Convert(Vector3 world)
        {
            return (Vector2Int)_grid.WorldToCell(world);
        }
    }
}