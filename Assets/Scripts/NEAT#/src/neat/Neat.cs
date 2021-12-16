using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;


namespace neat
{

	using ConnectionGene = genome.ConnectionGene;
	using Gene = genome.Gene;
	using Genome = genome.Genome;
	using NodeGene = genome.NodeGene;
	using Connection1=NeuronalNetwork.Connection;
	using Node1=NeuronalNetwork.Node;
	using Net=NeuronalNetwork.Net;



    

	public class Neat
	{

		public static readonly int MAX_NODES = (int)Math.Pow(2,20);


		private float C1_ = 1, C2_ = 1, C3_ = 1;

		private float CP_ = 8f;



		private float WEIGHT_SHIFT_STRENGTH_ = 2F;

		private float WEIGHT_RANDOM_STRENGTH_ = 20F;


		private float SURVIVORS_ = 0.7F;


		private float PROBABILITY_MUTATE_LINK_ = 0.4F;

		private float PROBABILITY_MUTATE_NODE_ = 0.4F;

		private float PROBABILITY_MUTATE_WEIGHT_SHIFT_ = 0.9F;

		private float PROBABILITY_MUTATE_WEIGHT_RANDOM_ = 0.01F;

		private float PROBABILITY_MUTATE_TOGGLE_LINK_ = 0.001F;

		private Dictionary<ConnectionGene, ConnectionGene> all_connections = new Dictionary<ConnectionGene, ConnectionGene>();
		private data_structures.RandomHashSet<NodeGene> all_nodes = new data_structures.RandomHashSet<NodeGene>();

		private data_structures.RandomHashSet<Client> clients = new data_structures.RandomHashSet<Client>();
		private data_structures.RandomHashSet<Species> species = new data_structures.RandomHashSet<Species>();

		private int max_clients;
		private int output_size;
		private int input_size;
        public  Neat() {}
		public Neat(int input_size, int output_size, int clients)
		{
			this.reset(input_size, output_size, clients);
		}

		public virtual Genome empty_genome()
		{
			Genome g = new Genome(this);
			for (int i = 0; i < input_size + output_size; i++)
			{
				g.Nodes.add(getNode(i + 1));
			}
			return g;
		}
		public virtual void reset(int input_size, int output_size, int clients)
		{
			this.input_size = input_size;
			this.output_size = output_size;
			this.max_clients = clients;

			all_connections.Clear();
			all_nodes.clear();
			this.clients.clear();

			for (int i = 0;i < input_size; i++)
			{
				NodeGene n = Node;
				n.X = 0.1F;
				n.Y = (float)((i + 1) / (float)(input_size + 1));
				n.acti();
			}

			for (int i = 0; i < output_size; i++)
			{
				NodeGene n = Node;
				n.X = 0.9F;
				n.Y = (float)((i + 1) / (float)(output_size + 1));
				n.acti();
			}

			for (int i = 0; i < max_clients; i++)
			{
				Client c = new Client();
				c.Genome = empty_genome();
				c.generate_calculator();
				this.clients.add(c);
			}
		}

		public virtual Client getClient(int index)
		{
			return clients.get(index);
		}

		public static ConnectionGene getConnection(ConnectionGene con)
		{
			ConnectionGene c = new ConnectionGene(con.From, con.To);
			c.Innovation_number = con.Innovation_number;
			c.Weight = con.Weight;
			c.Enabled = con.Enabled;
			return c;
		}
		public virtual ConnectionGene getConnection(NodeGene node1, NodeGene node2)
		{
			ConnectionGene connectionGene = new ConnectionGene(node1, node2);

			if (all_connections.ContainsKey(connectionGene))
			{
				connectionGene.Innovation_number = all_connections[connectionGene].Innovation_number;
			}
			else
			{
				connectionGene.Innovation_number = all_connections.Count + 1;
				all_connections[connectionGene] = connectionGene;
			}

			return connectionGene;
		}
		public virtual void setReplaceIndex(NodeGene node1, NodeGene node2, int index)
		{
			all_connections[new ConnectionGene(node1, node2)].ReplaceIndex = index;
		}
		public virtual int getReplaceIndex(NodeGene node1, NodeGene node2)
		{
			ConnectionGene con = new ConnectionGene(node1, node2);
			ConnectionGene data = all_connections[con];
			if (data == null)
			{
				return 0;
			}
			return data.ReplaceIndex;
		}

