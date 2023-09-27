using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	// �ϐ��̐錾
	// ����
	private Rigidbody2D rb;

	// �A�j���[�V����
	private Animator enemyAnim;

	// �������x�A�҂����ԁA��������
	[SerializeField]
	private float moveSpeed, waitTime, walkTime;

	// �^�C�}�[
	private float waitCounter, moveCounter;

	// ��������
	private Vector2 moveDir;

	// �ړ��͈�
	[SerializeField]
	private BoxCollider2D area;

  void Start()
  {
    // �R���|�[�l���g�擾
		rb = GetComponent<Rigidbody2D>();
		enemyAnim = GetComponent<Animator>();

		// �^�C�}�[�̐ݒ�
		waitCounter = waitTime;
  }

  void Update()
  {
		// �^�C�}�[�����炵�Ă����A�ړ�������߂�R�[�h
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

		// �G�̈ړ��͈͂�ݒ�
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, area.bounds.min.x + 1, area.bounds.max.x - 1),
			Mathf.Clamp(transform.position.y, area.bounds.min.y + 1, area.bounds.max.y - 1), transform.position.z);
  }
}
