using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	// �U����
	[SerializeField]
	public int attackDamage;

  void Start()
  {
    
  }

  void Update()
  {
    
  }

	/// <summary>
	/// �Փ˔���
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			// �U���p�̊֐����Ăяo��
			collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, transform.position);
			SoundManager.instance.PlaySE(3);
		}
	}
}
