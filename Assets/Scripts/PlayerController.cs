using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField, Tooltip("移動スピード")]
	private int moveSpeed;

	[SerializeField]
	private Animator playerAnim;

	public Rigidbody2D rb;

	// 変数の作成（武器を振る方向）
	[SerializeField]
	private Animator weaponAnim;

	// 変数の作成（現在のHP,最大HP）
	[System.NonSerialized]
	public int currentHealth;

	public int maxHealth;

	void Start()
  {
    // HPの設定
		currentHealth = maxHealth;

		// GameManagerからUI更新関数を呼び出す
		GameManager.instance.UpdateHealthUI();
  }

  void Update()
  {
		// 剛体
		rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed; // velocityは速度を扱う。Vector2はxとyを扱う。
		if (rb.velocity != Vector2.zero) // 上下左右の速度がゼロではなくプレイヤーが動いているとき
		{
			if (Input.GetAxisRaw("Horizontal") != 0)
			{
				if (Input.GetAxisRaw("Horizontal") > 0)
				{
					playerAnim.SetFloat("X", 1f);
					playerAnim.SetFloat("Y", 0);

					// パラメータのX,Yの数値をキー入力に合わせて変更
					weaponAnim.SetFloat("X", 1f);
					weaponAnim.SetFloat("Y", 0);
				}
				else
				{
					playerAnim.SetFloat("X", -1f);
					playerAnim.SetFloat("Y", 0);

					// パラメータのX,Yの数値をキー入力に合わせて変更
					weaponAnim.SetFloat("X", -1f);
					weaponAnim.SetFloat("Y", 0);
				}
			}
			else if (Input.GetAxisRaw("Vertical") > 0)
			{
				playerAnim.SetFloat("X", 0);
				playerAnim.SetFloat("Y", 1f);

				// パラメータのX,Yの数値をキー入力に合わせて変更
				weaponAnim.SetFloat("X", 0);
				weaponAnim.SetFloat("Y", 1f);
			}
			else
			{
				playerAnim.SetFloat("X", 0);
				playerAnim.SetFloat("Y", -1f);

				// パラメータのX,Yの数値をキー入力に合わせて変更
				weaponAnim.SetFloat("X", 0);
				weaponAnim.SetFloat("Y", -1f);
			}
		}

		// 左クリックで攻撃
		if (Input.GetMouseButtonDown(0))
		{
			weaponAnim.SetTrigger("Attack");
		}
  }
}
