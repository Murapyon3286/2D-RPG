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

  void Start()
  {
    // コンポーネント取得
		rb = GetComponent<Rigidbody2D>();
		enemyAnim = GetComponent<Animator>();

		// タイマーの設定
		waitCounter = waitTime;
  }

  void Update()
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

		// 敵の移動範囲を設定
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, area.bounds.min.x + 1, area.bounds.max.x - 1),
			Mathf.Clamp(transform.position.y, area.bounds.min.y + 1, area.bounds.max.y - 1), transform.position.z);
  }
}
