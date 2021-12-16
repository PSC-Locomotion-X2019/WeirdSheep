namespace neat
{
	using Calculator = calculations.Calculator;
	using Genome = genome.Genome;

	public class Client
	{

		private Calculator calculator;

		private Genome genome;
		private float score;
		private Species species;

        public Client()
		{   
			
		}
		public virtual void generate_calculator()
		{
			this.calculator = new Calculator(genome);
		}

		public virtual float[] calculate(params float[] input)
		{
			if (this.calculator == null)
			{
				generate_calculator();
			}
			return this.calculator.calculate(input);
		}

		public virtual float distance(Client other)
		{
			return this.Genome.distance(other.Genome);
		}

		public virtual void mutate()
		{
			Genome.mutate();
		}

		public virtual Calculator Calculator
		{
			get
			{
				return calculator;
			}
		}

		public virtual Genome Genome
		{
			get
			{
				return genome;
			}
			set
			{
				this.genome = value;
			}
		}


		public virtual float Score
		{
			get
			{
				return score;
			}
			set
			{
				this.score = value;
			}
		}


		public virtual Species Species
		{
			get
			{
				return species;
			}
			set
			{
				this.species = value;
			}
		}
  public float getScore() {
        return score;
    }

		public void setScore(float value)
		{
			this.score = value;
		}

		public int CompareTo(Client o2) {
                        return this.getScore().CompareTo( o2.getScore())            ;

}
	}}