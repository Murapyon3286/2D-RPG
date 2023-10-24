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

	// �v���C���[��ǂ������锻��
	[SerializeField, Tooltip("�v���C���[��ǂ�������H")]
	private bool chase;

	// �v���C���[��ǂ������Ă��锻��
	private bool isChasing;

	// �ǂ������鑬�x�A �C�Â��͈�
	[SerializeField]
	private float chaseSpeed, rangeToChase;

	// �v���C���[�̈ʒu
	private Transform target;

	// �U���Ԋu
	[SerializeField]
	private float waitAfterHitting;

	// �U����
	[SerializeField]
	private int attackDamage;

	// ���݂�HP
	[SerializeField]
	private int currentHealth;

	// �m�b�N�o�b�N������
	private bool isKnockingBack;

	// ������ю��ԁA������ԗ�
	[SerializeField]
	private float knockBackTime, knockBackForce;

	// �^�C�}�[
	private float knockBackCounter;

	// ������ԕ���
	private Vector2 knockDir;

	// �h���b�v�A�C�e��
	[SerializeField]
	private GameObject portion;

	// �h���b�v��
	[SerializeField]
	private float healthDropChance;

	// �G�t�F�N�g�i�[�p
	[SerializeField]
	private GameObject blood;

	// �o���l
	[SerializeField]
	private int exp;


	

  void Start()
  {
    // �R���|�[�l���g�擾
		rb = GetComponent<Rigidbody2D>();
		enemyAnim = GetComponent<Animator>();

		// �^�C�}�[�̐ݒ�
		waitCounter = waitTime;

		// �v���C���[�̈ʒu���擾
		target = GameObject.FindGameObjectWithTag("Player").transform;	// ���ׂẴQ�[���I�u�W�F�N�g����T���o���̂ŏ������d��
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

		// �ǂ������锻�莟��ł̓v���C���[��ǂ�������
		if (!isChasing)
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

			// Enemy���v���C���[��ǂ������锻��ɐݒ肳��Ă��邩�ǂ���
			if (chase)
			{
				// �C�Â��͈͂�苗�����߂�������v���C���[��ǂ�������
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

			// Enemy��Player��ǂ�������̂�������߂�
			if (Vector3.Distance(transform.position, target.transform.position) > rangeToChase)
			{
				isChasing = false;
				waitCounter = waitTime;
				enemyAnim.SetBool("moving", false);
			}
		}

		// �G�̈ړ��͈͂�ݒ�
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, area.bounds.min.x + 1, area.bounds.max.x - 1),
			Mathf.Clamp(transform.position.y, area.bounds.min.y + 1, area.bounds.max.y - 1), transform.position.z);
  }

	/// <summary>
	/// �Փ˔���
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter2D(Collision2D collision)	// ����collision�͂Ԃ���������̏��
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
	/// �m�b�N�o�b�N�֐�
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
	/// �_���[�W�֐�
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="position"></param>
	public void TakeDamage(int damage, Vector3 position)
	{
		currentHealth -= damage;

		if (currentHealth <= 0)
		{
			Instantiate(blood, transform.position, transform.rotation);
			// �o���l���Z�֐�
			GameManager.instance.AddExp(exp);
			// �h���b�v�����ǉ�
			if (Random.Range(0, 100) < healthDropChance && portion != null)
			{
				Instantiate(portion, transform.position, transform.rotation);
			}

			Destroy(gameObject);
		}

		KnockBack(position);
	}
}
