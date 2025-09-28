using SerenattoEnsaio.Dados;
using SerenattoEnsaio.Modelos;
using SerenattoPreGravacao.Dados;

IEnumerable<Cliente> clientes = DadosClientes.GetClientes().ToList();

foreach (var item in clientes)
{
    Console.WriteLine($"Id: {item.Id}, Nome: {item.Nome}, Pedidos: {item.Pedidos.Count}, " +
        $"Telefone: {item.Telefone}, Endereço: {item.Endereco}");
}

Console.WriteLine("\n===========Formas de pagamento===========");
var formaDePagamento = DadosFormaDePagamento.FormasDePagamento.Where(x => x.StartsWith('d'));
Console.WriteLine(string.Join(" ", formaDePagamento));

Console.WriteLine("\n===========Cliente por endereco===========");
var clienteComEndereco = clientes.Select(c => new
{
    NomeCliente = c.Nome,
    EnderecoCliente = c.Endereco
}).OrderBy(c => c.NomeCliente).ThenBy(e => e.EnderecoCliente);

foreach (var item in clienteComEndereco)
{
    Console.WriteLine($"Nome: {item.NomeCliente}, Endereço: {item.EnderecoCliente}");
}

Console.WriteLine("\n===========Clientes e telefone===========");
var clientesPorTelefone = clientes.Select(c => new
{
    NomeCliente = c.Nome,
    TelefoneCliente = c.Telefone
});

foreach (var item in clientesPorTelefone)
{
    Console.WriteLine($"Nome: {item.NomeCliente}, Telefone: {item.TelefoneCliente}");
}

Console.WriteLine("\n===========Clientes agrupados por código de telefone===========");
//var clientesAgrupadosPorTelefone = clientesPorTelefone.GroupBy(c => new string (c.TelefoneCliente.Skip(1).Take(2).ToArray()));
var clientesAgrupadosPorTelefone = clientesPorTelefone.OrderBy(c => c.TelefoneCliente).GroupBy(c => c.TelefoneCliente.Substring(1, 2));
foreach (var grupo in clientesAgrupadosPorTelefone)
{
    Console.WriteLine($"\nCódigo de telefone: {grupo.Key} | Número de Clientes {grupo.Count()}");
    foreach (var cliente in grupo)
    {
        Console.WriteLine($"Nome: {cliente.NomeCliente}, Telefone: {cliente.TelefoneCliente}");
    }
}


Console.WriteLine("\n===========Cliente por letra===========");
Console.WriteLine("Digite uma letra: ");
char letra = Console.ReadLine()[0];
var clienteComLetra = clientes.Where(c => c.Nome.StartsWith(letra.ToString(), StringComparison.OrdinalIgnoreCase))
    .Select(x => x.Nome);
foreach (var item in clienteComLetra)
{
    Console.WriteLine(item);
}

Console.WriteLine("\n===========Dados dos produtos do Cardápio da Loja===========");
IEnumerable<Produto> cardapioLoja = DadosCardapio.GetProdutos();
IEnumerable<Produto> cardapioDelivery = DadosCardapio.CardapioDelivery();
foreach (var item in cardapioLoja)
{
    Console.WriteLine($"Id: {item.Id}, Nome: {item.Nome}, Preço: {item.Preco:C}");
}

Console.WriteLine("\n===========Nomes dos produtos disponíveis SOMENTE do Cardápio da Loja===========");
IEnumerable<string> nomesProdutosLoja = cardapioLoja.Select(p => p.Nome);
IEnumerable<string> nomesProdutosDelivery = cardapioDelivery.Select(p => p.Nome);

var produtosSoVendidosNaLoja = nomesProdutosLoja.Except(nomesProdutosDelivery).ToList();
foreach (var item in produtosSoVendidosNaLoja)
{
    Console.WriteLine(item);
}


Console.WriteLine("\n===========Nomes dos produtos disponíveis no Cardápio da Loja e do Delivery ao mesmo tempo===========");
var produtosNas2Listas = nomesProdutosLoja.Intersect(nomesProdutosDelivery).ToList();
foreach (var item in produtosNas2Listas)
{
    Console.WriteLine(item);
}