		public virtual NodeGene Node
		{
			get
			{
				NodeGene n = new NodeGene(all_nodes.size() + 1);
				all_nodes.add(n);
				return n;
			}
		}
		public virtual NodeGene getNode(int id)
		{
			if (id <= all_nodes.size())
			{
				return all_nodes.get(id - 1);
			}
			return Node;
		}
        public int size() 
		{
         return species.size();
		}
		public virtual void evolve()
		{
			gen_species();
			kill();
			remove_extinct_species();
			reproduce();
			mutate();
			foreach (Client c in clients.Data)
			{
				c.generate_calculator();
			}
		}

		public virtual void printSpecies()
		{
			Console.WriteLine("##########################################");
			foreach (Species s in this.species.Data)
			{
				Console.WriteLine(s + "  " + s.Score + "  " + s.size());
			}
		}

		private void reproduce()
		{
			data_structures.RandomSelector<Species> selector = new data_structures.RandomSelector<Species>();
			foreach (Species s in species.Data)
			{
				selector.add(s, s.Score);
			}

			foreach (Client c in clients.Data)
			{
				if (c.Species == null)
				{
					Species s = selector.random();
					c.Genome = s.breed();
					s.force_put(c);
				}
			}
		}

		public virtual void mutate()
		{
			foreach (Client c in clients.Data)
			{   if ((c.Score-10) <0.98*(this.bestscore()-10) || c.Score < 60 ){
				c.mutate();}
			}
		}

		private void remove_extinct_species()
		{    int l=1;
			    
			for (int i = species.size() - 1; i >= 0; i--)
			{   
				
				if (species.get(i).size() <= l)
				{   if ((species.get(i).size()==0) || (species.get(i).Score-10 < ((this.bestscore()-10)*0.5) ) ){
					species.get(i).goExtinct();
					species.remove(i);}
				}
			}
		}

		private void gen_species()
		{
			foreach (Species s in species.Data)
			{
				s.reset();
			}
			species.clear();

			foreach (Client c in clients.Data)
			{
				if (c.Species != null)
				{
					continue;
				}


				float found = this.CP_;
				Species sp=null;
				float dis;
				foreach (Species s in species.Data)
				{   dis=s.put(c);
					if (dis<found)
					{
						found = dis;
						sp=s;
						
						
					}
				}
				if (found<this.CP_)
				{   c.Species = sp;
				    sp.Clients.add(c);
					
				}
				else {
					species.add(new Species(c));
				}
			}

			foreach (Species s in species.Data)
			{
				s.evaluate_score();
			}
		}

		private void kill()
		{
			foreach (Species s in species.Data)
			{
				s.kill((float)(1 - SURVIVORS_));
			}
		}


		public static void Main(string[] args)
		{
			Neat neat = new Neat(10,1,1000);

			float[] @in = new float[10];
			for (int i = 0; i < 10; i++)
			{
				@in[i] = GlobalRandom.Nextfloat;
			}

			for (int i = 0; i < 500; i++)
			{
				foreach (Client c in neat.clients.Data)
				{
					float score = (c.calculate(@in)[0]);
					c.Score = score;
				}
				neat.evolve();
				neat.printSpecies();



				 Console.WriteLine(neat.BestClient.Score);
				 neat.printScoreInformation();
			}
				Console.WriteLine();
		}




		public virtual data_structures.RandomHashSet<Client> Clients
		{
			get
			{
				return clients;
			}
			set
			{
				this.clients = value;
			}
		}


		public virtual float CP
		{
			get
			{
				return CP_;
			}
			set
			{
				this.CP_ = value;
			}
		}


		public virtual float C1
		{
			get
			{
				return C1_;
			}
			set
			{
				C1_ = value;
			}
		}

		public virtual float C2
		{
			get
			{
				return C2_;
			}
			set
			{
				C2_ = value;
			}
		}

		public virtual float C3
		{
			get
			{
				return C3_;
			}
			set
			{
				C3_ = value;
			}
		}


		public virtual float WEIGHT_SHIFT_STRENGTH
		{
			get
			{
				return WEIGHT_SHIFT_STRENGTH_;
			}
			set
			{
				WEIGHT_SHIFT_STRENGTH_ = value;
			}
		}

		public virtual float WEIGHT_RANDOM_STRENGTH
		{
			get
			{
				return WEIGHT_RANDOM_STRENGTH_;
			}
			set
			{
				WEIGHT_RANDOM_STRENGTH_ = value;
			}
		}

		public virtual float PROBABILITY_MUTATE_LINK
		{
			get
			{
				return PROBABILITY_MUTATE_LINK_;
			}
			set
			{
				PROBABILITY_MUTATE_LINK_ = value;
			}
		}

