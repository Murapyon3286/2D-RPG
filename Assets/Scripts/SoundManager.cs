using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	// シングルトン化
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

	// SE格納用変数
	public AudioSource[] se;

	/// <summary>
	/// SEを鳴らす(0:ゲームオーバー 1:回復 2:被弾 3:攻撃 4:UI)
	/// </summary>
	/// <param name="x"></param>
	public void PlaySE(int x)
	{
		se[x].Stop();
		se[x].Play();
	}
}
