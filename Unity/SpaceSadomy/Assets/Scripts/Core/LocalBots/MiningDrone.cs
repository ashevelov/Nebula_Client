using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LocalBot
{

	public class MiningDrone : MonoBehaviour {


		//private delegate bool BotOperation();

		private Dictionary<Stage, BotOperation> _opirations;
		
		public Stage curentOperation;
		public List<Stage> botOpirations = new List<Stage>();

		public HomeWhite homeWhite;
		public Undocking undocking;
		public GoToAsteroid goToAsteroid;
		public Mining mining;
		public GoToHome goToHome;
		public Docking docking;


		void Start()
		{
			_opirations  = new Dictionary<Stage, BotOperation>{
				{Stage.HomeWhite, this.homeWhite},
				{Stage.Undocking, this.undocking},
				{Stage.GoToAsteroid, this.goToAsteroid},
				{Stage.Mining, this.mining},
				{Stage.GoToHome, this.goToHome},
				{Stage.Docking, this.docking}
			};

			foreach(var botOper in _opirations)
			{
				botOper.Value.transform = transform;
			}
		}

		void Update()
		{
			_opirations[curentOperation].Run();
		}

#region HomeWhite
		[System.Serializable]
		public class HomeWhite : BotOperation
		{
			public Transform transform{get; set;}

			public float whiteTime = 5;

			private float _time;

			public List<Stage> nextStage = new List<Stage>{Stage.Undocking};
			public Stage GetNextStage()
			{
				return nextStage[Random.Range(0, nextStage.Count)];
			}

			public bool Run()
			{
				_time -= Time.deltaTime;
				if(_time <=0)
				{
					_time = whiteTime;
					return true;
				}

				return false;
			}
		}
#endregion

#region Undoking
		[System.Serializable]
		public class Undocking : BotOperation
		{
			public Transform transform{get; set;}

			public Transform undockPoint;
			public float undockSpeed;

			private float _speed = 0;

			public List<Stage> nextStage = new List<Stage>{Stage.GoToAsteroid};
			public Stage GetNextStage()
			{
				return nextStage[Random.Range(0, nextStage.Count)];
			}
			
			public bool Run()
			{
				if(_speed  < undockSpeed)
				{
					_speed += undockSpeed *Time.deltaTime;
				}

				if(Vector3.Distance(transform.position, undockPoint.position) > 0.1f)
				{
					transform.LookAt(undockPoint);
					transform.position += transform.forward * _speed * Time.deltaTime;
				}
				else
				{
					return true;
				}

				return false;
			}
		}
#endregion


#region GoToAsteroid
		[System.Serializable]
		public class GoToAsteroid : BotOperation
		{
			public Transform transform{get; set;}
			
			public List<Stage> nextStage = new List<Stage>{Stage.Mining};
			public Stage GetNextStage()
			{
				return nextStage[Random.Range(0, nextStage.Count)];
			}

			public bool Run()
			{
				return false;
			}
		}
#endregion

#region Mining
		[System.Serializable]
		public class Mining : BotOperation
		{
			public Transform transform{get; set;}
			
			public List<Stage> nextStage = new List<Stage>{Stage.GoToHome};
			public Stage GetNextStage()
			{
				return nextStage[Random.Range(0, nextStage.Count)];
			}

			public bool Run()
			{
				return false;
			}
		}
#endregion

#region GoToHome
		[System.Serializable]
		public class GoToHome : BotOperation
		{
			public Transform transform{get; set;}
			
			
			public List<Stage> nextStage = new List<Stage>{Stage.GoToAsteroid};
			public Stage GetNextStage()
			{
				return nextStage[Random.Range(0, nextStage.Count)];
			}

			public bool Run()
			{
				return false;
			}
		}
#endregion

#region Docking
		[System.Serializable]
		public class Docking : BotOperation
		{
			public Transform transform{get; set;}
			
			
			public List<Stage> nextStage = new List<Stage>{Stage.GoToAsteroid};
			public Stage GetNextStage()
			{
				return nextStage[Random.Range(0, nextStage.Count)];
			}

			public bool Run()
			{
				return false;
			}
		}
#endregion


	}

	public interface BotOperation
	{
		Transform transform{get; set;}
		Stage GetNextStage();
		bool Run();
	}
	
	public enum Stage
	{
		HomeWhite,
		Undocking,
		GoToAsteroid,
		Mining,
		GoToHome,
		Docking
	}
}
