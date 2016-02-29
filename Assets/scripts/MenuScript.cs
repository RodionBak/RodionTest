using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//скрипт для управления меню
public class MenuScript : MonoBehaviour {

	//панель ввода IP
	public GameObject ipPanel;

	//режим просмотра игры
	public static bool watchMode = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//старт игры
	public void startGame()
	{
		//по умолчаниюб выключить режим просмотра из главного меню
		if (SceneManager.GetActiveScene ().name == "Main") 
		{watchMode = false;}

		Application.LoadLevel("Game");
	}

	//завершить игру
	public void EndOfGame()
	{
		Application.Quit();
	}

	//войти в режим просмотра игры
	public void WatchGame()
	{
		if (ipPanel.activeSelf == false) {
			ipPanel.SetActive (true);
			GameObject.Find("WatchBtn").transform.GetChild(0).GetComponent<Text>().text = "OK";
		} else {
			watchMode = true;//включить режим просмотра
			Application.LoadLevel("Game");
		}
	}

}
