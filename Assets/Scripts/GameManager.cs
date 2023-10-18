using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ライブラリの追加
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
		// 文字送りの処理を実装
		if (dialogBox.activeInHierarchy)
		{
			if (Input.GetMouseButtonUp(1))
			{
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

	/// <summary>
	/// ダイアログの表示切替
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
}
