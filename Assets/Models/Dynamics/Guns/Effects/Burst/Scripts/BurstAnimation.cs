using UnityEngine;

public class BurstAnimation : MonoBehaviour {
	public Texture[] Textures;
	public float Time = 0.5f;

	private float _frameTime;
	private float _timer;
	private MeshRenderer _meshRenderer;
	private Light _flashLight;
	private int _currentTextureIndex;

	void Start()
	{
		_meshRenderer = GetComponent<MeshRenderer> ();
        _flashLight = GetComponent<Light> ();
		_frameTime = Time / Textures.Length;
	}

	public void Activate()
	{
		_timer = 0;
		_currentTextureIndex = 0;
		_meshRenderer.enabled = true;
        _flashLight.enabled = true;
		_meshRenderer.material.mainTexture = Textures[_currentTextureIndex];
	}
	
	void Update () 
	{
		_timer += UnityEngine.Time.deltaTime;
		if (_timer > _frameTime) 
		{
			_timer = 0;
			_meshRenderer.material.mainTexture = Textures[_currentTextureIndex];
			if(_currentTextureIndex >= Textures.Length-1)
			{
				_currentTextureIndex = 0;
				_meshRenderer.enabled = false;
                _flashLight.enabled = false;
			}
			else
			{
				_currentTextureIndex++;
			}
		}
	}
}
