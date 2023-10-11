using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ���C�u�����̒ǉ�
using UnityEngine.UI;

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

	void Start()
  {
    
  }

  void Update()
  {
    
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

	// �_�C�A���O�̕\���ؑ�
}
