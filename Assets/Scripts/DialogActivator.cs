using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
	// 会話文章
	[SerializeField, Header("会話文章"), Multiline(3)]
	private string[] lines;

	// ダイアログの表示判定
	private bool canActivator;

	// セーブポイント判定
	[SerializeField]
	private bool savePoint;

  // Start is called before the first frame update
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
		if (Input.GetMouseButtonDown(1) && canActivator && !GameManager.instance.dialogBox.activeInHierarchy)
		{
			GameManager.instance.ShowDialog(lines);

			// savePoint次第でセーブを行う
			if (savePoint)
			{
				GameManager.instance.SaveStatus();
			}
		}
  }

	/// <summary>
	/// 衝突判定①
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			canActivator = true;
		}
	}

	/// <summary>
	/// 衝突判定②
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			canActivator = false;
			GameManager.instance.ShowdialogChange(canActivator);
		}
	}
}
