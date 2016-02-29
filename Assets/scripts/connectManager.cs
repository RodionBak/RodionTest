using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Net;
using UnityEngine.UI;
using System;

[RequireComponent( typeof( NetworkView ) )]


//Соединение
public class connectManager : MonoBehaviour {


	//панель для входа в мультиплеер
	public static string serverIp = "127.0.0.1";
	public static int serverPort = 4585;
	public static bool started = false;

	public static NetworkPlayer playerAdres;
	public static RPCMode mode = new RPCMode ();

	//есть подключенные игроки
	public static bool ConnectedPlayers = false;

	// Use this for initialization
	void Start () {
		//если режим игры - по умолчанию создать сервер
		if (MenuScript.watchMode == false) {
			createServer ();
		} else {//в режиме просмотра - подключаться к IP
			joinToServer();
		}
	}

	// Update is called once per frame
	void Update () {

	}




	//подключится к серверу
	public static void joinToServer()
	{
		Network.Connect (serverIp, serverPort);
	}


	//инициализирует сервер
	public static void createServer()
	{
		//Network.InitializeSecurity ();
		Network.InitializeServer(10, serverPort, false);//создать сервер
	}



	//при ошибке подключения к серверу
	void OnFailedToConnect(NetworkConnectionError error) 
	{
		if (GameObject.Find ("TextMessage") != null) {
			GameObject.Find ("TextMessage").GetComponent<Text> ().text = "Ошибка подключения";
			gameObject.GetComponent<GameLevel> ().gameOver ();
		}
	}




	//получение координат
	[RPC]
	private void OnGetCoords(int objId, Vector3 pos) {
	}





	//удачное соединение
	void OnConnectedToServer() 
	{
		if (GameObject.Find ("TextMessage") != null) {
			GameObject.Find ("TextMessage").GetComponent<Text> ().text = "Соединено. Режим просмотра";
		}
	}


	//плеер присоединился к игре
	void OnPlayerConnected( NetworkPlayer player ) {

		//лог
		if (GameObject.Find ("TextMessage") != null) {
			GameObject.Find ("TextMessage").GetComponent<Text> ().text = "Присоединен зритель.";
		}

		//можно синхронизировать
		ConnectedPlayers = true;
	}


	//сервер проиграл
	[RPC]
	private void OnGameOver() {
		Camera.main.GetComponent<GameLevel> ().gameOver ();
	}

	//команда о проигрыше
	public void myGameOver()
	{
		if (ConnectedPlayers == true) {
			RPCMode mode = new RPCMode ();

			//всем сообщить
			Camera.main.GetComponent<NetworkView> ().RPC ("OnGameOver", mode);
		}
	}

	//изменение счета на сервере
	[RPC]
	private void OnScoreChange(int score) {
		Camera.main.GetComponent<GameLevel> ().score = score;
		Camera.main.GetComponent<GameLevel> ().addScore (0);
	}

	//команда о изменении счета
	public void myScoreChange(int score)
	{
		if (ConnectedPlayers == true) {
		  RPCMode mode = new RPCMode ();

		  //всем сообщить
			Camera.main.GetComponent<NetworkView>().RPC("OnScoreChange", mode,score);
		}
	}








	//сервер создался
	void OnServerInitialized()
	{
		//лог
		if (GameObject.Find ("TextMessage") != null) {
			GameObject.Find ("TextMessage").GetComponent<Text> ().text = "";
		}

		//запустить игру
		gameObject.GetComponent<GameLevel>().startGame();
	}








}
