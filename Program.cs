using System;
using System.Collections.Generic;
using System.Linq;

namespace projeto_gw
{
    
    public class Paciente
    {
        public string NomeCompleto { get; set; }
        public string NomeMae { get; set; }
        public DateTime DataNascimento { get; set; }
        public char Sexo { get; set; }
        public string CPF { get; set; }
    }

    public class Atendimento
    {
        public Paciente Paciente { get; set; }
        public DateTime DataAtendimento { get; set; }
        public string Procedimento { get; set; }
        public TimeSpan Duracao { get; set; }
        public string Codigo { get; set; }
    }

    public class Program
    {
        static List<Paciente> Pacientes = new List<Paciente>();
        static List<Atendimento> Atendimentos = new List<Atendimento>();

        public static void Main()
        {
            InserirDadosTestes();
            bool sair = false;
            while (!sair)
            {
                Console.WriteLine("Menu do ERP simples da Clinica CDI");
                Console.WriteLine("1. Cadastrar paciente");
                Console.WriteLine("2. Realizar atendimento");
                Console.WriteLine("3. Listar pacientes");
                Console.WriteLine("4. Listar atendimentos");
                Console.WriteLine("5. Total de procedimento");
                Console.WriteLine("6. Tempo em procedimento(s)");
                Console.WriteLine("7. Apagar todos os dados");
                Console.WriteLine("0. Para finalizar o programa!");
                Console.WriteLine("------------------------------------");

                int opcao = Convert.ToInt32(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        CadastrarPaciente();
                        break;
                    case 2:
                        RealizarAtendimento();
                        break;
                    case 3:
                        ListarPacientes();
                        break;
                    case 4:
                        ListarAtendimentos();
                        break;
                    case 5:
                        NumeroTotalDeCadaProcedimento();
                        break;
                    case 6:
                        TempoTotalProcedimento();
                        break;
                    case 7:
                        ApagarDados();
                        break;
                    case 0:
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }

        static void CadastrarPaciente()
        {
            Paciente novoPaciente = new Paciente();

            Console.WriteLine("Digite o nome completo do paciente:");
            novoPaciente.NomeCompleto = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(novoPaciente.NomeCompleto))
            {
                Console.WriteLine("É necessário registrar o nome completo do paciente.");
                return;
            }

            Console.WriteLine("Digite o nome da mãe do paciente:");
            novoPaciente.NomeMae = Console.ReadLine();

            Console.WriteLine("Digite a data de nascimento do paciente (dd/MM/yyyy):");
            string dataNascimento = Console.ReadLine();
            if (!DateTime.TryParseExact(dataNascimento, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataNasc))
            {
                Console.WriteLine("É necessário registrar a data de nascimento do paciente no formato dd/MM/yyyy.");
                return;
            }
            if ((DateTime.Now.Year - dataNasc.Year) < 13)
            {
                Console.WriteLine("O paciente tem menos de 13 anos e não pode ser registrado. Registre uma idade válida maior de 12 anos.");
                return;
            }
            novoPaciente.DataNascimento = dataNasc;

            Console.WriteLine("Digite o sexo do paciente (M ou F):");
            char sexo = Convert.ToChar(Console.ReadLine().ToUpper());
            if (sexo != 'M' && sexo != 'F')
            {
                Console.WriteLine("É necessário registrar o sexo do paciente como M ou F.");
                return;
            }
            novoPaciente.Sexo = sexo;

            Console.WriteLine("Digite o CPF de 11 numeros do paciente:");
            string cpf = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11 || !long.TryParse(cpf, out _) || Pacientes.Exists(p => p.CPF == cpf))
            {
                Console.WriteLine("É necessário registrar o CPF do paciente com somente números e que não esteja duplicado.");
                return;
            }
            novoPaciente.CPF = cpf;

            Pacientes.Add(novoPaciente);
            Console.WriteLine("Paciente cadastrado com sucesso!");
        }


