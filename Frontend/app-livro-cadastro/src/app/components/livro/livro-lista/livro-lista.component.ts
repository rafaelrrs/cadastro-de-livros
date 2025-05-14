import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { Router, RouterModule } from '@angular/router'; 
import { Livro } from '../../../models/livro.model'; 
import { LivroService } from '../../../services/livro.service'; 
import { Autor } from '../../../models/autor.model';
import { Assunto } from '../../../models/assunto.model';


@Component({
  selector: 'app-livro-lista',
  standalone: true, 
  templateUrl: './livro-lista.component.html',
  imports: [CommonModule, RouterModule], 
  styleUrls: ['./livro-lista.component.css']
})
export class LivroListaComponent implements OnInit {
  livros: Livro[] = [];
  displayedColumns: string[] = ['titulo', 'editora', 'anoPublicacao', 'autores', 'assuntos', 'acoes'];
  error: string | null = null;

  constructor(private livroService: LivroService, private router: Router) {}

  ngOnInit(): void {
    this.carregarLivros();
    
  }

  carregarLivros(): void {
    this.livroService.getAll().subscribe({
      next: (dadosRecebidos: any) => {
        // console.log('Dados recebidos no COMPONENTE:', dadosRecebidos);
        if (dadosRecebidos && Array.isArray(dadosRecebidos.$values)) {
          this.livros = dadosRecebidos.$values; 
        } else if (Array.isArray(dadosRecebidos)) {
        this.livros = dadosRecebidos;
        } else {
          // console.error('Erro: A API não retornou um array!', dadosRecebidos);
          this.error = 'Formato inesperado recebido da API.';
          this.livros = [];
        }
        this.error = null;
      },
      error: (err) => {
        // console.error('Erro ao carregar livros no COMPONENTE:', err);
        this.error = `Falha ao buscar livros: ${err.message || 'Erro desconhecido'}`;
        this.livros = [];
      }
    });
  }

  editarLivro(id: number): void {
    this.router.navigate(['/livros/editar', id]);
  }

  excluirLivro(id: number): void {
    if (confirm('Tem certeza que deseja excluir este livro?')) {
      this.livroService.delete(id).subscribe(
        () => {
          this.carregarLivros();
        },
        (error) => {
          this.error = 'Erro ao excluir autor.';
          console.error('Erro ao excluir livro:', error);
        }
      );
    }
  }

  formatarAutores(autores: Autor[]): string {
    return autores.map(autor => autor.nome).join(', ');
  }

  formatarAssuntos(assuntos: Assunto[]): string {
    return assuntos.map(assunto => assunto.descricao).join(', ');
  }
}
