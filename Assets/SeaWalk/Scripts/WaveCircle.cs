using UnityEngine;
using System.Collections;

public class WaveCircle : MonoBehaviour
{
	public GameObject SingleWavePrefab = null;

	public int WaveCount = 4;

	public void Update()
	{
		if ((waveInstances == null) ||
			(waveInstances.Length != WaveCount))
		{
			if (waveInstances != null)
			{
				foreach (GameObject waveInstance in waveInstances)
				{
					Destroy(waveInstance);
				}

				waveInstances = null;
			}

			waveInstances = new GameObject[WaveCount];

			for (int waveIndex = 0;
				waveIndex < waveInstances.Length;
				++waveIndex)
			{
				float waveSeriesFraction = (waveIndex / (float)waveInstances.Length);

				Quaternion waveOrientation = 
					Quaternion.AngleAxis(
						(waveSeriesFraction * 180),
						Vector3.forward);

				GameObject waveInstance = 
					GameObject.Instantiate(SingleWavePrefab) as GameObject;

				waveInstance.transform.parent = transform;
				waveInstance.transform.localPosition = Vector3.zero;
				waveInstance.transform.localRotation = waveOrientation;
				waveInstance.transform.localScale = Vector3.one;

				// Offset the wave's animation.
				{
					WaveyLineAnimator waveAnimator = 
						waveInstance.GetComponent<WaveyLineAnimator>();

					waveAnimator.CurrentAnimationFraction = waveSeriesFraction;
				}

				waveInstances[waveIndex] = waveInstance;
			}
		}
	}

	public GameObject[] waveInstances = null;
}
