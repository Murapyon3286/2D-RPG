using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
	/// <summary>
	/// �J�ڗp�֐�
	/// </summary>
  public void GameStart()
	{
		SceneManager.LoadScene("Main");
	}
}
