using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	// 変数の宣言
	// 剛体
	private Rigidbody2D rb;

	// アニメーション
	private Animator enemyAnim;

	// 動く速度、待ち時間、動く時間
	[SerializeField]
	private float moveSpeed, waitTime, walkTime;

	// タイマー
	private float waitCounter, moveCounter;

	// 動く方向
	private Vector2 moveDir;

	// 移動範囲
	[SerializeField]
	private BoxCollider2D area;

	// プレイヤーを追いかける判定
	[SerializeField, Tooltip("プレイヤーを追いかける？")]
	private bool chase;

	// プレイヤーを追いかけている判定
	private bool isChasing;

	// 追いかける速度、 気づく範囲
	[SerializeField]
	private float chaseSpeed, rangeToChase;

	// プレイヤーの位置
	private Transform target;

	// 攻撃間隔
	[SerializeField]
	private float waitAfterHitting;

	// 攻撃力
	[SerializeField]
	private int attackDamage;

	// 現在のHP
	[SerializeField]
	private int currentHealth;

	// ノックバック中判定
	private bool isKnockingBack;

	// 吹き飛び時間、吹き飛ぶ力
	[SerializeField]
	private float knockBackTime, knockBackForce;

	// タイマー
	private float knockBackCounter;

	// 吹き飛ぶ方向
	private Vector2 knockDir;

	// ドロップアイテム
	[SerializeField]
	private GameObject portion;

	// ドロップ率
	[SerializeField]
	private float healthDropChance;

	// エフェクト格納用
	[SerializeField]
	private GameObject blood;

	// 経験値
	[SerializeField]
	private int exp;


	

  void Start()
  {
    // コンポーネント取得
		rb = GetComponent<Rigidbody2D>();
		enemyAnim = GetComponent<Animator>();

		// タイマーの設定
		waitCounter = waitTime;

		// プレイヤーの位置を取得
		target = GameObject.FindGameObjectWithTag("Player").transform;	// すべてのゲームオブジェクトから探し出すので処理が重い
  }

  void Update()
  {
		if (isKnockingBack)
		{
			if (knockBackCounter > 0)
			{
				knockBackCounter -= Time.deltaTime;
				rb.velocity = knockDir * knockBackForce;
			}
			else
			{
				rb.velocity = Vector2.zero;
				isKnockingBack = false;
			}
			return;
		}

		// 追いかける判定次第ではプレイヤーを追いかける
		if (!isChasing)
		{
			// タイマーを減らしていき、移動先を決めるコード
			if (waitCounter > 0)
			{
				waitCounter -= Time.deltaTime;
				rb.velocity = Vector2.zero;

				if (waitCounter <= 0)
				{
					moveCounter = waitTime;
					enemyAnim.SetBool("moving", true);
					moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
					moveDir.Normalize();
				}
			}
			else
			{
				moveCounter -= Time.deltaTime;
				rb.velocity = moveDir * moveSpeed;

				if (moveCounter <= 0)
				{
					enemyAnim.SetBool("moving", false);
					waitCounter = waitTime;
				}
			}

			// Enemyがプレイヤーを追いかける判定に設定されているかどうか
			if (chase)
			{
				// 気づく範囲より距離が近かったらプレイヤーを追いかける
				if (Vector3.Distance(transform.position, target.transform.position) < rangeToChase)
				{
					isChasing = true;
				}
			}
		}
		else
		{
			if (waitCounter > 0)
			{
				waitCounter -= Time.deltaTime;
				rb.velocity = Vector2.zero;

				if (waitCounter <= 0)
				{
					enemyAnim.SetBool("moving", true);
				}
			}
			else
			{
				moveDir = target.transform.position - transform.position;
				moveDir.Normalize();
				rb.velocity = moveDir * chaseSpeed;
			}

			// EnemyがPlayerを追いかけるのをあきらめる
			if (Vector3.Distance(transform.position, target.transform.position) > rangeToChase)
			{
				isChasing = false;
				waitCounter = waitTime;
				enemyAnim.SetBool("moving", false);
			}
		}

		// 敵の移動範囲を設定
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, area.bounds.min.x + 1, area.bounds.max.x - 1),
			Mathf.Clamp(transform.position.y, area.bounds.min.y + 1, area.bounds.max.y - 1), transform.position.z);
  }

	/// <summary>
	/// 衝突判定
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter2D(Collision2D collision)	// 引数collisionはぶつかった相手の情報
	{
		if (collision.gameObject.tag == "Player")
		{
			if (isChasing)
			{
				PlayerController player = collision.gameObject.GetComponent<PlayerController>();
				player.KnockBack(transform.position);
				player.DamagePlayer(attackDamage);
				waitCounter = waitAfterHitting;
				enemyAnim.SetBool("moving", false);
			}
		}
	}

	/// <summary>
	/// ノックバック関数
	/// </summary>
	/// <param name="position"></param>
	public void KnockBack(Vector3 position)
	{
		isKnockingBack = true;
		knockBackCounter = knockBackTime;
		knockDir = transform.position - position;
		knockDir.Normalize();
		enemyAnim.SetBool("moving", false);
	}

	/// <summary>
	/// ダメージ関数
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="position"></param>
	public void TakeDamage(int damage, Vector3 position)
	{
		currentHealth -= damage;

		if (currentHealth <= 0)
		{
			Instantiate(blood, transform.position, transform.rotation);
			// 経験値加算関数
			GameManager.instance.AddExp(exp);
			// ドロップ判定を追加
			if (Random.Range(0, 100) < healthDropChance && portion != null)
			{
				Instantiate(portion, transform.position, transform.rotation);
			}

			Destroy(gameObject);
		}

		KnockBack(position);
	}
}
