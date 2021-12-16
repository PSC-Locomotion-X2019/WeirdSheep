namespace genome
{
	using Neat = neat.Neat;

	public class ConnectionGene : Gene
	{

		private NodeGene from;
		private NodeGene to;

		private float weight;
		private bool enabled = true;

		private int replaceIndex;
        public ConnectionGene()
		{
			
		}
		public ConnectionGene(NodeGene from, NodeGene to)
		{
			this.from = from;
			this.to = to;
		}

		public virtual NodeGene From
		{
			get
			{
				return from;
			}
			set
			{
				this.from = value;
			}
		}


		public virtual NodeGene To
		{
			get
			{
				return to;
			}
			set
			{
				this.to = value;
			}
		}


		public virtual float Weight
		{
			get
			{
				return weight;
			}
			set
			{
				this.weight = value;
			}
		}


		public virtual bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				this.enabled = value;
			}
		}

		public override bool Equals(object o)
		{
			if (!(o is ConnectionGene))
			{
				return false;
			}
			ConnectionGene c = (ConnectionGene) o;
			return (from.Equals(c.from) && to.Equals(c.to));
		}

		public override string ToString()
		{
			return "ConnectionGene{" +
					"from=" + from.Innovation_number +
					", to=" + to.Innovation_number +
					", weight=" + weight +
					", enabled=" + enabled +
					", innovation_number=" + innovation_number +
					'}';
		}

		public override int GetHashCode()
		{
			return from.Innovation_number * Neat.MAX_NODES + to.Innovation_number;
		}

		public virtual int ReplaceIndex
		{
			get
			{
				return replaceIndex;
			}
			set
			{
				this.replaceIndex = value;
			}
		}

	}

}