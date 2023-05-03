using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCompassData : MonoBehaviour
{

	public GameObject[] ObjToRotate;

	/*public int UpdateFrameCount = 10
	public int CoroutineLoopDelayFrame = 10*/
	private float Direction = 0f;
	public float UpdateValueDelay = 0.1f;                           //time between compass reading
	public float UpdateValNum = 10;                             //amount of value needed before computing avg

	public float UpdateValNumActual = 0;
	public List<float> ValueStored = new List<float>();       //stocking compass reading values

	public int CalculationMethodIndex = 1;                          //choose which calculation algorithm is chosen

	private void Start()
	{
		StartCoroutine(Init());
	}

	/*private void Update()
	{
		UpdateFrameCount += 1;
		if (UpdateFrameCount >= CoroutineLoopDelayFrame)
        {
			StartCoroutine(UpdateCoroutinet());
        }

	}*/

	public float GetDir()
    {
		return this.Direction;
    }

	private IEnumerator Init()
	{
		Input.location.Start();                                     //enable localisation service -> the user needs to accept the android popup
		Input.compass.enabled = true;                               //enable gyroscope

		if (!Input.compass.enabled)                                 //if compass isn't working
		{
			Debug.LogError("compass not working");
			yield return new WaitForSeconds(3);
			Debug.Log("retrying ...");
			StartCoroutine(Init());
		}
		else
		{
			Debug.LogWarning("compass enabled with success");
			if (CalculationMethodIndex == 1 && UpdateValNum < 4)
			{
				UpdateValNum = 4;
			}
			StartCoroutine(UpdateCoroutine());
		}
	}

	public IEnumerator UpdateCoroutine()
	{
		if (Input.compass.enabled)                          //if compass is working
		{
			switch (CalculationMethodIndex)
			{
				case 1: Method1(); break;
				case 2: Method2(); break;
			}
		}
		yield return new WaitForSeconds(UpdateValueDelay);
		StartCoroutine(UpdateCoroutine());
		/*while (true)
		{
			yield return new WaitForSeconds(UpdateValueDelay);
			StartCoroutine(UpdateCoroutine());
		}*/
	}


	public void Method1()                                  //Remove 2 extremum on each side of the list
	{
		if (UpdateValNumActual < UpdateValNum)
		{
			ValueStored.Add(Input.compass.magneticHeading);
			UpdateValNumActual += 1;
		}
		else
		{
			//COMPUTE AVG VALUE
			//this.Direction = 0;
			ValueStored.Sort();
			ValueStored.RemoveRange(ValueStored.Count - 2, 2);
			ValueStored.RemoveRange(0, 2);
			float sum_x = 0, sum_y = 0;
			for (int i = 0; i < ValueStored.Count; i++)
			{
				sum_x += Mathf.Cos(Mathf.Deg2Rad * ValueStored[i]);
				sum_y += Mathf.Sin(Mathf.Deg2Rad * ValueStored[i]);
			}
			float dir_x = 0, dir_y = 0;
			dir_x = sum_x / ValueStored.Count;
			dir_y = sum_y / ValueStored.Count;
			this.Direction = -(Mathf.Atan2(dir_x, dir_y) * Mathf.Rad2Deg - 90);

			/*//UPDATE OBJECT ROTATION
			foreach (GameObject OneObject in ObjToRotate)
			{
				OneObject.transform.rotation = Quaternion.Euler(0, 0, Moyenne);
			}*/

			ValueStored.Clear();                        //reset input value list
			UpdateValNumActual = 0;                     //reset
		}
		
	}



	public void Method2()                                  //Shift array used for avg everytime, 50 for list size and 0.05 delay
	{
		if (UpdateValNumActual < UpdateValNum)
		{
			ValueStored.Add(Input.compass.magneticHeading);
			UpdateValNumActual += 1;
		}
		else
		{
			//COMPUTE AVG VALUE
			//this.Direction = 0;
			ValueStored.RemoveAt(0);                  //Remove oldest value to update
			ValueStored.Add(Input.compass.magneticHeading);
			float sum_x = 0, sum_y = 0;
			for (int i = 0; i < ValueStored.Count; i++)
			{
				sum_x += Mathf.Cos(Mathf.Deg2Rad * ValueStored[i]);
				sum_y += Mathf.Sin(Mathf.Deg2Rad * ValueStored[i]);
			}
			float dir_x = 0, dir_y = 0;
			dir_x = sum_x / ValueStored.Count;
			dir_y = sum_y / ValueStored.Count;
			this.Direction = -(Mathf.Atan2(dir_x, dir_y) * Mathf.Rad2Deg - 90);

			/*//UPDATE OBJECT ROTATION
			foreach (GameObject OneObject in ObjToRotate)
			{
				OneObject.transform.rotation = Quaternion.Euler(0, 0, Moyenne);
			}*/
		}
	}





}
