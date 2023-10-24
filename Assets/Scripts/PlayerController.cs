using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField, Tooltip("�ړ��X�s�[�h")]
	private int moveSpeed;

	[SerializeField]
	private Animator playerAnim;

	public Rigidbody2D rb;

	// �ϐ��̍쐬�i�����U������j
	[SerializeField]
	private Animator weaponAnim;

	// �ϐ��̍쐬�i���݂�HP,�ő�HP�j
	[System.NonSerialized]
	public int currentHealth;

	public int maxHealth;

	// ������є���
	private bool isKnockingBack;

	// ������ԕ���
	private Vector2 knockDir;

	// ������ю��ԁA������ԗ�
	[SerializeField]
	private float knockbackTime, knockbockForce;

	// �^�C�}�[�i������сj
	private float knockbackCounter;

	// ���G���ԁA�^�C�}�[�i���G���ԁj
	[SerializeField]
	private float invincibilityTime;
	private float invincibilityCounter;

	// �X�^�~�i�ʁA�X�^�~�i�񕜑��x
	public float totalStamina, recoverySpeed;

	// ���݂̃X�^�~�i
	[System.NonSerialized]
	public float currentStamina;

	// �_�b�V���̑��x�A�����A�X�^�~�i�����
	[SerializeField]
	private float dashSpeed, dashLength, dashCost;

	// �^�C�}�[�A�ړ����ɂ�����p�ϐ�
	private float dashCounter, activeMoveSpeed;

	void Start()
  {
    // HP�̐ݒ�
		currentHealth = maxHealth;

		// GameManager����UI�X�V�֐����Ăяo��
		GameManager.instance.UpdateHealthUI();

		activeMoveSpeed = moveSpeed;

		currentStamina = totalStamina;
		GameManager.instance.UpdateHealthUI();
	}

  void Update()
  {
		// ���j���[���J���Ă���Ƃ��͓����Ȃ��悤�ɂ���
		if (GameManager.instance.statusPanel.activeInHierarchy)
		{
			return;
		}

		// ���G���Ԃ̔���Ɩ��G���̃R�[�h�擾
		// ���݂����G���Ԃ��ǂ���
		if (invincibilityCounter > 0)
		{
			// ���݂̌o�ߎ��Ԃ���1�t���[���iupdate�֐������s���ꂽ���ԁj������
			invincibilityCounter -= Time.deltaTime;
		}

		// ���݃m�b�N�o�b�N�����H
		if (isKnockingBack)
		{
			// player�ɍs�������������Ȃ����߁A���Ԃ�����
			knockbackCounter -= Time.deltaTime;
			rb.velocity = knockDir * knockbockForce;

			// �m�b�N�o�b�N�̎��Ԃ��I���������H
			if (knockbackCounter <= 0)
			{
				isKnockingBack = false;
			}
			else
			{
				return;
			}
		}

		// ����
		rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * activeMoveSpeed; // velocity�͑��x�������BVector2��x��y�������B
		if (rb.velocity != Vector2.zero) // �㉺���E�̑��x���[���ł͂Ȃ��v���C���[�������Ă���Ƃ�
		{
			if (Input.GetAxisRaw("Horizontal") != 0)
			{
				if (Input.GetAxisRaw("Horizontal") > 0)
				{
					playerAnim.SetFloat("X", 1f);
					playerAnim.SetFloat("Y", 0);

					// �p�����[�^��X,Y�̐��l���L�[���͂ɍ��킹�ĕύX
					weaponAnim.SetFloat("X", 1f);
					weaponAnim.SetFloat("Y", 0);
				}
				else
				{
					playerAnim.SetFloat("X", -1f);
					playerAnim.SetFloat("Y", 0);

					// �p�����[�^��X,Y�̐��l���L�[���͂ɍ��킹�ĕύX
					weaponAnim.SetFloat("X", -1f);
					weaponAnim.SetFloat("Y", 0);
				}
			}
			else if (Input.GetAxisRaw("Vertical") > 0)
			{
				playerAnim.SetFloat("X", 0);
				playerAnim.SetFloat("Y", 1f);

				// �p�����[�^��X,Y�̐��l���L�[���͂ɍ��킹�ĕύX
				weaponAnim.SetFloat("X", 0);
				weaponAnim.SetFloat("Y", 1f);
			}
			else
			{
				playerAnim.SetFloat("X", 0);
				playerAnim.SetFloat("Y", -1f);

				// �p�����[�^��X,Y�̐��l���L�[���͂ɍ��킹�ĕύX
				weaponAnim.SetFloat("X", 0);
				weaponAnim.SetFloat("Y", -1f);
			}
		}

		// ���N���b�N�ōU��
		if (Input.GetMouseButtonDown(0))
		{
			weaponAnim.SetTrigger("Attack");
		}

		// �_�b�V���̔���idashCounter���ǂ̂��炢����̂��j
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
	/// ������ъ֐�
	/// </summary>
	/// <param name="position"></param>
	public void KnockBack(Vector3 position)
	{
		// ������Ԏ��Ԃ�ݒ�
		knockbackCounter = knockbackTime;
		isKnockingBack = true;

		knockDir = transform.position - position;
		knockDir.Normalize();
	}

	/// <summary>
	/// �U���֐�
	/// </summary>
	/// <param name="damage"></param>
	public void DamagePlayer(int damage)
	{
		// ���G���Ԃł͂Ȃ��ꍇ
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
	/// �A�C�e���Ƃ̏Փ˔���
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
