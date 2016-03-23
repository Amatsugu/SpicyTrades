using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace LuminousVector
{
	public class ChatUI : MonoBehaviour
	{
		public Text chat;


		private Dialouge _curDialouge;
		private int _subDivisions;
		private int _curSubDivision;
		private bool _choicesRendered = false;

		public void Proceed()
		{
			if (_choicesRendered)
				return;
			if (_curSubDivision == _subDivisions)
				_curDialouge = default(Dialouge); // TODO: Get Dialouge
			else
				_curSubDivision++;
			Render();
		}

		public void Proceed(int choice)
		{
			_curDialouge = default(Dialouge); // TODO: Get Dialouge(2)
			Render();
		}

		private void Render()
		{
			if(_subDivisions > 0)
				if(_curDialouge.choices.Length != 0)
					RenderChoices();
			else if(_curSubDivision == _subDivisions)
				if (_curDialouge.choices.Length != 0)
					RenderChoices();
			// TODO: Render Text
		}

		private void RenderChoices()
		{
			if (_choicesRendered)
				UnRenderChoices();
			_choicesRendered = true;
			// TODO: Render Choices
		}

		private void UnRenderChoices()
		{
			_choicesRendered = false;
			// TODO: UnRender Choices
		}

		
	}

	public class Dialouge
	{
		public string speaker { get { return _speaker; } }
		public string message { get { return _message; } }
		public string[] choices { get { return _choices; } }

		private string _speaker;
		private string _message;
		private string[] _choices;

		public Dialouge(string speaker, string message)
		{
			_speaker = speaker;
			_message = message;
			_choices = null;
		}

		public Dialouge(string speaker, string message, string[] choices)
		{
			_speaker = speaker;
			_message = message;
			_choices = choices;
		}
	}
}
