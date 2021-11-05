# WeirdSheep
Unity Project to teach generic skeleton 2D creatures to learn to walk on randomly generated ground.
Creature parts : 
- Bones (rigid rectangles, contact with ground forbidden)
- Muscles (modified SpringJoint2D between two bones centers, no mass, no hitbox) : parameters = { min length, max length, k spring constant, damping}
- Articulations (rigid spheres with mass linking two or more bones, contact with ground forbidden)
- Paws (rigid spheres with mass, contact with ground allowed and tracked)
Physics Engine : Unity 2D, every creature instantiated on a different floor level (all creatures see the same ground but are located separately to avoid hitboxes problems)
Ground : Identical for all childs of a generation, continuous segments of angle from horizon uniformly distributed between [-bound,bound]
Learning Algorithm used : NEAT (genetic algorithm on the geometric structure of a neural network)
Neurals inputs/outputs : Freely editable, see code for default


PSC Project (2nd year undergrad year project) at école Polytechnique
Implemented by :
- BICHOT Lilian
- BIENVENU Marie
- DHOUIB Mohamed
- DREYER Simon
- PELUSO Nathan

With the help of Polytechnique's LIX and "Science x Games" chair
