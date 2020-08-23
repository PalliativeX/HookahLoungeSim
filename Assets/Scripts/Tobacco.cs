using UnityEngine;
using System.Collections.Generic;

public enum Strength
{
	Soft,
	Medium,
	Strong
}

public enum HeatTolerance
{
	Weak,
	Medium,
	Heatproof
}

public class Brand
{
	public string name; // Dairy Qookah
	public Strength strength;
	public HeatTolerance heatTolerance;
	public float smokingTime;
	public Flavour[] flavours;

	public Brand(string name, Strength strength, HeatTolerance heatTolerance, float smokingTime, Flavour[] flavours)
	{
		this.name = name;
		this.strength = strength;
		this.heatTolerance = heatTolerance;
		this.smokingTime = smokingTime;
		this.flavours = (Flavour[])flavours.Clone(); // prob needs deep copy or test
	}

	public bool HasFlavour(Flavour specifiedFlavour)
	{
		foreach (Flavour flavour in flavours)
		{
			if (specifiedFlavour == flavour)
				return true;
		}

		return false;
	}

	public Flavour GetRandomFlavour()
	{
		return flavours[Random.Range(0, flavours.Length - 1)];
	}

	// Get random fresh, berry, fruit taste, etc.
	public Taste GetRndTasteByGroup(FlavourGroup group)
	{
		List<Taste> tastes = new List<Taste>();

		foreach (Flavour fl in flavours)
		{
			if (fl.group == group)
			{
				tastes.Add(fl.taste);
			}
		}

		return tastes[Random.Range(0, flavours.Length - 1)];
	}

}
