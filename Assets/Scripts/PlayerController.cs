using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// �����肽������

	// �ϐ��쐬�i�ړ��X�s�[�h�A�A�j���[�V�����A���́j
	[SerializeField, Tooltip("�ړ��X�s�[�h")]
	private int moveSpeed;

	[SerializeField]
	private Animator playerAnim;

	public Rigidbody2D rb;

	// �L�[���͂��󂯎��A�ړ�������R�[�h����
	// �����ƃA�j���[�V������A��
  // Start is called before the first frame update
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
		// ����
		rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;	// velocity�͑��x�������BVector2��x��y�������B
  }
}
