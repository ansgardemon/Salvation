namespace Salvation.ViewModels
{
    public class DashboardViewModel
    {
    // ====== VIEWMODEL PRINCIPAL ======



            // cards
            public int TotalFilmes { get; set; }
            public int TotalGeneros { get; set; }
            public int TotalClassificacoes { get; set; }
            public int TotalUsuarios { get; set; }

            public int FilmesComImagem { get; set; }
            public int FilmesSemImagem { get; set; }


            // Listas
            public List<FilmeItem> UltimosFilmes { get; set; } = new();
            public List<GeneroCount> GenerosMaisPopulares { get; set; } = new();

            // Gráficos
            public List<GeneroCount> FilmesPorGenero { get; set; } = new();
            public List<ClassificacaoCount> FilmesPorClassificacao { get; set; } = new();
        }

        // ====== TIPOS DE APOIO (no MESMO namespace) ======
        public class FilmeItem
        {
            public int IdFilme { get; set; }
            public string Titulo { get; set; } = "";
            public string Genero { get; set; } = "";
            public string Classificacao { get; set; } = "";
            public string UrlImagem { get; set; } = "";
        }

        public class GeneroCount
        {
            public int IdGenero { get; set; }
            public string DescricaoGenero { get; set; } = "";
            public int QtdeFilmes { get; set; }
        }

        public class ClassificacaoCount
        {
            public int IdClassificacao { get; set; }
            public string DescricaoClassificacao { get; set; } = "";
            public int QtdeFilmes { get; set; }
        }
    }


