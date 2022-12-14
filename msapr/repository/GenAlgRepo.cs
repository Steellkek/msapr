namespace msapr.repository;

public class GenAlgRepo
{
    public void Go()
    {
        PCB Pcb = new RepoPCB().CreatePCB();
        var x = new populationRepo();
        var y = new GenomeRepo();
        var population =x.CreateFirstPopulation(100,Pcb);
        population = x.GeneticOpertors(population, 1000);
        var g = 5;
    }

}