using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// 今回やりたいこと

	// 変数作成（移動スピード、アニメーション、剛体）
	[SerializeField, Tooltip("移動スピード")]
	private int moveSpeed;

	[SerializeField]
	private Animator playerAnim;

	public Rigidbody2D rb;

	// キー入力を受け取り、移動させるコード入力
	// 方向とアニメーションを連動
  // Start is called before the first frame update
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
		// 剛体
		rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;	// velocityは速度を扱う。Vector2はxとyを扱う。
  }
}
