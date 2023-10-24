using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	// �V���O���g����
	public static SoundManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	// SE�i�[�p�ϐ�
	public AudioSource[] se;

	/// <summary>
	/// SE��炷(0:�Q�[���I�[�o�[ 1:�� 2:��e 3:�U�� 4:UI)
	/// </summary>
	/// <param name="x"></param>
	public void PlaySE(int x)
	{
		se[x].Stop();
		se[x].Play();
	}
}
