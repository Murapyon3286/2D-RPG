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

	// 吹き飛び判定
	private bool isKnockingBack;

	// 吹き飛ぶ方向
	private Vector2 knockDir;

	// 吹き飛び時間、吹き飛ぶ力
	[SerializeField]
	private float knockbackTime, knockbockForce;

	// タイマー（吹き飛び）
	private float knockbackCounter;

	// 無敵時間、タイマー（無敵時間）
	[SerializeField]
	private float invincibilityTime;
	private float invincibilityCounter;

	// スタミナ量、スタミナ回復速度
	public float totalStamina, recoverySpeed;

	// 現在のスタミナ
	[System.NonSerialized]
	public float currentStamina;

	// ダッシュの速度、長さ、スタミナ消費量
	[SerializeField]
	private float dashSpeed, dashLength, dashCost;

	// タイマー、移動時にかける用変数
	private float dashCounter, activeMoveSpeed;

	void Start()
  {
    // HPの設定
		currentHealth = maxHealth;

		// GameManagerからUI更新関数を呼び出す
		GameManager.instance.UpdateHealthUI();

		activeMoveSpeed = moveSpeed;

		currentStamina = totalStamina;
		GameManager.instance.UpdateHealthUI();
	}

  void Update()
  {
		// メニューを開いているときは動かないようにする
		if (GameManager.instance.statusPanel.activeInHierarchy)
		{
			return;
		}

		// 無敵時間の判定と無敵時のコード取得
		// 現在が無敵時間かどうか
		if (invincibilityCounter > 0)
		{
			// 現在の経過時間から1フレーム（update関数が実行された時間）を引く
			invincibilityCounter -= Time.deltaTime;
		}

		// 現在ノックバック中か？
		if (isKnockingBack)
		{
			// playerに行動をさせたくないため、時間を引く
			knockbackCounter -= Time.deltaTime;
			rb.velocity = knockDir * knockbockForce;

			// ノックバックの時間が終了したか？
			if (knockbackCounter <= 0)
			{
				isKnockingBack = false;
			}
			else
			{
				return;
			}
		}

		// 剛体
		rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * activeMoveSpeed; // velocityは速度を扱う。Vector2はxとyを扱う。
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

		// ダッシュの判定（dashCounterがどのくらいあるのか）
		if (dashCounter <= 0)
		{
			if (Input.GetKeyDown(KeyCode.Space) && currentStamina > dashCost)
			{
				activeMoveSpeed = dashSpeed;
				dashCounter = dashLength;
				currentStamina -= dashCost;
				GameManager.instance.UpdateStaminaUI();
			}
		}
		else
		{
			dashCounter -= Time.deltaTime;

			if (dashCounter <= 0)
			{
				activeMoveSpeed = moveSpeed;
			}
		}

		currentStamina = Mathf.Clamp(currentStamina + recoverySpeed * Time.deltaTime, 0, totalStamina);
		GameManager.instance.UpdateStaminaUI();
	}

	/// <summary>
	/// 吹き飛び関数
	/// </summary>
	/// <param name="position"></param>
	public void KnockBack(Vector3 position)
	{
		// 吹き飛ぶ時間を設定
		knockbackCounter = knockbackTime;
		isKnockingBack = true;

		knockDir = transform.position - position;
		knockDir.Normalize();
	}

	/// <summary>
	/// 攻撃関数
	/// </summary>
	/// <param name="damage"></param>
	public void DamagePlayer(int damage)
	{
		// 無敵時間ではない場合
		if (invincibilityCounter <= 0)
		{
			currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
			invincibilityCounter = invincibilityTime;
			SoundManager.instance.PlaySE(2);

			if (currentHealth == 0)
			{
				gameObject.SetActive(false);
				SoundManager.instance.PlaySE(0);
				GameManager.instance.Load();
			}
		}

		GameManager.instance.UpdateHealthUI();
	}

	/// <summary>
	/// アイテムとの衝突判定
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Portion" && maxHealth != currentHealth && collision.GetComponent<Items>().waitTime <= 0)
		{
			Items items = collision.GetComponent<Items>();
			SoundManager.instance.PlaySE(1);
			currentHealth = Mathf.Clamp(currentHealth + items.healthItemRecoveryValue, 0, maxHealth);
			GameManager.instance.UpdateHealthUI();
			Destroy(collision.gameObject);
		}
	}
}
