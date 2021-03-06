using SixtyMeters.scripts.level.missions;
using UnityEngine;

namespace SixtyMeters.scripts
{
	public class Cleanable : MonoBehaviour
	{

		public int secondsUntilClean;
		public bool spawnCreatureOnClean;
		public GameObject spawnableCreature;
		public GameObject spawnPoint;

		public MissionObjective missionObjective;

		private float _timePassedInCleaningCycle;
		private bool _isCleaning;
		private Cleaner _cleaner;
	
		private Material _material;
		private AudioSource _audioSource;

		// Start is called before the first frame update
		void Start()
		{
			_material = GetComponentInParent<MeshRenderer>().material;
			_audioSource = GetComponent<AudioSource>();
		}

		// Update is called once per frame
		void Update()
		{
			if (_isCleaning)
			{
				_timePassedInCleaningCycle += Time.deltaTime;
				var lerpTime = _timePassedInCleaningCycle / secondsUntilClean;
				SetAlpha(Mathf.Lerp(1, 0.4f,lerpTime), _material);
				if (!_audioSource.isPlaying)
				{
					_audioSource.Play();
				}
			}
	    
			if (_timePassedInCleaningCycle >= secondsUntilClean)
			{
				_cleaner.cleaningEffect.Stop();
				if (missionObjective)
				{
					missionObjective.percentageComplete = 100;
					//TODO: maybe do incremental updates to the progress
				}
				Destroy(transform.parent.gameObject);
			}
		}
    
		void OnTriggerEnter(Collider col)
		{
			if (IsCleaner(col))
			{

				if (spawnCreatureOnClean)
				{
					spawnCreatureOnClean = false;
					Instantiate(spawnableCreature, spawnPoint.transform.position, spawnPoint.transform.rotation);
				}
		    
				_cleaner = col.gameObject.GetComponent<Cleaner>();
				_cleaner.cleaningEffect.Play();
				_isCleaning = true;
			}
		}

		private void SetAlpha(float alpha, Material material)
		{
			var color = material.color;
			color.a = Mathf.Clamp( alpha, 0, 1 );
			material.color = color;
		}
    
		private static bool IsCleaner(Collider col)
		{
			return col.gameObject.GetComponent<Cleaner>() != null;
		}

		void OnTriggerExit(Collider col)
		{
			if (IsCleaner(col))
			{
				col.gameObject.GetComponent<Cleaner>().cleaningEffect.Stop();
				_timePassedInCleaningCycle = 0;
				_isCleaning = false;
				_audioSource.Stop();
			}
		}
	}
}
