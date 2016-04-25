using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace LuminousVector
{
	public class ChatUI : MonoBehaviour
	{
		public Text chat;
		public int characterLimit = 200;

		private Dialouge _curDialouge;
		private int _subDivisions;
		private int _divisionStart;
		private int _divisionEnd;
		private int _curSubDivision;
		private bool _choicesRendered = false;

		public void Proceed(int choice)
		{
			if (_subDivisions != 0)
				GetDivisionIndex();
			if (_choicesRendered)
				return;
			if (_curSubDivision == _subDivisions)
				GetNextDialouge(choice);
			else
			{
				_curSubDivision++;

			}
			Render();
		}

		private void GetDivisionIndex()
		{
			if (_curSubDivision == 0)
				_divisionStart = 0;
			string message = _curDialouge.message;
			if(_curSubDivision == _subDivisions)
			{
				_divisionEnd = message.Length;
				return;
			}
			for(int i = characterLimit * _curSubDivision; i > 0; i--)
			{
				if (message[i] == ' ')
					_divisionEnd = i;
			}
		}

		private void GetNextDialouge(int choice)
		{
			_subDivisions = 0;
			_curDialouge = default(Dialouge); // TODO: Get Dialouge
			if(_curDialouge.message.Length >= characterLimit)
				_subDivisions = Mathf.CeilToInt(_curDialouge.message.Length/characterLimit);
			_curSubDivision = 0;
		}

		private void Render()
		{
			if(_curDialouge.choices.Length != 0)
				if(_subDivisions != 0)
					RenderChoices();
				else if(_curSubDivision == _subDivisions)
					RenderChoices();
			chat.text = _curDialouge.message.Substring(_divisionStart, _divisionEnd - _divisionStart);
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
		public string label { get { return _label; } }
		public string speaker { get { return _speaker; } }
		public string message { get { return _message; } }
		public string[] choices { get { return _choices; } }

		private string _label;
		private string _speaker;
		private string _message;
		private string[] _choices;

		public Dialouge(string label, string speaker, string message)
		{
			_label = label;
			_speaker = speaker;
			_message = message;
			_choices = null;
		}

		public Dialouge(string label, string speaker, string message, string[] choices)
		{
			_label = label;
			_speaker = speaker;
			_message = message;
			_choices = choices;
		}
	}
}
