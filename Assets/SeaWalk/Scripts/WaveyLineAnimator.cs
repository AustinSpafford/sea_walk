using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class WaveyLineAnimator : MonoBehaviour
{
	public float WavePeriod = 3.0f;
	public float WaveAmplitude = 0.5f;
	public float WaveAmplitudeStartMultiplier = 1.0f;
	public float WaveAmplitudeEndMultiplier = 1.0f;
	public float WaveWavelength = 0.25f;

	[Range(1, 1000)]
	public int WaveSegmentCount = 30;

	public float CurrentAnimationFraction = 0.0f;

	public bool DebugEnabled = false;

	public void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	public void Update()
	{
		// Advance the animation.
		if (Mathf.Approximately(WavePeriod, 0.0f) == false)
		{
			CurrentAnimationFraction = 
				Mathf.Repeat(
					(CurrentAnimationFraction + (Time.deltaTime / WavePeriod)),
					1.0f);
		}
		
		// Update the line renderer to accept our new positions.
		{
			int neededPositionCount = (WaveSegmentCount + 1);

			if ((scratchLinePositions == null) ||
				(scratchLinePositions.Length != neededPositionCount))
			{
				scratchLinePositions = new Vector3[neededPositionCount];

				lineRenderer.SetVertexCount(scratchLinePositions.Length);
			}
		}

		// Generate new positions and push them to the line renderer.
		{
			for (int positionIndex = 0;
				positionIndex < scratchLinePositions.Length;
				++positionIndex)
			{
				float positionFraction = (positionIndex / (float)(scratchLinePositions.Length - 1));

				float waveSampleFraction = (
					(positionFraction / WaveWavelength) +
					CurrentAnimationFraction);

				float waveAmplitudeAtPosition = (
					WaveAmplitude *
					Mathf.Lerp(
						WaveAmplitudeStartMultiplier, 
						WaveAmplitudeEndMultiplier, 
						positionFraction));

				scratchLinePositions[positionIndex] = 
					new Vector3(
						0.0f,
						(waveAmplitudeAtPosition * Mathf.Sin((2 * Mathf.PI) * waveSampleFraction)),
						positionFraction);
			}

			lineRenderer.SetPositions(scratchLinePositions);
		}
	}
	
	private LineRenderer lineRenderer = null;

	private Vector3[] scratchLinePositions = null;
}
