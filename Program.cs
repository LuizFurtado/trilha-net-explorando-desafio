using System.Text;
using Newtonsoft.Json;
using DesafioProjetoHospedagem.Models;

Console.OutputEncoding = Encoding.UTF8;

// Utiliza arquivo seed para popular a lista de suítes
var reservas = new List<Reserva>();
string suitesJson = File.ReadAllText("Seed/suites.json");
var suites = JsonConvert.DeserializeObject<List<Suite>>(suitesJson);

// Menu de opções para navegação
while (true)
{
    Console.WriteLine("Selecione uma opção:");
    Console.WriteLine("1 - Verificar suítes disponíveis");
    Console.WriteLine("2 - Criar reserva");
    Console.WriteLine("3 - Listar reservas");
    Console.WriteLine("4 - Sair");
    Console.WriteLine();

    string opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            VerificarSuitesDisponíveis(suites);
            break;
        case "2":
            CriarReserva(suites);
            break;
        case "3":
            ListarReservas();
            break;
        case "4":
            return;
        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}

void CriarReserva(List<Suite> suites)
{
    Console.WriteLine("Por favor informe o número de hóspedes:");
    int quantidadeHospedes = int.Parse(Console.ReadLine());

    bool suiteDisponivel = suites.Any(s => s.Disponivel && s.Capacidade >= quantidadeHospedes);

    if(!suiteDisponivel)
    {
        Console.WriteLine("Nenhuma suíte disponível para a quantidade de hóspedes informada. Tente novamente.");
        return;
    }

    var suitesCapacidade = suites.Where(s => s.Capacidade >= quantidadeHospedes).ToList();
    var hospedes = new List<Pessoa>();

    Console.WriteLine("Essas são as suítes disponíveis para a quantidade de hóspedes informada:");
    suitesCapacidade.ForEach(suite => Console.WriteLine($"{suite.Numero} - {suite.TipoSuite} - Capacidade: {suite.Capacidade}"));

    Console.WriteLine("Por favor informe o número da suíte:");
    int suiteEscolhida = int.Parse(Console.ReadLine());

    Console.WriteLine("Por favor informe a quantidade de dias:");
    int diasReservados = int.Parse(Console.ReadLine());

    for(int i = 0; i < quantidadeHospedes; i++)
    {
        Console.WriteLine("Por favor informe o nome do hóspede:");
        string nomeHospede = Console.ReadLine();
        hospedes.Add(new Pessoa(nomeHospede));
    }

    var suiteReserva = suitesCapacidade.First(s => s.Numero == suiteEscolhida);
    suites.First(s => s.Numero == suiteEscolhida).Disponivel = false;

    var reserva = new Reserva(diasReservados);
    reserva.CadastrarSuite(suiteReserva);
    reserva.CadastrarHospedes(hospedes);
    reservas.Add(reserva);

    Console.WriteLine("Reserva efetuada com sucesso! Confira abaixo os dados da sua reserva:");
    Console.WriteLine(reserva);
    Console.WriteLine();
}

void ListarReservas()
{
    if(reservas.Count == 0)
    {
        Console.WriteLine("Nenhuma reserva encontrada.");
        Console.WriteLine();
        return;
    }

    Console.WriteLine("Reservas:");
    reservas.ForEach(reserva => Console.WriteLine(reserva));
    Console.WriteLine();
}

void VerificarSuitesDisponíveis(List<Suite> suites)
{
    var disponiveis = suites.Where(s => s.Disponivel).ToList();
    Console.WriteLine("Suites disponíveis:");

    disponiveis.ForEach(suite => Console.WriteLine($"{suite.Numero} - {suite.TipoSuite} - Capacidade: {suite.Capacidade}"));
    Console.WriteLine();
}