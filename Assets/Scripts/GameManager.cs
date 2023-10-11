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

	// スライダー作成
	[SerializeField]
	private Slider staminaSlider;

	// ダイアログUI変数
	public GameObject dialogBox;
	public Text dialogText;

	// 表示する文章
	private string[] dialogLines;

	// 現在の文章（今何行目か）
	private int currentLine;

	// 文字送り判定
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
	/// 関数の作成（UIを更新する）
	/// </summary>
	public void UpdateHealthUI()
	{
		hpSlider.maxValue = player.maxHealth;
		hpSlider.value = player.currentHealth;
	}

	/// <summary>
	/// 関数の作成（スタミナスライダー更新用）
	/// </summary>
	public void UpdateStaminaUI()
	{
		staminaSlider.maxValue = player.totalStamina;
		staminaSlider.value = player.currentStamina;
	}

	/// <summary>
	/// ダイアログを表示して表示する文章を設定
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

	// ダイアログの表示切替
}
