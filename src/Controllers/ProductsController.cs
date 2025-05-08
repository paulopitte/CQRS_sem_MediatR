namespace CQRS_sem_MediatR.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    //    Na ASP.NET Core, ao usar [FromBody], o framework já:
    //    Desserializa o JSON do corpo da requisição
    //    Cria uma instância do CreateProductCommand
    //    Preenche todas as propriedades
    //    Valida o objeto(se tiver validação)
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        //   Recebe um objeto de comando/query (ex: CreateProductCommand)
        //   Localiza o handler apropriado dinamicamente usando o IServiceProvider
        //   Executa o handler correspondente ao tipo do comando / query
        //   Assim :
        //     Detecta que command é do tipo CreateProductCommand
        //     Busca um ICommandHandler<CreateProductCommand>
        //     Encontra CreateProductCommandHandler(que você registrou no DI)
        //     Executa o método Handle desse handler
        await _dispatcher.Dispatch(command);

        return CreatedAtAction(nameof(GetById), new { id = command.Id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest("O ID da rota não confere com o ID do corpo da requisição.");

        await _dispatcher.Dispatch(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteProductCommand(id);
        await _dispatcher.Dispatch(command);
        return NoContent();
    }

    // Os requests GET não têm corpo (em APIs REST convencionais)
    // Todos os parâmetros vêm via: 1- Route parameters({ id})
    // 2- Query string (? category=1&page=2)
    // A ASP.NET Core não desserializa automaticamente para objetos complexos em GET:
    // Por design REST e Por limitações técnicas (URLs têm tamanho máximo)
    // As Consultas usam parâmetros simples via URL (que você monta manualmente)
    // Assim temos que construir manualmente o objeto de consulta
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _dispatcher.Dispatch<GetProductByIdQuery, Product?>(query);
        return result is not null ? Ok(result) : NotFound();
    }

    // O que significa "category/{categoryId}"?
    // Parte estática("category/")
    //   Define um segmento fixo na URL
    //   Exemplo: https://api.com/products/category/5
    // Parte variável({ categoryId})
    //   Define um parâmetro de rota(route parameter)
    //   O valor será capturado e passado para o parâmetro int categoryId do método
    //   GET /products/category/5 é mais descritivo que GET /products?categoryId=5
    // URLs com parâmetros de rota são mais facilmente cacheadas por CDNs e proxies
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var query = new GetProductsByCategoryQuery(categoryId);
        var result = await _dispatcher.Dispatch<GetProductsByCategoryQuery, List<Product>>(query);
        return Ok(result);
    }

    //  Padrão Command-Query Responsibility Segregation, onde:
    //  Toda operação é representada por um objeto(command ou query)
    //  Mesmo consultas vazias são objetos explícitos
    //  Uniformidade no tratamento de todas as requisições
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllProductsQuery();
        var result = await _dispatcher.Dispatch<GetAllProductsQuery, List<Product>>(query);
        return Ok(result);
    }
}