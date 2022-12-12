namespace msapr.repository;

public class GenAlgRepo
{
    public void Go()
    {
        PCB Pcb = new RepoPCB().CreatePCB();
        var x = new populationRepo();
        var y = new GenomeRepo();
        var population =x.CreateFirstPopulation(100,Pcb);
        population.Genomes= population.Genomes.OrderBy(x => x.Fitness).ToList();
        //for (int i = 0; i < 50; i++)
        //{
            population.Genomes= x.GetNewParents(population);
            y.Mutation(population.Genomes[0]);
            //population= x.Crossingover(population,0.95);
            /*foreach (var genome in population.Genomes)
            {
                genome.Fitness = y.DetermineFitnes(genome.Modules);
            }
            population.SumFitness = population.Genomes.Sum(x => x.Fitness);
            population.Genomes= population.Genomes.OrderBy(x => x.Fitness).ToList();
            var p = 0;
        }*/
        var g = 5;
    }

}