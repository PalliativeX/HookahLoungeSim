using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
	public static Vector3 ClosestPointOnSphere(Vector3 currentLoc, Vector3 sphereLoc, float radius)
	{
		float sqrt = Mathf.Sqrt(Mathf.Pow(currentLoc.x - sphereLoc.x, 2) + Mathf.Pow(currentLoc.z - sphereLoc.z, 2) + Mathf.Pow(currentLoc.y - sphereLoc.y, 2));

		Vector3 point = new Vector3
		{
			x = sphereLoc.x + radius * ((currentLoc.x - sphereLoc.x) / sqrt),
			y = 0f,
			z = sphereLoc.z + radius * ((currentLoc.z - sphereLoc.z) / sqrt)
		};

		return point;
	}
}
