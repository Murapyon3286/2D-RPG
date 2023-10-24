using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	// UŒ‚—Í
	[SerializeField]
	public int attackDamage;

  void Start()
  {
    
  }

  void Update()
  {
    
  }

	/// <summary>
	/// Õ“Ë”»’è
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			// UŒ‚—p‚ÌŠÖ”‚ğŒÄ‚Ño‚·
			collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, transform.position);
			SoundManager.instance.PlaySE(3);
		}
	}
}
