#if UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_5
	#define UNITY_FEATURE_UGUI
#endif

// Some older versions of Unity don't set the _TexelSize variable from uGUI so we need to set this manually
#if (!UNITY_5 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3_0 || UNITY_5_3_1 || UNITY_5_3_2 || UNITY_5_3_3)
	#define UNITY_UGUI_NOSET_TEXELSIZE
#endif

using System.Collections.Generic;
#if UNITY_FEATURE_UGUI
using UnityEngine;
using UnityEngine.UI;

//-----------------------------------------------------------------------------
// Copyright 2015-2016 RenderHeads Ltd.  All rights reserverd.
//-----------------------------------------------------------------------------

namespace RenderHeads.Media.AVProVideo
{
	[AddComponentMenu("AVPro Video/Display uGUI")]
	public class DisplayUGUI : UnityEngine.UI.MaskableGraphic
	{
		[SerializeField]
		public MediaPlayer _mediaPlayer;

		[SerializeField]
		public Rect m_UVRect = new Rect(0f, 0f, 1f, 1f);

		[SerializeField]
		public bool _setNativeSize = false;

		[SerializeField]
		public bool _keepAspectRatio = true;

		[SerializeField]
		public bool _noDefaultDisplay = true;

		[SerializeField]
		public Texture _defaultTexture;

		private int _lastWidth;
		private int _lastHeight;
		private bool _flipY;
		private Texture _lastTexture;
		private static int _propAlphaPack;
		private static int	_propVertScale;
#if  UNITY_UGUI_NOSET_TEXELSIZE
		private static int _propMainTextureTexelSize;
#endif

		protected override void Awake()
		{
			if (_propAlphaPack == 0)
			{
				_propAlphaPack = Shader.PropertyToID("AlphaPack");
				_propVertScale = Shader.PropertyToID("_VertScale");
#if UNITY_UGUI_NOSET_TEXELSIZE
				_propMainTextureTexelSize = Shader.PropertyToID("_MainTex_TexelSize");
#endif
			}
			base.Awake();
		}

		/// <summary>
		/// Returns the texture used to draw this Graphic.
		/// </summary>
		public override Texture mainTexture
		{
			get
			{
				Texture result = Texture2D.whiteTexture;
				if (HasValidTexture())
				{
					result = _mediaPlayer.TextureProducer.GetTexture();
				}
				else
				{
					if (_noDefaultDisplay)
					{
						result = null;
					}
					else if (_defaultTexture != null)
					{
						result = _defaultTexture;
					}
				}
				return result;
			}
		}

		public bool HasValidTexture()
		{
			return (_mediaPlayer != null && _mediaPlayer.TextureProducer != null && _mediaPlayer.TextureProducer.GetTexture() != null);
		}

		// We do a LateUpdate() to allow for any changes in the texture that may have happened in Update()
		void LateUpdate()
		{
			if (_setNativeSize)
			{
				SetNativeSize();
			}

			if (_lastTexture != mainTexture)
			{
				_lastTexture = mainTexture;
				SetVerticesDirty();
			}

			if (HasValidTexture())
			{
				if (mainTexture != null)
				{
					if (mainTexture.width != _lastWidth || mainTexture.height != _lastHeight)
					{
						_lastWidth = mainTexture.width;
						_lastHeight = mainTexture.height;
						SetVerticesDirty();
					}
				}
			}

			
			if (material != null && _mediaPlayer != null)
			{
				// Apply changes for alpha videos
				if (material.HasProperty(_propAlphaPack))
				{
					Helper.SetupAlphaPackedMaterial(material, _mediaPlayer.m_AlphaPacking);

					if (_flipY)
					{
						material.SetFloat(_propVertScale, -1f);
					}
					else
					{
						material.SetFloat(_propVertScale, 1f);
					}

#if UNITY_UGUI_NOSET_TEXELSIZE
					if (mainTexture != null)
					{
						material.SetVector(_propMainTextureTexelSize, new Vector4(1.0f / mainTexture.width, 1.0f / mainTexture.height, mainTexture.width, mainTexture.height));
					}
#endif
				}
			}


			SetMaterialDirty();
		}

