using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLevel : MonoBehaviour {
	
	//шарик
	public GameObject sphere;
	public int sphereCount = 10;//общее кол-во шариков

	//доска
	public GameObject panel;
	GameObject playerPanel;

	//скорость шариков
	public int speedLevel = 1;
	public float playerSpeed = 14f;

	//очки игрока
	public int score = 0;
	public GameObject displayScore;

	//окно проигрыша
	public GameObject endPanel;

	//время игры
	float gameTime;

	//центр игры
	public static Vector3 gCenter;


	// Use this for initialization
	void Start () {
		//центр игры
		gCenter = Camera.main.transform.position + new Vector3 (0f, 0f, 10f);
		gameObject.transform.rotation.SetLookRotation (gCenter);

		//отображение очков
		if (displayScore == null) {
			displayScore = GameObject.Find ("ScoreShadow");
		}

		//остановить время
		Time.timeScale = 0f;
	}





	//начать игру
	public void startGame()
	{
		//разместить доску
		playerPanel = Network.Instantiate (panel,
			gCenter + new Vector3 (0f, -4f, 0f), new Quaternion (0f, 0f, 0f, 0f), 0) as GameObject;

		//создать шарики
		if (sphere == null) //если не задан префаб добавить примитив
		{sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);}

		for (int i = 0; i < sphereCount; i++) {			
			GameObject sphereClone = Network.Instantiate (sphere,
				new Vector3(0f, gCenter.y - 42f + (i * 5f),0f),
				new Quaternion(0f,0f,0f,0f),0) as GameObject;
		}

		//запустить время
		Time.timeScale = 1f;

		//начать отсчет времени
		StartCoroutine (gameTimer());
	}






	
	// Update is called once per frame
	void Update () {
         //управление доской
		if ((Input.anyKey == true) && (MenuScript.watchMode == false))
		{
			//смещать горизонтальным джойстиком
			playerPanel.transform.position += new Vector3 (Input.GetAxis ("Horizontal") * Time.deltaTime * playerSpeed, 0f, 0f);

			//рамки передвижения
			if (Mathf.Abs (playerPanel.transform.position.x - gameObject.transform.position.x) > 5f) {
				panel.transform.position = new Vector3 (gameObject.transform.position.x + (5f * Mathf.Sign (playerPanel.transform.position.x - gameObject.transform.position.x)),
					                                    playerPanel.transform.position.y, 
				                                    	playerPanel.transform.position.z);
			}
		}
	}



	//отсчет времени
	IEnumerator gameTimer()
	{
		while (true) {
			yield return null;


			//отсчет
			gameTime += Time.deltaTime;

			//30 сек - ускорить шарики
			if (gameTime > 30f) {
				speedLevel += 1;
				gameTime = 0f;
			}
		}
	}



	//добавить очки
	public void addScore(int addCount)
	{
		//добавить
		score += addCount;

		//обновление по сети
		gameObject.GetComponent<connectManager>().myScoreChange(score);

		//обновить на дисплее
		displayScore.GetComponent<Text>().text = "Очки: " + score.ToString();
		displayScore.transform.GetChild (0).GetComponent<Text> ().text = displayScore.GetComponent<Text> ().text;
	}



	//игра закончена
	public void gameOver()
	{
		//сообщить по сети
		gameObject.GetComponent<connectManager> ().myGameOver ();

		//если создана панель проиграша - показать
		if (endPanel != null) {
			endPanel.SetActive (true);
			Time.timeScale = 0f;
		} else {
			Application.Quit();//если нет - выйти
		}
	}



}
