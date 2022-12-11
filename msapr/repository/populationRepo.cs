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
}