        static void RealizarAtendimento()
        {

                Console.WriteLine("Digite o nome completo, a data de nascimento (dd/MM/yyyy) ou o CPF(11 numeros) do paciente :");
                string busca = Console.ReadLine();

                Paciente pacienteBuscado = Pacientes.Find(p => p.NomeCompleto == busca || p.DataNascimento.ToString("dd/MM/yyyy") == busca || p.CPF == busca);
                if (pacienteBuscado == null)
                {                    
                
                Console.WriteLine("Paciente não encontrado. Gostaria de cadastrar um novo paciente? (Sim ou Não)");
                string resposta1 = Console.ReadLine().ToUpper();
                if (resposta1 == "SIM")
                {
                    CadastrarPaciente();
                    return;
                }
                else
                {
                    return;
                }
                }

                Console.WriteLine("Registrar atendimento para " + pacienteBuscado.NomeCompleto + "? (Sim ou Não)");
                string resposta2 = Console.ReadLine().ToUpper();
                if (resposta2 != "SIM")
                {
                    return;
                }

                Atendimento novoAtendimento = new Atendimento();
                novoAtendimento.Paciente = pacienteBuscado;

                Console.WriteLine("Digite a data do atendimento (dd/MM/yyyy):");
                string dataAtendimento = Console.ReadLine();
                if (!DateTime.TryParseExact(dataAtendimento, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataAtend))
                {
                    Console.WriteLine("É necessário registrar a data do atendimento no formato dd/MM/yyyy.");
                    return;
                }
                novoAtendimento.DataAtendimento = dataAtend;

                Console.WriteLine("Escolha o procedimento:");
                Console.WriteLine("1 - Raios – X de Tórax em PA");
                Console.WriteLine("2 - Ultrassonografia Obstétrica");
                Console.WriteLine("3 - Ultrassonografia de Próstata");
                Console.WriteLine("4 - Tomografia");
                int procedimento = Convert.ToInt32(Console.ReadLine());
                switch (procedimento)
                {
                    case 1:
                        novoAtendimento.Procedimento = "Raios – X de Tórax em PA";
                        break;
                    case 2:
                        if (pacienteBuscado.Sexo != 'F' || (DateTime.Now.Year - pacienteBuscado.DataNascimento.Year) >= 60)
                        {
                            Console.WriteLine("O procedimento Ultrassonografia Obstétrica só pode ser realizado por pacientes do sexo F com idade menor que 60 anos.");
                            return;
                        }
                        novoAtendimento.Procedimento = "Ultrassonografia Obstétrica";
                        break;
                    case 3:
                        if (pacienteBuscado.Sexo != 'M')
                        {
                            Console.WriteLine("O procedimento Ultrassonografia de Próstata só pode ser realizado por pacientes do sexo M.");
                            return;
                        }
                        novoAtendimento.Procedimento = "Ultrassonografia de Próstata";
                        break;
                    case 4:
                        Atendimento ultimoAtendimento = Atendimentos.FindLast(a => a.Paciente == pacienteBuscado && (a.Procedimento == "Ultrassonografia Obstétrica" || a.Procedimento == "Ultrassonografia de Próstata"));
                        if (ultimoAtendimento != null && (DateTime.Now - ultimoAtendimento.DataAtendimento).TotalDays <= 90)
                        {
                            Console.WriteLine("O procedimento Tomografia só pode ser realizado em pacientes que não realizaram Ultrassonografia Obstétrica ou Ultrassonografia de Próstata nos últimos três meses.");
                            return;
                        }
                        novoAtendimento.Procedimento = "Tomografia";
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        return;
                }

                Console.WriteLine("Digite o tempo de duração do procedimento (em minutos):");
                novoAtendimento.Duracao = TimeSpan.FromMinutes(Convert.ToDouble(Console.ReadLine()));

                Console.WriteLine("Digite um código com 10 caracteres:");
                string codigo = Console.ReadLine();
                if (codigo.Length != 10)
                {
                    Console.WriteLine("O código deve ter 10 caracteres.");
                    return;
                }
                novoAtendimento.Codigo = codigo;

                Atendimentos.Add(novoAtendimento);
                Console.WriteLine("Atendimento registrado com sucesso!");
            

        }

        static void ListarPacientes()
        {
            if (Pacientes.Count == 0)
            {
                Console.WriteLine("Não há pacientes cadastrados.");
                return;
            }

            Console.WriteLine("Lista de Pacientes:");
            foreach (Paciente paciente in Pacientes)
            {
                Console.WriteLine("Nome do Paciente: " + paciente.NomeCompleto);
                Console.WriteLine("Data de Nascimento: " + paciente.DataNascimento.ToString("dd/MM/yyyy"));
                Console.WriteLine("-------------------------");
            }
        }