Console.WriteLine("\n===========Nomes dos produtos disponíveis em ambos Cardápios, Loja e Delivery===========");
var produtosEmTodos = nomesProdutosLoja.Union(nomesProdutosDelivery).ToList();
foreach (var item in produtosEmTodos)
{
    Console.WriteLine(item);
}


Console.WriteLine("\n===========Produtos da loja por nome===========");

var produtosPorNome = cardapioLoja.Select(p => p.Nome).OrderBy(n => n);
foreach (var item in produtosPorNome)
{
    Console.WriteLine(item);
}

Console.WriteLine("\n===========Produtos por preço===========");
var produtosPorPreco = cardapioLoja.Select(p => new
{
    NomeProduto = p.Nome,
    PrecoProduto = p.Preco
});

foreach (var item in produtosPorPreco)
{
    Console.WriteLine($"{item.NomeProduto} = R$ {item.PrecoProduto:F2}");
}

Console.WriteLine("\n===========Produtos por preço do combo===========");
var produtosPrecoCombo = cardapioLoja.Select(p => new
{
    NomeProduto = p.Nome,
    PrecoCombo = p.Preco * 3
});

Console.WriteLine("\n-------------Combo LEVE 4 pague 3-------------");
foreach (var item in produtosPrecoCombo)
{
    Console.WriteLine($"{item.NomeProduto} = R$ {item.PrecoCombo:F2}");
}

Console.WriteLine("\n===========Nomes dos produtos ordenados por nome e preço===========");
var cardapioOrdenadoNomePreco = cardapioLoja.OrderBy(p => p.Nome).ThenBy(p => p.Preco);
foreach (var item in cardapioOrdenadoNomePreco)
{
    Console.WriteLine($"Nome: {item.Nome}, Preço: {item.Preco:C}");
}

Console.WriteLine("\n===========Quantidade de pedidos no mês===========");
IEnumerable<int> totalPedidosMes = DadosPedidos.QuantidadeItensPedidosPorDia.SelectMany(lista => lista);

foreach (var pedido in totalPedidosMes)
{
    Console.Write( $"{pedido} ");
}

Console.WriteLine();
Console.WriteLine("\n===========Quantidade de pedidos no mês com um únco item===========");
var pedidosIndiduais = totalPedidosMes.Count(numero => numero == 1);
Console.WriteLine("Total de pedidos individuais foi: " + pedidosIndiduais);

Console.WriteLine("\n===========Pedidos com Quantidade diferentes itens===========");
IEnumerable<int> pedidosDiferents = totalPedidosMes.Distinct();
foreach (var item in pedidosDiferents)
{
    Console.Write( $"{item} ");
}

Console.WriteLine();
Console.WriteLine("\n===========Produtos no Carrinho de compras===========");
IEnumerable<Produto> carrinhoCompras = DadosCarrinho.GetProdutosCarrinho();
foreach (var item in carrinhoCompras.ToList().OrderBy(p => p.Nome))
{
    Console.WriteLine($"Id: {item.Id}, Nome: {item.Nome}, Preço: {item.Preco:C}");
}

Console.WriteLine("\n===========Nome dos Produtos no Carrinho de compras===========");
string nomeProdutosNoCarrinho = carrinhoCompras.Select(p => p.Nome).Aggregate((p1, p2) => p1 + ", " + p2);
Console.WriteLine(nomeProdutosNoCarrinho);

Console.WriteLine("\n===========Quantidade Produtos no Carrinho===========");
int quantidadeProdutosNoCarrinho = carrinhoCompras.Count();
Console.WriteLine($"Quantidade de protudos no carrinho {quantidadeProdutosNoCarrinho}");

Console.WriteLine("\n===========Quantidade Produtos no Carrinho===========");
var produtosAgrupadorPorNome = carrinhoCompras.Select(p => p.Nome).GroupBy(nome => nome);
foreach (var item in produtosAgrupadorPorNome)
{
    Console.WriteLine($"Produto: {item.Key}, Quantidade: {item.Count()}");
}

Console.WriteLine("\n===========Total dos Produtos no Carrinho===========");
//decimal totalCarrinho = carrinhoCompras.Select(p => p.Preco).Aggregate((p1, p2) => p1 + p2);
decimal totalCarrinho = carrinhoCompras.Select(p => p.Preco).Sum();
Console.WriteLine($"Total do carrinho: {totalCarrinho:C}");
