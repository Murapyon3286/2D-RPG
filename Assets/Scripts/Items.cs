using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
	// �A�C�e���񕜗�
	public int healthItemRecoveryValue;

	// �A�C�e����������܂ł̎���
	[SerializeField]
	private float lifeTime;

	// �擾�ł���悤�ɂȂ�܂ł̎���
	public float waitTime;

  void Start()
  {
		// ���Ԍo�߂ō폜
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
