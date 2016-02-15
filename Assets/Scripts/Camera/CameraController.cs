using UnityEngine;
using System.Collections;
namespace LuminousVector
{
	public class CameraController : MonoBehaviour
	{
		//Public
		public Vector2 minPos;
		public Vector2 maxPos;
		public float speed = 10;
		public float yLevel = 60;

		//Private
		private Vector3 _curPos;

		void Start ()
		{
			EventManager.StartListening(GameEvent.PLAY_FIELD_GENERATED, CameraStart);
			_curPos = transform.position;
			_curPos.y = yLevel;
		}

		void CameraStart()
		{
			EventManager.StopListening(GameEvent.PLAY_FIELD_GENERATED, CameraStart);
		}

		void Update ()
		{
			_curPos.y = yLevel;
			if (Input.GetKey(KeyCode.W))
				_curPos.z += speed * Time.deltaTime;
			else if (Input.GetKey(KeyCode.S))
				_curPos.z -= speed * Time.deltaTime;
			if (Input.GetKey(KeyCode.A))
				_curPos.x -= speed * Time.deltaTime;
			else if (Input.GetKey(KeyCode.D))
				_curPos.x += speed * Time.deltaTime;

			transform.position = _curPos;
		}
	}
}
