using UnityEngine;
using System.Collections;

public class AutoTransparent : MonoBehaviour
{
	private Shader m_OldShader = null;
	private Color m_OldColor = Color.black;

	int frameCountToDestroy = 0;
	MeshRenderer mRenderer = null;

	public void BeTransparent()
	{
		if (mRenderer == null) {
			mRenderer = GetComponent<MeshRenderer>();
		}
		// reset the frame count to destroy;
		frameCountToDestroy = 0;
		if (m_OldShader == null)
		{
			// Save the current shader
			m_OldShader = mRenderer.material.shader;
			m_OldColor  = mRenderer.material.color;
			mRenderer.material.shader = Shader.Find("Standard");
			mRenderer.material.SetFloat("_Mode",3f);
			mRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			mRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			mRenderer.material.SetInt("_ZWrite", 0);
			mRenderer.material.DisableKeyword("_ALPHATEST_ON");
			mRenderer.material.DisableKeyword("_ALPHABLEND_ON");
			mRenderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			mRenderer.material.renderQueue = 3000;
		}
	}
	void Update()
	{
		frameCountToDestroy++;
		if (frameCountToDestroy > 5)
		{
			// Reset the shader
			mRenderer.material.shader = m_OldShader;
			mRenderer.material.color = m_OldColor;
			// And remove this script
			Destroy(this);
		}
	}


}