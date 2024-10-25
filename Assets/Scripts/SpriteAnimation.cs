using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [System.Serializable]
        public class SpriteClip
        {
            public string name;
            public bool loop;
            public Sprite[] sprites;
            public bool allowNext;
            public UnityEvent onComplete;
        }

        [SerializeField] private int _frameRate;
        [SerializeField] private string _initialClipName;



        [SerializeField] private SpriteClip[] _clips;
        private Dictionary<string, SpriteClip> _clipsDictionary = new Dictionary<string, SpriteClip>();

        private SpriteRenderer _renderer;
        private float _secondsPerFrame;
        private int _currentSpriteIndex;
        private float _nextFrameTime;

        [SerializeField] private bool _isPlaying;
        private SpriteClip _currentClip;

        private void Start()
        {

            _renderer = GetComponent<SpriteRenderer>();
            StartAnimation();

            foreach (var clip in _clips)
            {
                _clipsDictionary[clip.name] = clip;
            }

            if (!string.IsNullOrEmpty(_initialClipName))
            {
                SetClip(_initialClipName);
            }
        }

        private void StartAnimation()
        {
            _secondsPerFrame = 1f / _frameRate;
            _nextFrameTime = Time.time;
        }


        private void Update()
        {
            if (!_isPlaying || _nextFrameTime > Time.time) return;

            if (_currentSpriteIndex >= _currentClip.sprites.Length)
            {
                if (_currentClip.loop)
                {
                    _currentSpriteIndex = 0;
                }
                else
                {
                    _isPlaying = false;
                    _currentClip.onComplete?.Invoke();
                    if (_currentClip.allowNext)
                    {
                        string nextClipName = GetNextClipName(_initialClipName);
                        if (!string.IsNullOrEmpty(nextClipName))
                        {
                            SetClip(nextClipName);
                        }
                    }
                    return;
                }
            }

            _renderer.sprite = _currentClip.sprites[_currentSpriteIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentSpriteIndex++;
        }


        private string GetNextClipName(string currentClipName)
        {
            for (int i = 0; i < _clips.Length; i++)
            {
                if (_clips[i].name == currentClipName && i < _clips.Length - 1)
                {
                    return _clips[i + 1].name;
                }
            }

            return null;
        }
        public void SetClip(string name)
        {
            if (_clipsDictionary.TryGetValue(name, out var sprites))
            {
                _currentClip = sprites;
                _currentSpriteIndex = 0;
                _isPlaying = true;
                _nextFrameTime = Time.time;
            }
            else
            {
                Debug.LogWarning($"Sprite clip not found: {name}");
            }
        }
    }
}