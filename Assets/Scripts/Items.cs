using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
	// アイテム回復量
	public int healthItemRecoveryValue;

	// アイテムが消えるまでの時間
	[SerializeField]
	private float lifeTime;

	// 取得できるようになるまでの時間
	public float waitTime;

  void Start()
  {
		// 時間経過で削除
		Destroy(gameObject, lifeTime);
  }

  void Update()
  {
		if (waitTime > 0)
		{
			waitTime -= Time.deltaTime;
		}
  }
}
