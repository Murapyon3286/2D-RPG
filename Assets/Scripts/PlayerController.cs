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

	void Start()
  {
    // HP�̐ݒ�
		currentHealth = maxHealth;

		// GameManager����UI�X�V�֐����Ăяo��
		GameManager.instance.UpdateHealthUI();
  }

  void Update()
  {
		// ����
		rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed; // velocity�͑��x�������BVector2��x��y�������B
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
  }
}