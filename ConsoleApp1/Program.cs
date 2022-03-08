using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                if (string.IsNullOrEmpty(args[0]))
                {
                    Console.WriteLine("Não foi passado o parâmetro para localizar o texto: {0}", args[0]);

                    return;
                }

                string strPathFileText = string.Empty;

                if (!File.Exists(args[0]))
                {
                    Console.WriteLine("Arquivo não exite no caminho: {0}", args[0]);

                    return;
                }

                strPathFileText = args[0];

                if (string.IsNullOrEmpty(args[1]))
                {
                    Console.WriteLine("Não foi passado o parâmetro n-Gram: {0}", args[1]);

                    return;
                }

                if (!int.TryParse(args[1], out int intNGram))
                {
                    Console.WriteLine("O parâmetro n-Gram não foi reconhecido como um número inteiro: {0}", args[1]);

                    return;
                }

                if (intNGram <= 0)
                {
                    Console.WriteLine("O parâmetro n-Gram não pode ser de valor menor ou igual a zero: {0}", intNGram);

                    return;
                }

                using (StreamReader sr = new StreamReader(strPathFileText))
                {
                    string strLine = sr.ReadToEnd();

                    if (string.IsNullOrEmpty(strLine))
                    {
                        Console.WriteLine("O conteúdo do arquivo está vazio.");

                        return;
                    }

                    Console.WriteLine(strLine);

                    Console.WriteLine();

                    List<string> listPalavras = strLine.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                                       .Select((x, y) => new { x, y })
                                                       .GroupBy(g => g.y / intNGram)
                                                       .Select(s => string.Join(" ", s.Select(p => p.x)))
                                                       .ToList();

                    foreach (var line in listPalavras.ConvertAll(c => c.ToLower())
                                                     .GroupBy(g => g)
                                                     .Select(s => new { Quantidade = s.Count(), Letra = s.Key })
                                                     .OrderByDescending(o => o.Quantidade))
                    {
                        Console.WriteLine("{0} - {1}", line.Quantidade, line.Letra);
                    }

                    Console.ReadKey();
                }
            }
        }
    }
}
