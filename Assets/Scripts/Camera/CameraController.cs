using UnityEngine;
using System.Collections;
namespace LuminousVector
{
	public class CameraController : MonoBehaviour
	{
		//Public
	
		//Private
		void Start ()
		{
			EventManager.StartListening(GameEvents.PLAY_FIELD_GENERATED, CameraStart);
		}

		void CameraStart()
		{
			EventManager.StopListening(GameEvents.PLAY_FIELD_GENERATED, CameraStart);
		}

		void Update ()
		{
			
		}
	}
}
