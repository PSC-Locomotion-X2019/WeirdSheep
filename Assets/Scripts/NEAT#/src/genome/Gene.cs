namespace genome
{
	public class Gene
	{

		public int innovation_number;

		public Gene(int innovation_number)
		{
			this.innovation_number = innovation_number;
		}

		public Gene()
		{

		}

		public virtual int Innovation_number
		{
			get
			{
				return innovation_number;
			}
			set
			{
				this.innovation_number = value;
			}
		}

	}

}