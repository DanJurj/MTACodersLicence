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
                LanguageCode = 5,
                Name = "C",
                EditorMode = "c_cpp",
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
                LanguageCode = 10,
                Name = "C++",
                EditorMode = "c_cpp",
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
                LanguageCode = 27,
                Name = "Java",
                EditorMode = "java",
                CodeTemplate = "public class Main {\n" +
                               "    public static void main(String[] args) {\n" +
                               "        System.out.println(\"Hello, World\");\n" +
                               "    }\n" +
                               "}"
            });
            context.ProgrammingLanguages.Add(new ProgrammingLanguageModel()
            {
                Available = true,
                LanguageCode = 35,
                Name = "Python",
                EditorMode = "python",
                CodeTemplate = "print(\"Hello World!\")"
            });
            await context.SaveChangesAsync();
        }
    }
}
