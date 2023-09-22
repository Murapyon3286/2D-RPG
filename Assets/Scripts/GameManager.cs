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

	// �֐��̍쐬�iUI���X�V����j
	public void UpdateHealthUI()
	{
		hpSlider.maxValue = player.maxHealth;
		hpSlider.value = player.currentHealth;
	}
}
