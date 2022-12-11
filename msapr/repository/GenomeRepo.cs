namespace msapr.repository;

public class GenomeRepo
{
    public Genome CreateFirstGenome(PCB pcb)
    {
        var moduleRepo = new ModuleRepo();
        var genome = new Genome();
        genome.Modules = moduleRepo.CreateModules(pcb);
        genome.Fitness = DetermineFitnes(genome.Modules);
        return genome;
    }

    private decimal DetermineFitnes(List<Module> modules)
    {
        decimal F = 0;
        decimal K = 0;
        foreach (var module in modules)
        {
            foreach (var element in module.Elements)
            {
                foreach (var adjElement in element.AdjElement)
                {
                    if (!module.Elements.Contains(adjElement.Item1))
                    {
                        F += adjElement.Item2;
                    }
                    if (module.Elements.Contains(adjElement.Item1))
                    {
                        K += adjElement.Item2;
                    }
                }
            }
        }
        return  K/F;
    }


    
}