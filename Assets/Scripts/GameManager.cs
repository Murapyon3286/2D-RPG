using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ライブラリの追加
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Reporting;

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

	// UI格納用
	public GameObject statusPanel;

	[SerializeField]
	private Text hpText, stText, atText;

	// weaponスクリプト格納
	[SerializeField]
	private Weapon weapon;

	// 経験値、レベル
	private int totalExp, currentLV;

	// レベルアップに必要な経験値量
	[SerializeField, Tooltip("レベルアップに必要な経験値")]
	private int[] requiredExp;

	// LevelUp時に呼ぶUI格納用
	[SerializeField]
	private GameObject levelUpText;

	// levelUpTextを配置するためのcanvas
	[SerializeField]
	private Canvas canvas;



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

	/// <summary>
	/// ステータスUI表示関数
	/// </summary>
	public void ShowStatusPanel()
	{
		statusPanel.SetActive(true);
		Time.timeScale = 0f;
		// UI更新用関数の呼び出し
		StatusUpdate();
	}

	/// <summary>
	/// ステータスUI非表示関数
	/// </summary>
	public void CloseStatusPanel()
	{
		statusPanel.SetActive(false);
		Time.timeScale = 1f;
	}

	/// <summary>
	/// UI更新用関数
	/// </summary>
	public void StatusUpdate()
	{
		hpText.text = "HP : " + player.maxHealth;
		stText.text = "スタミナ : " + player.totalStamina;
		atText.text = "攻撃力 : " + weapon.attackDamage;
	}

	/// <summary>
	/// 経験値アップ関数
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
			// 経験値関数作成
			GameObject levelUp = Instantiate(levelUpText);
			levelUp.transform.SetParent(canvas.transform);
			levelUp.transform.localPosition = player.transform.position + new Vector3(0, 100, 0);
		}
	}
}