        static void ListarAtendimentos()
        {
            if (Atendimentos.Count == 0)
            {
                Console.WriteLine("Não há atendimentos registrados.");
                return;
            }

            Console.WriteLine("Datas de Atendimentos:");
            List<DateTime> datasAtendimentos = new List<DateTime>();
            foreach (Atendimento atendimento in Atendimentos)
            {
                if (!datasAtendimentos.Contains(atendimento.DataAtendimento))
                {
                    datasAtendimentos.Add(atendimento.DataAtendimento);
                    Console.WriteLine(atendimento.DataAtendimento.ToString("dd/MM/yyyy"));
                }
            }

            Console.WriteLine("Digite a data do atendimento que você deseja exibir (dd/MM/yyyy):");
            string dataEscolhida = Console.ReadLine();
            if (!DateTime.TryParseExact(dataEscolhida, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataAtend))
            {
                Console.WriteLine("Data inválida. Por favor, insira uma data no formato dd/MM/yyyy.");
                return;
            }

            List<Atendimento> atendimentosDataEscolhida = Atendimentos.FindAll(a => a.DataAtendimento.Date == dataAtend.Date);
            if (atendimentosDataEscolhida.Count == 0)
            {
                Console.WriteLine("Não há atendimentos registrados para a data escolhida.");
                return;
            }

            Console.WriteLine("Atendimentos para a data " + dataAtend.ToString("dd/MM/yyyy") + ":");
            foreach (Atendimento atendimento in atendimentosDataEscolhida)
            {
                Console.WriteLine("Nome do Paciente: " + atendimento.Paciente.NomeCompleto);
                Console.WriteLine("Procedimento: " + atendimento.Procedimento);
                Console.WriteLine("Duração: " + atendimento.Duracao.TotalMinutes + " minutos");
                Console.WriteLine("Código: " + atendimento.Codigo);
                Console.WriteLine("-------------------------");
            }
        }

        static void NumeroTotalDeCadaProcedimento()
        {
            if (Atendimentos.Count == 0)
            {
                Console.WriteLine("Não há atendimentos registrados.");
                return;
            }

            Console.WriteLine("Escolha um procedimento:");
            Console.WriteLine("1 - Raios – X de Tórax em PA");
            Console.WriteLine("2 - Ultrassonografia Obstétrica");
            Console.WriteLine("3 - Ultrassonografia de Próstata");
            Console.WriteLine("4 - Tomografia");
            Console.WriteLine("0 - Voltar ao menu");

            int opcao = Convert.ToInt32(Console.ReadLine());
            string procedimento;
            switch (opcao)
            {
                case 1:
                    procedimento = "Raios – X de Tórax em PA";
                    break;
                case 2:
                    procedimento = "Ultrassonografia Obstétrica";
                    break;
                case 3:
                    procedimento = "Ultrassonografia de Próstata";
                    break;
                case 4:
                    procedimento = "Tomografia";
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Opção inválida.");
                    return;
            }
            
            Console.WriteLine("Digite a data inicial do período (dd/MM/yyyy):");
            string dataInicial = Console.ReadLine();
            if (!DateTime.TryParseExact(dataInicial, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataIni))
            {
                Console.WriteLine("Data inicial inválida. Por favor, insira uma data no formato dd/MM/yyyy.");
                return;
            }

            Console.WriteLine("Digite a data final do período (dd/MM/yyyy):");
            string dataFinal = Console.ReadLine();
            if (!DateTime.TryParseExact(dataFinal, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataFim))
            {
                Console.WriteLine("Data final inválida. Por favor, insira uma data no formato dd/MM/yyyy.");
                return;
            }

            List<Atendimento> atendimentosPeriodo = Atendimentos.FindAll(a => a.Procedimento == procedimento && a.DataAtendimento.Date >= dataIni.Date && a.DataAtendimento.Date <= dataFim.Date);
            if (atendimentosPeriodo.Count == 0)
            {
                Console.WriteLine("Não há atendimentos registrados para o procedimento e período escolhidos.");
                return;
            }

            Console.WriteLine("Número total do procedimento '" + procedimento + "' no período de " + dataIni.ToString("dd/MM/yyyy") + " até " + dataFim.ToString("dd/MM/yyyy") + ": " + atendimentosPeriodo.Count);
            Console.WriteLine("Datas dos atendimentos:");
            foreach (Atendimento atendimento in atendimentosPeriodo)
            {
                Console.WriteLine(atendimento.DataAtendimento.ToString("dd/MM/yyyy"));
            }
        }

