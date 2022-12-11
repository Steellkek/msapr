using msapr.repository;

namespace msapr // Note: actual namespace depends on the project name.
{
    internal static class Program
    {
        static void Main()
        {
            new GenAlgRepo().Go();

        }
    }
}