using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
	// ��b����
	[SerializeField, Header("��b����"), Multiline(3)]
	private string[] lines;

	// �_�C�A���O�̕\������
	private bool canActivator;

	// �Z�[�u�|�C���g����
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

			// savePoint����ŃZ�[�u���s��
			if (savePoint)
			{
				GameManager.instance.SaveStatus();
			}
		}
  }

	/// <summary>
	/// �Փ˔���@
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
	/// �Փ˔���A
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
