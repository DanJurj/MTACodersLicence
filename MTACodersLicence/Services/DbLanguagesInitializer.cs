using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTACodersLicence.Data;
using MTACodersLicence.Models;

namespace MTACodersLicence.Services
{
    public class DbLanguagesInitializer
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            // Verificam daca a fost initializata baza de date cu Limbajele disponibile
            if (context.ProgrammingLanguages.Any())
            {
                return;
            }
            // Daca nu a fost initializata o initializam cu limbajele urmatoare
            context.ProgrammingLanguages.Add(new ProgrammingLanguageModel()
            {
                Available = true,
                Filename = "source.c",
                Name = "C",
                Type = "c",
                CodeTemplate = "#include<stdio.h>\n" +
                               "int main(void)\n" +
                               "{\n" +
                               "    printf(\"Hello World!\"); \n" +
                               "    return 0; \n" +
                               "}"
            });
            context.ProgrammingLanguages.Add(new ProgrammingLanguageModel()
            {
                Available = true,
                Filename = "source.cpp",
                Name = "C++",
                Type = "cpp",
                CodeTemplate = "#include<iostream>\n" +
                               "using namespace std;\n" +
                               "int main(void)\n" +
                               "{\n" +
                               "    cout<<\"Hello World!\"; \n" +
                               "    return 0; \n" +
                               "}"
            });
            context.ProgrammingLanguages.Add(new ProgrammingLanguageModel()
            {
                Available = true,
                Filename = "Solution.java",
                Name = "Java",
                Type = "java",
                CodeTemplate = "public class Solution {\n" +
                               "    public static void main(String[] args) {\n" +
                               "        System.out.println(\"Hello, World\");\n" +
                               "    }\n" +
                               "}"
            });
            context.ProgrammingLanguages.Add(new ProgrammingLanguageModel()
            {
                Available = true,
                Filename = "source.py",
                Name = "Python",
                Type = "python",
                CodeTemplate = "print(\"Hello World!\")"
            });
            await context.SaveChangesAsync();
        }
    }
}
