using UnityEngine;

//-----------------------------------------------------------------------------
// Copyright 2015-2016 RenderHeads Ltd.  All rights reserverd.
//-----------------------------------------------------------------------------

namespace RenderHeads.Media.AVProVideo
{
	[AddComponentMenu("AVPro Video/Display IMGUI")]
	[ExecuteInEditMode]
	public class DisplayIMGUI : MonoBehaviour
	{
		public MediaPlayer	_mediaPlayer;

		public bool			_displayInEditor = true;
		public ScaleMode	_scaleMode	= ScaleMode.ScaleToFit;
		public Color		_color		= Color.white;
		public bool			_alphaBlend	= false;

		public bool			_fullScreen	= true;
		public int 			_depth		= 0;
		[Range(0f, 1f)]
		public float		_x			= 0.0f;
		[Range(0f, 1f)]
		public float		_y			= 0.0f;
		[Range(0f, 1f)]
		public float		_width		= 1.0f;
		[Range(0f, 1f)]
		public float		_height		= 1.0f;

		private static int	_propAlphaPack;
		private static int	_propVertScale;
		private Shader		_shaderAlphaPacking;
		private Material	_material;

		void Awake()
		{
			if (_propAlphaPack == 0)
			{
				_propAlphaPack = Shader.PropertyToID("AlphaPack");
				_propVertScale = Shader.PropertyToID("_VertScale");
			}
		}

		void Start()
		{
			// Disabling this lets you skip the GUI layout phase.
			this.useGUILayout = false;

			if (_shaderAlphaPacking == null)
			{
				_shaderAlphaPacking = Shader.Find("AVProVideo/IMGUI/Texture Transparent");
			}
		}

		void Update()
		{
			if (_mediaPlayer != null)
			{
				Shader currentShader = null;
				if (_material != null)
				{
					currentShader = _material.shader;
				}
				Shader nextShader = null;
				switch (_mediaPlayer.m_AlphaPacking)
				{
					case AlphaPacking.None:
						break;
					case AlphaPacking.LeftRight:
					case AlphaPacking.TopBottom:
						nextShader = _shaderAlphaPacking;
						break;
				}

				if (currentShader != nextShader)
				{
					if (_material != null)
					{
#if UNITY_EDITOR
						Material.DestroyImmediate(_material);
#else
						Material.Destroy(_material);
#endif
						_material = null;
					}

					if (nextShader != null)
					{
						_material = new Material(nextShader);
					}
				}

				// Apply changes for alpha videos
				if (_material != null && _material.HasProperty(_propAlphaPack))
				{
					Helper.SetupAlphaPackedMaterial(_material, _mediaPlayer.m_AlphaPacking);
				}
			}
		}

		void OnGUI()
		{
			if (_mediaPlayer == null)
			{
				return;
			}

			bool requiresVerticalFlip = false;
			Texture texture = null;

			if (_displayInEditor)
			{
				texture = Texture2D.whiteTexture;
			}

			if (_mediaPlayer.Info != null && !_mediaPlayer.Info.HasVideo())
			{
				texture = null;
			}

			if (_mediaPlayer.TextureProducer != null)
			{
				if (_mediaPlayer.TextureProducer.GetTexture() != null)
				{
					texture = _mediaPlayer.TextureProducer.GetTexture();
					requiresVerticalFlip = _mediaPlayer.TextureProducer.RequiresVerticalFlip();
				}
			}

			if (texture != null)
			{
				if (!_alphaBlend || _color.a > 0)
				{
					GUI.depth = _depth;
					GUI.color = _color;

					Rect rect = GetRect();

					switch (_mediaPlayer.m_AlphaPacking)
					{
						case AlphaPacking.None:
							if (requiresVerticalFlip)
							{
								GUIUtility.ScaleAroundPivot(new Vector2(1f, -1f), new Vector2(0f, rect.y + (rect.height / 2f)));
							}
							GUI.DrawTexture(rect, texture, _scaleMode, _alphaBlend);
							break;
						case AlphaPacking.LeftRight:
						case AlphaPacking.TopBottom:
							if (requiresVerticalFlip)
							{
								_material.SetFloat(_propVertScale, -1f);
							}
							else
							{
								_material.SetFloat(_propVertScale, 1f);
							}
							Helper.DrawTexture(rect, texture, _scaleMode, _mediaPlayer.m_AlphaPacking, _material);
							break;
					}
				}
			}
		}

		public Rect GetRect()
		{
			Rect rect;
			if (_fullScreen)
			{
				rect = new Rect(0.0f, 0.0f, Screen.width, Screen.height);
			}
			else
			{
				rect = new Rect(_x * (Screen.width - 1), _y * (Screen.height - 1), _width * Screen.width, _height * Screen.height);
			}

			return rect;
		}
	}
}
