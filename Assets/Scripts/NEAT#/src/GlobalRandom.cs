//---------------------------------------------------------------------------------------------------------
//	Copyright © 2007 - 2018 Tangible Software Solutions, Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class is used to replace calls to the static java.lang.Math.random method.
//---------------------------------------------------------------------------------------------------------
internal static class GlobalRandom
{
	private static System.Random randomInstance = null;

	public static float Nextfloat
	{
		get
		{
			if (randomInstance == null)
				randomInstance = new System.Random();

			return (float) randomInstance.NextDouble();
		}
	}
}