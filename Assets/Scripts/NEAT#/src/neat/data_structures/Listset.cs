using System.Collections.Generic;

namespace data_structures
{
	using Gene = genome.Gene;


	public class RandomHashSet<T>
	{

		private HashSet<T> set;
		private List<T> data;

		public RandomHashSet()
		{
			set = new HashSet<T>();
			data = new List<T>();
		}

		public virtual bool contains(T @object)
		{
			return set.Contains(@object);
		}

		public virtual T random_element()
		{
			if (set.Count > 0)
			{
				return data[(int)(GlobalRandom.Nextfloat * size())];
			}
			return default(T);
		}

		public virtual int size()
		{
			return data.Count;
		}

		public virtual void add(T @object)
		{
			if (!set.Contains(@object))
			{
				set.Add(@object);
				data.Add(@object);
			}
		}

		public virtual void add_sorted(RandomHashSet<genome.ConnectionGene> hash,genome.ConnectionGene @object)
		{ 
			for (int i = 0; i < this.size(); i++)
			{
				int innovation = (hash.data[i]).Innovation_number;
				if (@object.Innovation_number < innovation)
				{
					hash.data.Insert(i, @object);
					hash.set.Add(@object);
					return;
				}
			}
			hash.data.Add(@object);
			hash.set.Add(@object);
		}

		public virtual void clear()
		{
			set.Clear();
			data.Clear();
		}

		public virtual T get(int index)
		{
			if (index < 0 || index >= size())
			{
				return default(T);
			}
			return data[index];
		}

		public virtual void remove(int index)
		{
			if (index < 0 || index >= size())
			{
				return;
			}
			set.Remove(data[index]);
			data.RemoveAt(index);
		}

		public virtual void remove(T @object)
		{
			set.Remove(@object);
			data.Remove(@object);
		}

		public virtual List<T> Data
		{
			get
			{
				return data;
			}
		}
	}

}