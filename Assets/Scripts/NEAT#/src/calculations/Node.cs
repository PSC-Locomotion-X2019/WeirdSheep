using System;
using System.Collections.Generic;

namespace calculations
{

	public class Node : IComparable<Node>
	{

		private float x;
		private float output;
		private List<Connection> connections = new List<Connection>();
		private int activation;

        public Node()
		{
			
		}
		public Node(float x,int activation )
		{
			this.x = x;
			this.activation=activation;
		}

		public virtual void calculate()
		{
			float s = 0;
			foreach (Connection c in connections)
			{
				if (c.Enabled)
				{
					s += c.Weight * c.From.Output;
				}
			}
			output = activation_function(s);
		}

		private float activation_function(float x)
		{   
	     return  (float) (Math.Sign(x)/(1+Math.Exp(-Math.Abs(x))));}
	
		
	
		public virtual float X
		{
			set
			{
				this.x = value;
			}
			get
			{
				return x;
			}
		}

		public virtual float Output
		{
			set
			{
				this.output = value;
			}
			get
			{
				return output;
			}
		}

		public virtual List<Connection> Connections
		{
			set
			{
				this.connections = value;
			}
			get
			{
				return connections;
			}
		}





		public  int CompareTo(Node o)
		{
			if (this.x > o.x)
			{
				return -1;
			}
			if (this.x < o.x)
			{
				return 1;
			}
			return 0;
		}
	}

}