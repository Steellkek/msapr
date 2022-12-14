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
        //DetermineNewFitnes(population);
        population.SumFitness = population.Genomes.Sum(x => x.Fitness);
        return population;
    }

    /*public void DetermineNewFitnes(Population population)
    {
        decimal dMin = 1;
        decimal dMax = 100;
        var min = population.Genomes.Min(x => x.Fitness);
        var max = population.Genomes.Max(x => x.Fitness);
        var a = (dMin - dMax) / (min - max);
        var b = dMin - a * min;
        foreach (var genome in population.Genomes)
        {
            genome.NewFitness = a * genome.Fitness + b;
        }
        population.SumNewFitness = population.Genomes.Sum(x => x.NewFitness);
        population.SumFitness = population.Genomes.Sum(x => x.Fitness);
    }*/

    public List<Genome> GetNewParents(Population population)
    {
        Random ran = new Random();
        var Chances = new List<decimal>();
        var x = population.Genomes.Count * (population.Genomes.Count + 1) / 2;
        Chances.Add( (decimal)1 / x);
        for (int i = 1; i <population.Genomes.Count; i++)
        {
            Chances.Add( Chances[i-1]+ ((decimal)(i+1)/x));
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

    public Population GeneticOpertors(Population population, int count)
    {
        var g = new GenomeRepo();
        var ListSumFitness = new List<decimal>();
        ListSumFitness.Add(population.SumFitness);
        var listBestGen = new List<decimal>();
        listBestGen.Add(population.Genomes.Max(x=>x.Fitness));
        for (int i = 0; i < count; i++)
        {
            population.Genomes= population.Genomes.OrderBy(x => x.Fitness).ToList();
            population.Genomes2= GetNewParents(population);
            population=Crossingover(population,0.95);
            foreach (var genome in population.Genomes2)
            {
                genome.Fitness = g.DetermineFitnes(genome.Modules);
            }
            population = Mutation(population);
            foreach (var genome in population.Genomes)
            {
                genome.Fitness = g.DetermineFitnes(genome.Modules);
            }

            population.Genomes = Otbor(population.Genomes, population.Genomes2);
            population.SumFitness = population.Genomes.Sum(x => x.Fitness);
            population.Genomes= population.Genomes.OrderBy(x => x.Fitness).ToList();
            ListSumFitness.Add(population.SumFitness);
            listBestGen.Add(population.Genomes.Max(x=>x.Fitness));
            var x = 6;
        }
        return population;
    }

    public List<Genome> Otbor(List<Genome> genomes1, List<Genome> genomes2)
    {
        var newList = genomes1.Concat(genomes2).OrderBy(x=>x.Fitness).Reverse().ToList();
        return newList.GetRange(0,genomes1.Count);
    }

    public Population Mutation(Population population)
    {
        var g = new GenomeRepo();
        for (int i = 0; i < population.Genomes2.Count; i ++)
        {
            population.Genomes2[i].Modules = g.Mutation(population.Genomes2[i]);
        }
        return population;
    }

    public Population Crossingover(Population population,double chance)
    {
        var rand = new Random();
        var y = new GenomeRepo();
        population.Genomes2.Clear();
        for (int i = 0; i < population.Genomes.Count; i=i+2)
        {
            var newChance = rand.NextDouble();
            var point = rand.Next(0,population.Genomes[i].Modules.Sum(x=>x.Cnt));
            if (chance>newChance&&!y.CheckEquality(population.Genomes[i],population.Genomes[i+1]))
            {
                var child1 = y.GetChild(population.Genomes[i], population.Genomes[i + 1], point);
                var child2 = y.GetChild(population.Genomes[i+1], population.Genomes[i], point);
                population.Genomes2.Add(new Genome());
                population.Genomes2[i].Modules = child1;
                population.Genomes2.Add(new Genome());
                population.Genomes2[i + 1].Modules = child2;
                var x = 5;
            }
            else
            {
                population.Genomes2.Add(new Genome());
                population.Genomes2[i].Modules = population.Genomes[i].Modules;
                population.Genomes2.Add(new Genome());
                population.Genomes2[i + 1].Modules = population.Genomes[i+1].Modules;
            }
        }
        return population;
    }
}