		public virtual float PROBABILITY_MUTATE_NODE
		{
			get
			{
				return PROBABILITY_MUTATE_NODE_;
			}
			set
			{
				PROBABILITY_MUTATE_NODE_ = value;
			}
		}

		public virtual float PROBABILITY_MUTATE_WEIGHT_SHIFT
		{
			get
			{
				return PROBABILITY_MUTATE_WEIGHT_SHIFT_;
			}
			set
			{
				PROBABILITY_MUTATE_WEIGHT_SHIFT_ = value;
			}
		}

		public virtual float PROBABILITY_MUTATE_WEIGHT_RANDOM
		{
			get
			{
				return PROBABILITY_MUTATE_WEIGHT_RANDOM_;
			}
			set
			{
				PROBABILITY_MUTATE_WEIGHT_RANDOM_ = value;
			}
		}

		public virtual float PROBABILITY_MUTATE_TOGGLE_LINK
		{
			get
			{
				return PROBABILITY_MUTATE_TOGGLE_LINK_;
			}
			set
			{
				PROBABILITY_MUTATE_TOGGLE_LINK_ = value;
			}
		}

		public virtual int Output_size
		{
			get
			{
				return output_size;
			}
			set
			{
				this.output_size = value;
			}
		}

		public virtual int Input_size
		{
			get
			{
				return input_size;
			}
			set
			{
				this.input_size = value;
			}
		}

		public virtual int Max_clients
		{
			get
			{
				return max_clients;
			}
			set
			{
				this.max_clients = value;
			}
		}








		public virtual float SURVIVORS
		{
			get
			{
				return SURVIVORS_;
			}
			set
			{
				SURVIVORS_ = value;
			}
		}

		public virtual Dictionary<ConnectionGene, ConnectionGene> All_connections
		{
			get
			{
				return all_connections;
			}
			set
			{
				this.all_connections = value;
			}
		}

		public virtual data_structures.RandomHashSet<NodeGene> All_nodes
		{
			get
			{
				return all_nodes;
			}
			set
			{
				this.all_nodes = value;
			}
		}

		public virtual data_structures.RandomHashSet<Species> Species
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







        public float bestscore()
		{float best=0;
    
				foreach (Client t in this.clients.Data)
				{
					if (t.Score > best)
					{
						best = t.Score;
    
					}
    
				}
				return best;
				}


		public virtual Client BestClient
		{
			get
			{
				Client best = this.clients.Data[0];
    
				foreach (Client t in this.clients.Data)
				{
					if (t.Score > best.Score)
					{
						best = t;
    
					}
    
				}
				return best;
			}
		}
		public virtual void Savebestclient (int i){
			Net N=new Net();
			Client thebest=BestClient;
			Genome best=thebest.Genome;
			foreach(NodeGene Node in best.Nodes.Data){
				N.Nodes.Add(new Node1(Node.X,Node.Y,Node.Activation));


			}
			foreach(ConnectionGene c in best.Connections.Data){
				N.Connections.Add(new Connection1(new Node1(c.From.X,c.From.Y,c.From.Activation),new Node1(c.To.X,c.To.Y,c.To.Activation),(float) c.Weight));

			}
			foreach(Node1 Node in N.Nodes){
				StreamWriter sw = new StreamWriter("best"+i.ToString(),append: true);
                sw.WriteLine(Node.X);
            
				sw.Close();

			}

			var serializer = new DataContractSerializer(typeof(Net));
            var settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
            };
            var writer = XmlWriter.Create(".\\save\\Reseau"+i.ToString()+".xml", settings);
            serializer.WriteObject(writer, N);
            writer.Close();
			

            

		}
		public virtual void printScoreInformation()
		{
	Client best = this.clients.Data[0];
			float score = 0;
			foreach (Client t in this.clients.Data)
			{
				if (t.Score > best.Score)
				{
					best = t;

				}
				score = score + t.Score;

			}
			Console.WriteLine("best score :" + best.Score);
			Console.WriteLine("moy score  :" + score / this.max_clients);




		}


		public Client[] GetGeneration()
		{
			Client[] generation = new Client[max_clients];

			for (int i = 0; i < max_clients; i++)
			{
				Client c = getClient(i);
				generation[i] = c;
			}
			return generation;
		}

		public Client[] NextGeneration(float[] scores)
		{
			for (int i = 0; i < max_clients; i++)
			{
				Client c = this.getClient(i);
				c.setScore(scores[i]);
			}
			this.evolve();
			return this.GetGeneration();
		}



	}

}