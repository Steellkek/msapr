namespace msapr.repository;

public class GenAlgRepo
{
    public void Go()
    {
        PCB Pcb = new RepoPCB().CreatePCB();
        var x = new populationRepo();
        var t =x.CreateFirstPopulation(10,Pcb);
        var g = 5;
    }
}