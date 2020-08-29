
// NOTE: A generic division of tastes
[System.Serializable]
public enum FlavourGroup
{
	None,
	Berries,
	Fresh,
	Spicy,
	Fruit
}

[System.Serializable]
public enum Taste
{
	None,
	Mint,
	Cherry,
	DoubleApple,
	Bacon,
	Banana,
	RedTea,
}

[System.Serializable]
public class Flavour
{
	public FlavourGroup group;
	public Taste taste;

	public Flavour(FlavourGroup group, Taste taste)
	{
		this.group = group;
		this.taste = taste;
	}

	public static bool operator ==(Flavour fl1, Flavour fl2)
	{
		return (fl1.taste == fl2.taste && fl1.group == fl2.group);
	}

	public static bool operator !=(Flavour fl1, Flavour fl2)
	{
		return !(fl1 == fl2);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is Flavour))
		{
			return false;
		}

		var flavour = (Flavour)obj;
		return taste == flavour.taste &&
			   group == flavour.group;
	}

	public override int GetHashCode()
	{
		var hashCode = 1466067192;
		hashCode = hashCode * -1521134295 + taste.GetHashCode();
		hashCode = hashCode * -1521134295 + group.GetHashCode();
		return hashCode;
	}
}