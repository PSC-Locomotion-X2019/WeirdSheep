using System.Collections.Generic;

namespace data_structures
{

	public class RandomSelector<T>
	{

		private List<T> objects = new List<T>();
		private List<float> scores = new List<float>();

		private float total_score = 0;

		public virtual void add(T element, float score)
		{
			objects.Add(element);
			scores.Add(score);
			total_score += score;
		}

		public virtual T random()
		{
			float v = GlobalRandom.Nextfloat * total_score;
			float c = 0;
			for (int i = 0; i < objects.Count; i++)
			{
				c += scores[i];
				if (c >= v)
				{
					return objects[i];
				}
			}
			return default(T);
		}

		public virtual void reset()
		{
			objects.Clear();
			scores.Clear();
			total_score = 0;
		}

	}

}