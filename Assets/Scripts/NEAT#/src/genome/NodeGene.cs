using System;

namespace genome
{
	public class NodeGene : Gene
	{


		private float x, y;
		private int activation;
        public NodeGene() 
		{
		}
		public NodeGene(int innovation_number) 
		{ 
		  this.innovation_number=innovation_number;
		  Random _random = new Random(); 
          this.activation= _random.Next(9); 
		}
		public void acti(){
			   
          this.activation= 0; 
		}
		
		

		public virtual float X
		{
			get
			{
				return x;
			}
			set
			{
				this.x = value;
			}
		}
		public virtual int Activation
		{
			get
			{
				return activation;
			}
			set
			{
				this.activation = value;
			}
		}


		public virtual float Y
		{
			get
			{
				return y;
			}
			set
			{
				this.y = value;
			}
		}


		public override bool Equals(object o)
		{
			if (!(o is NodeGene))
			{
				return false;
			}
			return innovation_number == ((NodeGene) o).Innovation_number;
		}

		public override string ToString()
		{
			return "NodeGene{" +
					"innovation_number=" + innovation_number +
					'}';
		}

		public override int GetHashCode()
		{
			return innovation_number;
		}
	}

}