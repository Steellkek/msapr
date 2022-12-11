namespace msapr.repository;

public class GenAlgRepo
{
    public void Go()
    {
        PCB Pcb = new RepoPCB().CreatePCB();
        var x = new populationRepo();
        var y = new GenomeRepo();
        var population =x.CreateFirstPopulation(6,Pcb);
        population.Genomes= x.GetNewParents(population);
        y.Crossingover(population.Genomes[0],population.Genomes[1]);
        var g = 5;
    }
}