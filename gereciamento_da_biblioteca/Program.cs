using System;
using System.Collections.Generic;
using System.Linq;

class Livro
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string Genero { get; set; }
    public int QuantidadeDisponivel { get; set; }

    public Livro(string titulo, string autor, string genero, int quantidade)
    {
        Titulo = titulo;
        Autor = autor;
        Genero = genero;
        QuantidadeDisponivel = quantidade;
    }
}

class Usuario
{
    public string Nome { get; set; }
    public List<Livro> LivrosEmprestados { get; private set; } = new List<Livro>();

    public Usuario(string nome)
    {
        Nome = nome;
    }

    public bool PodeEmprestar() => LivrosEmprestados.Count < 3;

    public void DevolverLivro(Livro livro)
    {
        LivrosEmprestados.Remove(livro);
    }

    public void EmprestarLivro(Livro livro)
    {
        LivrosEmprestados.Add(livro);
    }
}

class Biblioteca
{
    private List<Livro> livros = new List<Livro>();

    public void CadastrarLivro(Livro livro)
    {
        livros.Add(livro);
    }

    public List<Livro> ConsultarCatalogo()
    {
        return livros;
    }

    public bool EmprestarLivro(Livro livro, Usuario usuario)
    {
        if (livro.QuantidadeDisponivel > 0 && usuario.PodeEmprestar())
        {
            livro.QuantidadeDisponivel--;
            usuario.EmprestarLivro(livro);
            return true;
        }
        return false;
    }

    public void DevolverLivro(Livro livro, Usuario usuario)
    {
        livro.QuantidadeDisponivel++;
        usuario.DevolverLivro(livro);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Biblioteca biblioteca = new Biblioteca();
        List<Usuario> usuarios = new List<Usuario>();
        Usuario usuarioLogado = null;

        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Login como Administrador");
            Console.WriteLine("2. Login como Usuário");
            Console.WriteLine("3. Sair");
            string opcao = Console.ReadLine();

            if (opcao == "3") break;

            if (opcao == "1")
            {
                // Cadastro de livros
                CadastrarLivros(biblioteca);
            }
            else if (opcao == "2")
            {
                // Login do usuário
                usuarioLogado = LoginUsuario(usuarios);
                if (usuarioLogado != null)
                {
                    while (true)
                    {
                        Console.WriteLine("Menu do Usuário:");
                        Console.WriteLine("1. Emprestar Livro");
                        Console.WriteLine("2. Devolver Livro");
                        Console.WriteLine("3. Consultar Catálogo");
                        Console.WriteLine("4. Sair");
                        string usuarioOpcao = Console.ReadLine();

                        if (usuarioOpcao == "4") break;

                        switch (usuarioOpcao)
                        {
                            case "1":
                                EmprestarLivro(biblioteca, usuarioLogado);
                                break;
                            case "2":
                                DevolverLivro(biblioteca, usuarioLogado);
                                break;
                            case "3":
                                ConsultarCatalogo(biblioteca);
                                break;
                        }
                    }
                }
            }
        }
    }

    static void CadastrarLivros(Biblioteca biblioteca)
    {
        Console.WriteLine("Digite o título do livro:");
        string titulo = Console.ReadLine();
        Console.WriteLine("Digite o autor do livro:");
        string autor = Console.ReadLine();
        Console.WriteLine("Digite o gênero do livro:");
        string genero = Console.ReadLine();
        Console.WriteLine("Digite a quantidade disponível:");
        int quantidade = int.Parse(Console.ReadLine());

        Livro novoLivro = new Livro(titulo, autor, genero, quantidade);
        biblioteca.CadastrarLivro(novoLivro);
        Console.WriteLine("Livro cadastrado com sucesso!");
    }

    static Usuario LoginUsuario(List<Usuario> usuarios)
    {
        Console.WriteLine("Digite seu nome:");
        string nome = Console.ReadLine();

        var usuario = usuarios.FirstOrDefault(u => u.Nome == nome);
        if (usuario == null)
        {
            usuario = new Usuario(nome);
            usuarios.Add(usuario);
        }
        return usuario;
    }

    static void EmprestarLivro(Biblioteca biblioteca, Usuario usuario)
    {
        ConsultarCatalogo(biblioteca);
        Console.WriteLine("Digite o título do livro que deseja emprestar:");
        string titulo = Console.ReadLine();
        Livro livro = biblioteca.ConsultarCatalogo().FirstOrDefault(l => l.Titulo == titulo);

        if (livro != null && biblioteca.EmprestarLivro(livro, usuario))
        {
            Console.WriteLine("Livro emprestado com sucesso!");
        }
        else
        {
            Console.WriteLine("Empréstimo não realizado. Verifique se o livro está disponível ou se você já possui 3 livros emprestados.");
        }
    }

    static void DevolverLivro(Biblioteca biblioteca, Usuario usuario)
    {
        Console.WriteLine("Livros emprestados:");
        foreach (var Livro in usuario.LivrosEmprestados)
        {
            Console.WriteLine($"- {Livro.Titulo}");
        }
        Console.WriteLine("Digite o título do livro que deseja devolver:");
        string titulo = Console.ReadLine();
        Livro livro = usuario.LivrosEmprestados.FirstOrDefault(l => l.Titulo == titulo);

        if (livro != null)
        {
            biblioteca.DevolverLivro(livro, usuario);
            Console.WriteLine("Livro devolvido com sucesso!");
        }
        else
        {
            Console.WriteLine("Você não possui este livro emprestado.");
        }
    }

    static void ConsultarCatalogo(Biblioteca biblioteca)
    {
        Console.WriteLine("Catálogo da Biblioteca:");
        foreach (var livro in biblioteca.ConsultarCatalogo())
        {
            Console.WriteLine($"{livro.Titulo} - Autor: {livro.Autor} - Gênero: {livro.Genero} - Disponível: {livro.QuantidadeDisponivel}");
        }
    }
}

