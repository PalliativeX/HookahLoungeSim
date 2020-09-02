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
	public float price;

	public Brand(string name, Strength strength, HeatTolerance heatTolerance, float smokingTime, float price)
	{
		this.name = name;
		this.strength = strength;
		this.heatTolerance = heatTolerance;
		this.smokingTime = smokingTime;
		this.price = price;
	}
}