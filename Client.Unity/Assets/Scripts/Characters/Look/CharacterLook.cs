using System;
using Characters.Enums;
using Characters.Utils;
using Configuration.Character.Look;
using Extensions;
using Misc;
using UnityEngine;

namespace Characters.Look
{
    public class CharacterLook : CustomMonoBehaviour
    {
        private static readonly int TileIndexShaderProperty = Shader.PropertyToID("TileIndex");
        private static readonly int TileSizeShaderProperty = Shader.PropertyToID("TileSize");

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
            if (characterLookConfiguration == null)
            {
                Logger.Warn($"No {typeof(CharacterLookConfiguration)} provided.");
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
            _renderer.material = new Material(GlobalConstants.Get().textureMapShader);
            _renderer.material.SetVector(TileSizeShaderProperty, (Vector2)characterLookConfiguration.spriteSize);

            float spriteSheetRatio = (float)characterLookConfiguration.spriteSheet.width / (float)characterLookConfiguration.spriteSheet.height;
            float spriteRatio = (float)characterLookConfiguration.spriteSize.x / (float)characterLookConfiguration.spriteSize.y;

            float scaleY = characterLookConfiguration.sizeModifier;
            float scaleX = characterLookConfiguration.sizeModifier * spriteRatio / spriteSheetRatio;

            transform.localScale = new Vector3(scaleX, scaleY, 1);

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
                Logger.Warn("Animation has no frames, will not animate this character");
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
                Logger.Warn("Animation speed has been set to 0, it will be ignored. 1 is used instead.");
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