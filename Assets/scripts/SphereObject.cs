using UnityEngine;
using System.Collections;

//поведение шарика
public class SphereObject : MonoBehaviour {



	//уровень скорости
	int speedLvl = 0;

	// Use this for initialization
	void Start () {
	   //сделать физическим
		gameObject.AddComponent<Rigidbody>();
		gameObject.GetComponent<Rigidbody> ().useGravity = false;

		//коллайдер
		if (gameObject.GetComponent<SphereCollider> () == null)//если нет - добавить
		{gameObject.AddComponent<SphereCollider> ();}
		gameObject.GetComponent<SphereCollider> ().isTrigger = true;


		//запустить
		restartSphere();

	}


	//ускорить шарик
	void setSpeed(int speedLevel)
	{
		speedLvl = speedLevel;
		gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, -(speedLevel * 2f), 0f);
		Debug.Log ("accelerate me");
	}


	// Update is called once per frame
	void Update () {
	   //если игрок пропустил шарик
		if (gameObject.transform.position.y < (GameLevel.gCenter.y - 5f)) {
			//игра закончена
			Camera.main.GetComponent<GameLevel>().gameOver();
			restartSphere ();
		}

		if (speedLvl != Camera.main.GetComponent<GameLevel> ().speedLevel) {
			setSpeed (Camera.main.GetComponent<GameLevel> ().speedLevel);
		}

		//смещение 
		gameObject.transform.position += new Vector3(Mathf.Cos(gameObject.transform.position.y) * 0.02f,0f,0f);
	}



	//запустить шарик заново
	public void restartSphere()
	{
		//переместить в случайную позицию
		gameObject.transform.position = new Vector3 (Random.Range (GameLevel.gCenter.x - 5f, GameLevel.gCenter.x + 5f),
			                                         gameObject.transform.position.y + 50f,
			                                         GameLevel.gCenter.z);
		//задать ускорение
		speedLvl = Camera.main.GetComponent<GameLevel>().speedLevel;
		setSpeed (speedLvl);
	}



	//столкновение с доской
	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.tag == "Player") {
			//добавить очков
			Camera.main.GetComponent<GameLevel>().addScore(1);
			//перезапустить шарик
			restartSphere();
			Debug.Log ("coll");
		}
	}
}
