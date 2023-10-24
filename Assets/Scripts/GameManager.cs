using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ���C�u�����̒ǉ�
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Reporting;

public class GameManager : MonoBehaviour
{
	// static��
	public static GameManager instance;

	// �ϐ��̐錾�iUI,PlayerController�j
	[SerializeField]
	private Slider hpSlider;

	[SerializeField]
	private PlayerController player;

	// �X���C�_�[�쐬
	[SerializeField]
	private Slider staminaSlider;

	// �_�C�A���OUI�ϐ�
	public GameObject dialogBox;
	public Text dialogText;

	// �\�����镶��
	private string[] dialogLines;

	// ���݂̕��́i�����s�ڂ��j
	private int currentLine;

	// �������蔻��
	private bool justStarted;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	// UI�i�[�p
	public GameObject statusPanel;

	[SerializeField]
	private Text hpText, stText, atText;

	// weapon�X�N���v�g�i�[
	[SerializeField]
	private Weapon weapon;

	// �o���l�A���x��
	private int totalExp, currentLV;

	// ���x���A�b�v�ɕK�v�Ȍo���l��
	[SerializeField, Tooltip("���x���A�b�v�ɕK�v�Ȍo���l")]
	private int[] requiredExp;

	// LevelUp���ɌĂ�UI�i�[�p
	[SerializeField]
	private GameObject levelUpText;

	// levelUpText��z�u���邽�߂�canvas
	[SerializeField]
	private Canvas canvas;



	void Start()
  {
    
  }

  void Update()
  {
		// ��������̏���������
		if (dialogBox.activeInHierarchy)
		{
			if (Input.GetMouseButtonUp(1))
			{
				SoundManager.instance.PlaySE(4);
				if (!justStarted)
				{
					currentLine++;

					if (currentLine >= dialogLines.Length)
					{
						dialogBox.SetActive(false);
					}
					else
					{
						dialogText.text = dialogLines[currentLine];
					}
				}
				else
				{
					justStarted = false;
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			if (statusPanel.activeInHierarchy)
			{
				CloseStatusPanel();
			}
			else
			{
				ShowStatusPanel();
			}
		}
  }

	/// <summary>
	/// �֐��̍쐬�iUI���X�V����j
	/// </summary>
	public void UpdateHealthUI()
	{
		hpSlider.maxValue = player.maxHealth;
		hpSlider.value = player.currentHealth;
	}

	/// <summary>
	/// �֐��̍쐬�i�X�^�~�i�X���C�_�[�X�V�p�j
	/// </summary>
	public void UpdateStaminaUI()
	{
		staminaSlider.maxValue = player.totalStamina;
		staminaSlider.value = player.currentStamina;
	}

	/// <summary>
	/// �_�C�A���O��\�����ĕ\�����镶�͂�ݒ�
	/// </summary>
	/// <param name="lines"></param>
	public void ShowDialog(string[] lines)
	{
		dialogLines = lines;
		currentLine = 0;
		dialogText.text = dialogLines[currentLine];
		dialogBox.SetActive(true);
		justStarted = true;
	}

	/// <summary>
	/// �_�C�A���O�̕\���ؑ�
	/// </summary>
	/// <param name="x"></param>
	public void ShowdialogChange(bool x)
	{
		dialogBox.SetActive(x);
	}

	public void Load()
	{
		SceneManager.LoadScene("Main");
	}

	/// <summary>
	/// �X�e�[�^�XUI�\���֐�
	/// </summary>
	public void ShowStatusPanel()
	{
		statusPanel.SetActive(true);
		Time.timeScale = 0f;
		// UI�X�V�p�֐��̌Ăяo��
		StatusUpdate();
	}

	/// <summary>
	/// �X�e�[�^�XUI��\���֐�
	/// </summary>
	public void CloseStatusPanel()
	{
		statusPanel.SetActive(false);
		Time.timeScale = 1f;
	}

	/// <summary>
	/// UI�X�V�p�֐�
	/// </summary>
	public void StatusUpdate()
	{
		hpText.text = "HP : " + player.maxHealth;
		stText.text = "�X�^�~�i : " + player.totalStamina;
		atText.text = "�U���� : " + weapon.attackDamage;
	}

	/// <summary>
	/// �o���l�A�b�v�֐�
	/// </summary>
	public void AddExp(int exp)
	{
		if (requiredExp.Length <= currentLV)
		{
			return;
		}
		totalExp += exp;

		if (totalExp >= requiredExp[currentLV])
		{
			currentLV++;
			player.maxHealth += 5;
			player.totalStamina += 5;
			weapon.attackDamage += 5;
			// �o���l�֐��쐬
			GameObject levelUp = Instantiate(levelUpText);
			levelUp.transform.SetParent(canvas.transform);
			levelUp.transform.localPosition = player.transform.position + new Vector3(0, 100, 0);
		}
	}
}
