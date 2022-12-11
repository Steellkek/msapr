namespace msapr.repository;

public class populationRepo
{
    public Population CreateFirstPopulation(int k, PCB pcb)
    {
        var genomeRepo = new GenomeRepo();
        Population population = new Population();
        for (int i = 0; i < k; i++)
        {
            population.Genomes.Add(genomeRepo.CreateFirstGenome(pcb));
        }
        DetermineNewFitnes(population);
        population.SumFitness = population.Genomes.Sum(x => x.NewFitness);
        return population;
    }

    private void DetermineNewFitnes(Population population)
    {
        decimal dMin = 1;
        decimal dMax = 10;
        var min = population.Genomes.Min(x => x.Fitness);
        var max = population.Genomes.Max(x => x.Fitness);
        var a = (dMin - dMax) / (min - max);
        var b = dMin - a * min;
        foreach (var genome in population.Genomes)
        {
            genome.NewFitness = a * genome.Fitness + b;
        }
    }

    public List<Genome> GetNewParents(Population population)
    {
        Random ran = new Random();
        var Chances = new List<decimal>();
        Chances.Add( population.Genomes[0].NewFitness / population.SumFitness);
        for (int i = 1; i <population.Genomes.Count; i++)
        {
            Chances.Add( Chances[i-1]+ population.Genomes[i].NewFitness/population.SumFitness);
        }

        var Parents = new List<Genome>();
        for (int i = 0; i < population.Genomes.Count; i++)
        {
            var chanse = (decimal)ran.NextDouble();
            if (chanse<Chances[0])
            {
                Parents.Add(population.Genomes[i]);
                continue;
            }

            for (int j = 1; j < Chances.Count; j++)
            {
                if ((chanse<Chances[j])&(chanse>Chances[j-1]))
                {
                    Parents.Add(population.Genomes[j]);
                    break;
                }
            }
        }
        return Parents;
    }
}