namespace msapr;

public class Genome
{
    public List<Module> Modules { get; set; }
    public decimal Fitness { get; set; }
    public decimal NewFitness {get; set; }
}