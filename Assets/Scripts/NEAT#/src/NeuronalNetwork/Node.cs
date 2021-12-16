namespace NeuronalNetwork
{

	public class Node
	{
        private float x ;

        private float y; 
		private int activation;

        public Node(float   X, float Y,int a)
        {
            x = X;
            y = Y;
			activation=a;
        }

        public Node() {}
    
   public virtual float X
		{
			get
			{
				return x;
			}
            set
			{
				x=value;
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
				y=value;
			}}
			public virtual int Activation
		{
			get
			{
				return activation;
			}
            set
			{
				activation=value;
			}

    }
    }}
    