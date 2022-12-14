using System.Diagnostics.CodeAnalysis;

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

    
    private void PrintNumber(List<Element> elements)
    {
        foreach (var VARIABLE in elements)
        {
            Console.Write(VARIABLE.Number);
        }
        Console.WriteLine();
    }

    public List<Module> CopyListModule(List<Module> modules)
    {
        var copyModules = new List<Module>();
        foreach (var module in modules)
        {
            var newModule = new Module(module.Cnt, module.Square);
            module.Elements.ForEach(e=>newModule.Elements.Add(e));
            copyModules.Add(newModule);
        }

        return copyModules;
    }

    public List<Module> GetChild(Genome genome1,Genome genome2,int x)
    {
        
        var list1 = GetElementsFromModule(genome1.Modules);
        var list2 = GetElementsFromModule(genome2.Modules);
        var child = CopyListModule(genome1.Modules);
        //genome1.Modules.ForEach(e=>child.Add(e));
        PrintNumber(list1);
        PrintNumber(list2);

        for (int l = 0; l < genome1.Modules.Sum(x=>x.Elements.Count); l++)
        {
            var j = 0;
            foreach (var module in child)
            {
                module.Elements.Clear();
            }

            foreach (var module in child)
            {
                for (int i = j; i < list2.Count && module.Cnt > module.Elements.Count; i++)
                {
                    if (j <= x)
                    {
                        module.Elements.Add(list1[j]);
                        j += 1;
                    }
                    else if ((list2[i].Squre + module.Elements.Sum(x => x.Squre) < module.Square) &&
                             (!ExistInModule(child, list2[i])))
                    {
                        module.Elements.Add(list2[i]);
                        list2.Remove(list2[i]);
                        i--;
                    }

                    if (module.Cnt == module.Elements.Count)
                    {
                        break;
                    }
                }
            }

            foreach (var module in child)
            {
                if (module.Cnt > module.Elements.Count)
                {
                    for (int i = 0; i < list2.Count && module.Cnt > module.Elements.Count; i++)
                    {
                        if ((list2[i].Squre + module.Elements.Sum(x => x.Squre) < module.Square) &&
                            (!ExistInModule(child, list2[i])))
                        {
                            module.Elements.Add(list2[i]);
                            list2.Remove(list2[i]);
                            i--;
                        }
                    }
                }
            }

            if (child.Sum(x=>x.Elements.Count)!=genome1.Modules.Sum(x=>x.Cnt))
            {
                list1 = GetElementsFromModule(genome1.Modules);
                list2 = leftShufle(GetElementsFromModule(genome2.Modules));
            }
            else
            {
                break;
            }
        }   
        PrintNumber(GetElementsFromModule(child));
        
        if (child.Sum(x=>x.Elements.Count)!=genome1.Modules.Sum(x=>x.Cnt))
        {
            return genome1.Modules;
        }
        else
        {
            return child;
        }
        
    }

    public List<Module> Mutation(Genome genome)
    {

        Random ran = new();
        int position1=0;
        int position2=0;

        var list = GetElementsFromModule(genome.Modules);
        var listTuple = new List<Tuple<int, int>>();
        var mutatationModule = CopyListModule(genome.Modules);

        var cnt = genome.Modules.Sum(x => x.Cnt) * (genome.Modules.Sum(x => x.Cnt) - 1) / 2;
        for (int j = 0; j < cnt; j++)
        {
            foreach (var module in mutatationModule)
            {
                module.Elements.Clear();
            }
            do
            {
                position1 = ran.Next(0, list.Count);
                position2 = ran.Next(0, list.Count);
                while (position1 == position2)
                {
                    position2 = ran.Next(0, list.Count);
                }
            } while (listTuple.Contains(new Tuple<int, int>(position1, position2)));

            listTuple.Add(new Tuple<int, int>(position1,position2));
            listTuple.Add(new Tuple<int, int>(position2,position1));
            (list[position1], list[position2])= (list[position2], list[position1]);

            foreach (var module in mutatationModule)
            {
                for (int i = 0; i < list.Count && module.Cnt > module.Elements.Count; i++)
                {
                    module.Elements.Add(list[i]);
                    list.Remove(list[i]);
                    i--;
                }
            }
            if (mutatationModule.Sum(x=>x.Elements.Count)!=genome.Modules.Sum(x=>x.Cnt))
            {
                list = GetElementsFromModule(genome.Modules);
            }
            else
            {
                break;
            }
        }

        if (mutatationModule.Sum(x=>x.Elements.Count)!=genome.Modules.Sum(x=>x.Cnt))
        {
            return genome.Modules;
        }
        else
        {
            return mutatationModule;
        }

    }

    public bool CheckEquality(Genome genome1, Genome genome2)
    {
        var list1 = GetElementsFromModule(genome1.Modules);
        var list2 = GetElementsFromModule(genome2.Modules);
        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i].Number!=list2[i].Number)
            {
                return false;
            }
        }

        return true;
    }
    public List<Element> leftShufle(List<Element> elements)
    {
        /*Random rand = new();
        for (int i = elements.Count - 1; i >= 1; i--)
        {
            int j = rand.Next(i + 1);
            (elements[j], elements[i]) = (elements[i], elements[j]);
        }*/
        var newElements = elements.GetRange(1, elements.Count-1);
        newElements.Add(elements[0]);
        return newElements;
    }

    private bool ExistInModule(List<Module> modules,Element element)
    {
        foreach (var module in modules)
        {
            if (module.Elements.Select(x=>x.Number).ToList().Contains(element.Number))
            {
                return true;
            }
        }

        return false;
    }
    private List<Element> GetElementsFromModule(List<Module> modules)
    {
        List<Element> list = new();
        foreach (var module in modules)
        {
            foreach (var element in module.Elements)
            {
                list.Add(element);
            }
        }

        return list;
    }
    public decimal DetermineFitnes(List<Module> modules)
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