		/// <summary>
		/// Texture to be used.
		/// </summary>
		public MediaPlayer CurrentMediaPlayer
		{
			get
			{
				return _mediaPlayer;
			}
			set
			{
				if (_mediaPlayer != value)
				{
					_mediaPlayer = value;
					//SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		/// <summary>
		/// UV rectangle used by the texture.
		/// </summary>
		public Rect uvRect
		{
			get
			{
				return m_UVRect;
			}
			set
			{
				if (m_UVRect == value)
				{
					return;
				}
				m_UVRect = value;
				SetVerticesDirty();
			}
		}

		/// <summary>
		/// Adjust the scale of the Graphic to make it pixel-perfect.
		/// </summary>

		[ContextMenu("Set Native Size")]
		public override void SetNativeSize()
		{
			Texture tex = mainTexture;
			if (tex != null)
			{
				int w = Mathf.RoundToInt(tex.width * uvRect.width);
				int h = Mathf.RoundToInt(tex.height * uvRect.height);

				if (_mediaPlayer != null)
				{
					if (_mediaPlayer.m_AlphaPacking == AlphaPacking.LeftRight)
					{
						w /= 2;
					}
					else if (_mediaPlayer.m_AlphaPacking == AlphaPacking.TopBottom)
					{
						h /= 2;
					}
				}

				rectTransform.anchorMax = rectTransform.anchorMin;
				rectTransform.sizeDelta = new Vector2(w, h);
			}
		}

		/// <summary>
		/// Update all renderer data.
		/// </summary>
// OnFillVBO deprecated by 5.2
// OnPopulateMesh(Mesh mesh) deprecated by 5.2 patch 1
#if UNITY_5 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2_0
/*		protected override void OnPopulateMesh(Mesh mesh)
		{			
			List<UIVertex> verts = new List<UIVertex>();
			_OnFillVBO( verts );

			var quad = new UIVertex[4];
			for (int i = 0; i < vbo.Count; i += 4)
			{
				vbo.CopyTo(i, quad, 0, 4);
				vh.AddUIVertexQuad(quad);
			}
			vh.FillMesh( toFill );
		}*/

#if !UNITY_5_2_1
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			
			List<UIVertex> aVerts = new List<UIVertex>();
			_OnFillVBO( aVerts );

			// TODO: cache these indices since they never change?
			List<int> aIndicies = new List<int>( new int[]{ 0, 1, 2, 2, 3, 0 } );
			vh.AddUIVertexStream( aVerts, aIndicies );
		}
#endif
		[System.Obsolete("This method is not called from Unity 5.2 and above")]
#endif
		protected override void OnFillVBO(List<UIVertex> vbo)
		{
			_OnFillVBO(vbo);
		}

		private void _OnFillVBO(List<UIVertex> vbo)
		{
			_flipY = false;
			if (HasValidTexture())
			{
				_flipY = _mediaPlayer.TextureProducer.RequiresVerticalFlip();
			}

			Vector4 v = GetDrawingDimensions(_keepAspectRatio);

			vbo.Clear();

			var vert = UIVertex.simpleVert;

			vert.position = new Vector2(v.x, v.y);
			vert.uv0 = new Vector2(m_UVRect.xMin, m_UVRect.yMin);
			if (_flipY)
			{
				vert.uv0 = new Vector2(m_UVRect.xMin, 1.0f - m_UVRect.yMin);
			}
			vert.color = color;
			vbo.Add(vert);

			vert.position = new Vector2(v.x, v.w);
			vert.uv0 = new Vector2(m_UVRect.xMin, m_UVRect.yMax);
			if (_flipY)
			{
				vert.uv0 = new Vector2(m_UVRect.xMin, 1.0f - m_UVRect.yMax);
			}
			vert.color = color;
			vbo.Add(vert);

			vert.position = new Vector2(v.z, v.w);
			vert.uv0 = new Vector2(m_UVRect.xMax, m_UVRect.yMax);
			if (_flipY)
			{
				vert.uv0 = new Vector2(m_UVRect.xMax, 1.0f - m_UVRect.yMax);
			}
			vert.color = color;
			vbo.Add(vert);

			vert.position = new Vector2(v.z, v.y);
			vert.uv0 = new Vector2(m_UVRect.xMax, m_UVRect.yMin);
			if (_flipY)
			{
				vert.uv0 = new Vector2(m_UVRect.xMax, 1.0f - m_UVRect.yMin);
			}
			vert.color = color;
			vbo.Add(vert);
		}

		//Added this method from Image.cs to do the keep aspect ratio
		private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
		{
			Vector4 returnSize = Vector4.zero;

			if (mainTexture != null)
			{
				var padding = Vector4.zero;
				var textureSize = new Vector2(mainTexture.width, mainTexture.height);

				if (_mediaPlayer != null)
				{
					if (_mediaPlayer.m_AlphaPacking == AlphaPacking.LeftRight)
					{
						textureSize.x /= 2f;
					}
					else if (_mediaPlayer.m_AlphaPacking == AlphaPacking.TopBottom)
					{
						textureSize.y /= 2f;
					}
				}
				
				Rect r = GetPixelAdjustedRect();
//				Debug.Log(string.Format("r:{2}, textureSize:{0}, padding:{1}", textureSize, padding, r));
				
				int spriteW = Mathf.RoundToInt( textureSize.x );
				int spriteH = Mathf.RoundToInt( textureSize.y );
				
				var size = new Vector4( padding.x / spriteW,
										padding.y / spriteH,
										(spriteW - padding.z) / spriteW,
										(spriteH - padding.w) / spriteH );

				if (shouldPreserveAspect && textureSize.sqrMagnitude > 0.0f)
				{
					var spriteRatio = textureSize.x / textureSize.y;
					var rectRatio = r.width / r.height;
					
					if (spriteRatio > rectRatio)
					{
						var oldHeight = r.height;
						r.height = r.width * (1.0f / spriteRatio);
						r.y += (oldHeight - r.height) * rectTransform.pivot.y;
					}
					else
					{
						var oldWidth = r.width;
						r.width = r.height * spriteRatio;
						r.x += (oldWidth - r.width) * rectTransform.pivot.x;
					}
				}
				
				returnSize = new Vector4( r.x + r.width * size.x,
										  r.y + r.height * size.y,
										  r.x + r.width * size.z,
										  r.y + r.height * size.w  );

			}

			return returnSize;
		}	
	}
}

#endif