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

    public void Crossingover(Genome genome1,Genome genome2)
    {
        
        var x = 2;
        var y = 0;
        var list1 = GetElementsFromModule(genome1);
        var list2 = GetElementsFromModule(genome2);
        PrintNumber(list1);
        PrintNumber(list2);
        var child1 = genome1.Modules.GetRange(0,genome1.Modules.Count);
        var child2 = genome2.Modules.GetRange(0,genome2.Modules.Count);
        var list3 = new List<Element>();

        while (y != 1)
        {
            list3.Clear();
            var j = 0;
            foreach (var module in child1)
            {
                module.Elements.Clear();
            }

            foreach (var module in child1)
            {
                module.Elements.Clear();
                for (int i = j; i < list2.Count && module.Cnt > module.Elements.Count; i++)
                {
                    if (j <= x)
                    {
                        module.Elements.Add(list1[j]);
                        list3.Add(list1[j]);
                        j += 1;
                    }
                    else if ((list2[i].Squre + module.Elements.Sum(x => x.Squre) < module.Square) &&
                             (!ExistInModule(child1, list2[i])))
                    {
                        module.Elements.Add(list2[i]);
                        list3.Add(list2[i]);
                        list2.Remove(list2[i]);
                        i--;
                    }

                    if (module.Cnt == module.Elements.Count)
                    {
                        break;
                    }
                }
            }

            foreach (var module in child1)
            {
                if (module.Cnt > module.Elements.Count)
                {
                    for (int i = 0; i < list2.Count && module.Cnt > module.Elements.Count; i++)
                    {
                        if ((list2[i].Squre + module.Elements.Sum(x => x.Squre) < module.Square) &&
                            (!ExistInModule(child1, list2[i])))
                        {
                            module.Elements.Add(list2[i]);
                            list3.Add(list2[j]);
                            list2.Remove(list2[i]);
                            i--;
                        }
                    }
                }

                if (module.Cnt > module.Elements.Count)
                {
                    y = 0;
                    list1 = GetElementsFromModule(genome1);
                    list2 = leftShufle(GetElementsFromModule(genome2));
                    break;
                }
                else
                {
                    y = 1;
                }
            }
        }   
        PrintNumber(list3);
        
         list1 = GetElementsFromModule(genome1);
         list2 = GetElementsFromModule(genome2);
        y = 0;
        while (y != 1)
        {
            list3.Clear();
            var j = 0;
            foreach (var module in child2)
            {
                module.Elements.Clear();
            }

            foreach (var module in child2)
            {
                module.Elements.Clear();
                for (int i = j; i < list1.Count && module.Cnt > module.Elements.Count; i++)
                {
                    if (j <= x)
                    {
                        module.Elements.Add(list2[j]);
                        list3.Add(list2[j]);
                        j += 1;
                    }
                    else if ((list1[i].Squre + module.Elements.Sum(x => x.Squre) < module.Square) &&
                             (!ExistInModule(child2, list1[i])))
                    {
                        module.Elements.Add(list1[i]);
                        list3.Add(list1[i]);
                        list1.Remove(list1[i]);
                        i--;
                    }

                    if (module.Cnt == module.Elements.Count)
                    {
                        break;
                    }
                }
            }

            foreach (var module in child2)
            {
                if (module.Cnt > module.Elements.Count)
                {
                    for (int i = 0; i < list1.Count && module.Cnt > module.Elements.Count; i++)
                    {
                        if ((list1[i].Squre + module.Elements.Sum(x => x.Squre) < module.Square) &&
                            (!ExistInModule(child2, list1[i])))
                        {
                            module.Elements.Add(list1[i]);
                            list3.Add(list1[i]);
                            list1.Remove(list1[i]);
                            i--;
                        }
                    }
                }

                if (module.Cnt > module.Elements.Count)
                {
                    y = 0;
                    list2 = GetElementsFromModule(genome1);
                    list1 = leftShufle(GetElementsFromModule(genome2));
                    break;
                }
                else
                {
                    y = 1;
                }
            }
        }   
        PrintNumber(list3);

        var g = 5;
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
    private List<Element> GetElementsFromModule(Genome genome)
    {
        List<Element> list = new();
        foreach (var module in genome.Modules)
        {
            foreach (var element in module.Elements)
            {
                list.Add(element);
            }
        }

        return list;
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