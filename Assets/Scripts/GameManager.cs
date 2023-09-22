using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ライブラリの追加
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	// static化
	public static GameManager instance;

	// 変数の宣言（UI,PlayerController）
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

	// 関数の作成（UIを更新する）
	public void UpdateHealthUI()
	{
		hpSlider.maxValue = player.maxHealth;
		hpSlider.value = player.currentHealth;
	}
}
