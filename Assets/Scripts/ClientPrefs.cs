
[System.Serializable]
public struct ClientPrefs
{
	public FlavourGroup group;
	public Taste taste;
	public Strength strength;

	public ClientPrefs(FlavourGroup group = FlavourGroup.None, Taste taste = Taste.None, Strength strength = Strength.None)
	{
		this.group = group;
		this.taste = taste;
		this.strength = strength;
	}

	public bool SatisfiesPrefs(Tobacco tobacco)
	{
		if (group != FlavourGroup.None && group != tobacco.flavour.group)
		{
			return false;
		}
		if (taste != Taste.None && taste != tobacco.flavour.taste)
		{
			return false;
		}
		if (strength != Strength.None && strength != tobacco.brand.strength)
		{
			return false;
		}

		return true;
	}
}