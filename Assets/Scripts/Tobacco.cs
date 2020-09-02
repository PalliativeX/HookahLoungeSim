

[System.Serializable]
public class Tobacco
{
	public Brand brand;
	public Flavour flavour;

	public Tobacco(string brandName, Strength brandStrength, HeatTolerance brandHeatTolerance, float brandSmokingTime, float brandPrice, Flavour flavour)
	{
		this.brand = new Brand(brandName, brandStrength, brandHeatTolerance, brandSmokingTime, brandPrice);
		this.flavour = flavour;
	}

	public Tobacco(Brand brand, Flavour flavour)
	{
		this.brand = brand;
		this.flavour = flavour;
	}

	public string NameStr()
	{
		return brand.name + " " + flavour.taste.ToString();
	}

	public override string ToString()
	{
		return
			"Name: " + brand.name + "\n" +
			"Strength: " + brand.strength + "\n" +
			"Heat tolerance: " + brand.heatTolerance + "\n" +
			"Smoking time: " + brand.smokingTime + " minutes" + "\n" +
			"Flavour group: " + flavour.group + "\n" +
			"Taste: " + flavour.taste;
			
	}
}
