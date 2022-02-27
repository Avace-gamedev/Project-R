using System;
using System.Reflection;
using Avace.Backend.Interfaces.Logging;
using Avace.Backend.Kernel.Injection;
using Characters.Enums;
using Characters.Utils;
using Configuration.Character.Look;
using Extensions;
using Misc;
using UnityEngine;
using ILogger = Avace.Backend.Interfaces.Logging.ILogger;

namespace Characters.Look
{
    public class CharacterLook : MonoBehaviour
    {
        private static readonly int TileIndexShaderProperty = Shader.PropertyToID("TileIndex");
        private static readonly int TileSizeShaderProperty = Shader.PropertyToID("TileSize");

        private static ILogger _log;

        public GameObject visualRoot;

        [Header("Character description")]
        public CharacterLookConfiguration characterLookConfiguration;

        private Character _character;
        private SpriteRenderer _renderer;

        private CharacterState _currentlyDisplayedState;
        private CharacterOrientation _currentlyDisplayedOrientation;
        private int[] _currentAnimationFrames;
        private int _currentAnimationFrameIndex;
        private DateTime? _nextFrameTime;

        private void Start()
        {
            _log = Injector.Get<ILoggerProvider>().GetLogger(MethodBase.GetCurrentMethod().DeclaringType?.Name!);

            if (characterLookConfiguration == null)
            {
                _log.Warn($"No {typeof(CharacterLookConfiguration)} provided.");
                enabled = false;
                return;
            }

            _character = gameObject.RequireComponent<Character>();
            
            if (visualRoot == null)
            {
                visualRoot = gameObject;
            }

            GameObject obj = visualRoot.CreateChildWithComponents("Body", typeof(SpriteRenderer));
            obj.transform.localPosition = Vector3.zero;
            _renderer = obj.GetComponent<SpriteRenderer>();

            Rect rect = Rect.MinMaxRect(0, 0, characterLookConfiguration.spriteSheet.width, characterLookConfiguration.spriteSheet.height);
            _renderer.sprite = Sprite.Create(characterLookConfiguration.spriteSheet, rect, characterLookConfiguration.pivot, rect.width, 0,
                SpriteMeshType.FullRect);
            _renderer.material = new Material(GlobalConstants.TextureMapShader);
            _renderer.material.SetVector(TileSizeShaderProperty, (Vector2)characterLookConfiguration.spriteSize);

            UpdateState(true);
        }

        private void Update()
        {
            if (characterLookConfiguration == null)
            {
                return;
            }
            
            UpdateState();
            
            if (_nextFrameTime != null && _nextFrameTime > DateTime.Now)
            {
                return;
            }

            if (_currentAnimationFrames.Length == 0)
            {
                _log.Warn("Animation has no frames, will not animate this character");
                return;
            }

            if (_currentAnimationFrameIndex < 0 || _currentAnimationFrameIndex >= _currentAnimationFrames.Length)
            {
                _currentAnimationFrameIndex = 0;
            }

            _renderer.material.SetInt(TileIndexShaderProperty, _currentAnimationFrames[_currentAnimationFrameIndex]);

            _currentAnimationFrameIndex++;

            float speedInFps = characterLookConfiguration.GetAnimationSpeed(_currentlyDisplayedState, _currentlyDisplayedOrientation);

            if (speedInFps == 0)
            {
                _log.Warn("Animation speed has been set to 0, it will be ignored. 1 is used instead.");
                speedInFps = 1;
            }

            _nextFrameTime = DateTime.Now.AddSeconds(1 / speedInFps);
        }

        private void UpdateState(bool force = false)
        {
            if (!force && _character.State == _currentlyDisplayedState && _character.Orientation == _currentlyDisplayedOrientation)
            {
                return;
            }

            _currentlyDisplayedState = _character.State;
            _currentlyDisplayedOrientation = _character.Orientation;
            _currentAnimationFrames = characterLookConfiguration.GetAnimationFrames(_currentlyDisplayedState, _currentlyDisplayedOrientation);
            _nextFrameTime = null; // force animation computation at next frame
        }

        private void OnValidate()
        {
            if (visualRoot == null)
            {
                GameObject visual = transform.Find("Visual")?.gameObject;
                if (!visual)
                {
                    visual = gameObject.CreateChild("Visual");
                }

                visualRoot = visual;
            }

            if (characterLookConfiguration == null)
            {
                gameObject.GetConfigurationFromCharacterConfiguration(config => config.look);
            }
        }
    }
}