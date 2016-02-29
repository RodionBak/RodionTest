using UnityEngine;
using System.Collections;

[RequireComponent( typeof( NetworkView ) )]

//для трансляции позиции объекта по сети
public class SynchObject : MonoBehaviour {


	//Lerp - сглаживание позиции
	Vector3 oldPos = Vector3.zero;
	Vector3 newPos = Vector3.zero;
	float offsetTime = 0f;
	bool isPosSinch = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//объект синхронизирован - можно обновлять позицию
		//if (isPosSinch == true)
		//{
		//	offsetTime += Time.deltaTime * 9f;
		//	gameObject.transform.position = Vector3.Lerp(oldPos,newPos,offsetTime);
		//}

		//если есть подключенный игрок - синхронизировать позицию объекта
		if (connectManager.ConnectedPlayers == true) {
			translationPos ();
		}
	}




	//синхронизация координат
	[RPC]
	public void OnSynchPos(byte[] message)
	{
		//расчитать X
		float Xcoord = (((message[0] * 255) + message[1]) - 30000) / 100f;

		//расчитать Y
		float Ycoord = (((message[2] * 255) + message[3]) - 30000) / 100f;

		//расчитать Z
		float Zcoord = (((message[4] * 255) + message[5]) - 30000) / 100f;

		Debug.Log ("get position");

	    gameObject.transform.position = new Vector3 (Xcoord, Ycoord, Zcoord);

		//сообщить координаты объекту
		//сглаживание
		//oldPos = gameObject.transform.position;
		//newPos = new Vector3(Xcoord,Ycoord,Zcoord);
		//offsetTime = 0f;
		//isPosSinch = true;//можно синхронизировать
	}


	//транслировать свою позицию
	public void translationPos()
	{
		//сообщение
		byte[] message = new byte[6];

		//2)исхождные координаты
		//целое значение
		int Xcoord = 30000 + Mathf.RoundToInt (gameObject.transform.position.x / 0.01f);
		int Ycoord = 30000 + Mathf.RoundToInt (gameObject.transform.position.y / 0.01f);
		int Zcoord = 30000 + Mathf.RoundToInt (gameObject.transform.position.z / 0.01f);

		//расчитать X
		message [0] = (byte)(Xcoord / 255f);
		message [1] = (byte)(Xcoord % 255);

		//расчитать Y
		message [2] = (byte)(Ycoord / 255f);
		message [3] = (byte)(Ycoord % 255);

		//расчитать Z
		message [4] = (byte)(Zcoord / 255f);
		message [5] = (byte)(Zcoord % 255);

		//Debug.Log ("Translate position");

		//отослать
		RPCMode mode = new RPCMode ();
		gameObject.GetComponent<NetworkView>().RPC("OnSynchPos", mode, message);
	}


}
