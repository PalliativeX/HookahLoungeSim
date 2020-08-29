
public enum Strength
{
	None,
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

[System.Serializable]
public class Brand
{
	public string name;
	public Strength strength;
	public HeatTolerance heatTolerance;
	public float smokingTime;

	public Brand(string name, Strength strength, HeatTolerance heatTolerance, float smokingTime)
	{
		this.name = name;
		this.strength = strength;
		this.heatTolerance = heatTolerance;
		this.smokingTime = smokingTime;
	}
}

[System.Serializable]
public class Tobacco
{
	public Brand brand;
	public Flavour flavour;

	public Tobacco(string brandName, Strength brandStrength, HeatTolerance brandHeatTolerance, float brandSmokingTime, Flavour flavour)
	{
		this.brand = new Brand(brandName, brandStrength, brandHeatTolerance, brand.smokingTime);
		this.flavour = flavour;
	}

	public Tobacco(Brand brand, Flavour flavour)
	{
		this.brand = brand;
		this.flavour = flavour;
	}

}