        static void TempoTotalProcedimento()
        {
            if (Atendimentos.Count == 0)
            {
                Console.WriteLine("Não há atendimentos registrados.");
                return;
            }

            Console.WriteLine("Escolha um procedimento:");
            Console.WriteLine("1 - Raios – X de Tórax em PA");
            Console.WriteLine("2 - Ultrassonografia Obstétrica");
            Console.WriteLine("3 - Ultrassonografia de Próstata");
            Console.WriteLine("4 - Tomografia");
            Console.WriteLine("5 - Todos os procedimentos");
            Console.WriteLine("0 - Voltar ao menu");

            int opcao = Convert.ToInt32(Console.ReadLine());
            string procedimento;
            switch (opcao)
            {
                case 1:
                    procedimento = "Raios – X de Tórax em PA";
                    break;
                case 2:
                    procedimento = "Ultrassonografia Obstétrica";
                    break;
                case 3:
                    procedimento = "Ultrassonografia de Próstata";
                    break;
                case 4:
                    procedimento = "Tomografia";
                    break;
                case 5:
                    procedimento = "Todos";
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Opção inválida.");
                    return;
            }

            Console.WriteLine("Digite a data inicial do período (dd/MM/yyyy):");
            string dataInicial = Console.ReadLine();
            if (!DateTime.TryParseExact(dataInicial, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataIni))
            {
                Console.WriteLine("Data inicial inválida. Por favor, insira uma data no formato dd/MM/yyyy.");
                return;
            }

            Console.WriteLine("Digite a data final do período (dd/MM/yyyy):");
            string dataFinal = Console.ReadLine();
            if (!DateTime.TryParseExact(dataFinal, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataFim))
            {
                Console.WriteLine("Data final inválida. Por favor, insira uma data no formato dd/MM/yyyy.");
                return;
            }

            List<Atendimento> atendimentosProcedimento;
            if (procedimento == "Todos")
            {
                atendimentosProcedimento = Atendimentos;
            }
            else
            {
                atendimentosProcedimento = Atendimentos.FindAll(a => a.Procedimento == procedimento);
            }

            atendimentosProcedimento = atendimentosProcedimento.FindAll(a => a.DataAtendimento.Date >= dataIni.Date && a.DataAtendimento.Date <= dataFim.Date && a.Duracao.TotalMinutes >= 1);
            if (atendimentosProcedimento.Count == 0)
            {
                Console.WriteLine("Não há tempo registrado para o procedimento escolhido nesse período.");
                return;
            }

            TimeSpan tempoTotal = new TimeSpan(atendimentosProcedimento.Sum(a => a.Duracao.Ticks));
            Console.WriteLine("O tempo total do procedimento '" + procedimento + "' no período de " + dataIni.ToString("dd/MM/yyyy") + " até " + dataFim.ToString("dd/MM/yyyy") + " é de " + tempoTotal.TotalMinutes + " minutos.");

            Console.WriteLine("Datas dos atendimentos com pelo menos 1 minuto:");
            List<DateTime> datasAtendimentos = atendimentosProcedimento.Select(a => a.DataAtendimento.Date).Distinct().OrderBy(d => d).ToList();
            foreach (DateTime data in datasAtendimentos)
            {
                Console.WriteLine(data.ToString("dd/MM/yyyy"));
            }
        }

        static void InserirDadosTestes()      
        {
            Paciente paciente1 = new Paciente
            {
                NomeCompleto = "João Silva",
                NomeMae = "Maria Silva",
                DataNascimento = new DateTime(1980, 1, 1),
                Sexo = 'M',
                CPF = "12345678901"
            };
            Pacientes.Add(paciente1);

            Paciente paciente2 = new Paciente
            {
                NomeCompleto = "Ana Souza",
                NomeMae = "Carla Souza",
                DataNascimento = new DateTime(1990, 2, 2),
                Sexo = 'F',
                CPF = "23456789012"
            };
            Pacientes.Add(paciente2);
                        
            Atendimento atendimento1 = new Atendimento
            {
                Paciente = paciente1,
                DataAtendimento = new DateTime(2023, 1, 1),
                Procedimento = "Raios – X de Tórax em PA",
                Duracao = TimeSpan.FromMinutes(30),
                Codigo = "ATEND12345"
            };
            Atendimentos.Add(atendimento1);

            Atendimento atendimento2 = new Atendimento
            {
                Paciente = paciente1,
                DataAtendimento = new DateTime(2023, 2, 2),
                Procedimento = "Ultrassonografia de Próstata",
                Duracao = TimeSpan.FromMinutes(45),
                Codigo = "ATEND12346"
            };
            Atendimentos.Add(atendimento2);

            Atendimento atendimento3 = new Atendimento
            {
                Paciente = paciente2,
                DataAtendimento = new DateTime(2023, 3, 3),
                Procedimento = "Ultrassonografia Obstétrica",
                Duracao = TimeSpan.FromMinutes(60),
                Codigo = "ATEND67890"
            };
            Atendimentos.Add(atendimento3);

            Atendimento atendimento4 = new Atendimento
            {
                Paciente = paciente2,
                DataAtendimento = new DateTime(2023, 4, 4),
                Procedimento = "Tomografia",
                Duracao = TimeSpan.FromMinutes(90),
                Codigo = "ATEND67891"
            };
            Atendimentos.Add(atendimento4);
        }

        static void ApagarDados()
        {            
            Atendimentos.Clear();
                        
            Pacientes.Clear();
        }

    }

}
